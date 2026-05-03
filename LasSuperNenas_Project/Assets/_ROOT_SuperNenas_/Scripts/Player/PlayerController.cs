using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento lateral")]
    public float velocidadLateral = 4f;
    public float limiteX = 4f;

    [Header("Salto")]
    public float fuerzaSalto = 5f;

    [Header("Disparo")]
    public float alcanceDisparo = 50f;
    public float cadenciaDisparo = 0.2f;
    public LayerMask capaEnemigos;
    public GameObject efectoImpacto;

    [Header("Munición")]
    public int balasPorCargador = 10;
    public float tiempoRecarga = 1.5f;

    private int balasActuales;
    private bool recargando = false;

    [Header("Efectos de disparo")]
    public AudioSource sonidoDisparo;
    public AudioSource sonidoRecarga;
    public Transform arma;
    public float recoilFuerza = 0.1f;
    public float recoilVelocidad = 10f;

    [Header("Animación recarga")]
    public float velocidadRotacionRecarga = 720f; // grados por segundo

    private Vector2 inputMovimiento;
    private bool estaDisparando = false;
    private float tiempoProximoDisparo = 0f;
    private bool estaEnSuelo = false;

    private Camera camara;
    private Rigidbody rb;

    private Vector3 posicionInicialArma;
    private Quaternion rotacionInicialArma;

    void Start()
    {
        camara = Camera.main;
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        balasActuales = balasPorCargador;

        if (arma != null)
        {
            posicionInicialArma = arma.localPosition;
            rotacionInicialArma = arma.localRotation;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        Mover();

        if (Keyboard.current.rKey.wasPressedThisFrame && !recargando && balasActuales < balasPorCargador)
        {
            StartCoroutine(Recargar());
        }

        if (estaDisparando && Time.time >= tiempoProximoDisparo && !recargando)
        {
            if (balasActuales > 0)
            {
                tiempoProximoDisparo = Time.time + cadenciaDisparo;
                Disparar();
            }
            else
            {
                StartCoroutine(Recargar());
            }
        }

        // Recuperación del recoil (posición)
        if (arma != null && !recargando)
        {
            arma.localPosition = Vector3.Lerp(
                arma.localPosition,
                posicionInicialArma,
                Time.deltaTime * recoilVelocidad
            );

            arma.localRotation = Quaternion.Lerp(
                arma.localRotation,
                rotacionInicialArma,
                Time.deltaTime * recoilVelocidad
            );
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            estaEnSuelo = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            estaEnSuelo = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovimiento = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && estaEnSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) estaDisparando = true;
        if (context.canceled) estaDisparando = false;
    }

    void Mover()
    {
        float velocidad = GameManager.Instance != null ? GameManager.Instance.GetCurrentSpeed() : 5f;

        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        Vector3 movimiento = new Vector3(inputMovimiento.x * velocidadLateral * Time.deltaTime, 0f, 0f);
        transform.Translate(movimiento);

        float x = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void Disparar()
    {
        balasActuales--;

        if (sonidoDisparo != null)
            sonidoDisparo.Play();

        if (arma != null)
            arma.localPosition -= new Vector3(0f, 0f, recoilFuerza);

        Ray rayo = new Ray(camara.transform.position, camara.transform.forward);
        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, alcanceDisparo, capaEnemigos))
        {
            EnemyHealth enemigo = impacto.collider.GetComponent<EnemyHealth>();

            if (enemigo != null)
                enemigo.TakeDamage(1);

            if (efectoImpacto != null)
                Instantiate(efectoImpacto, impacto.point, Quaternion.identity);
        }
    }

    IEnumerator Recargar()
    {
        recargando = true;

        if (sonidoRecarga != null)
            sonidoRecarga.Play();

        float tiempo = 0f;

        while (tiempo < tiempoRecarga)
        {
            tiempo += Time.deltaTime;

            // 🔄 Rotación del arma sobre su eje
            if (arma != null)
            {
                arma.Rotate(Vector3.forward * velocidadRotacionRecarga * Time.deltaTime);
            }

            yield return null;
        }

        balasActuales = balasPorCargador;

        // Reset rotación al final
        if (arma != null)
            arma.localRotation = rotacionInicialArma;

        recargando = false;
    }
}