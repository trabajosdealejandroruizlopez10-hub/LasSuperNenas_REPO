using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento lateral")]
    public float velocidadLateral = 10f;
    public float limiteX = 8f;

    [Header("Movimiento hacia delante")]
    public float velocidadBase = 10f;

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
    public float velocidadRotacionRecarga = 720f;

    private Vector2 inputMovimiento;
    private bool estaDisparando = false;
    private float tiempoProximoDisparo = 0f;

    private Camera camara;

    private Vector3 posicionInicialArma;
    private Quaternion rotacionInicialArma;

    void Start()
    {
        camara = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        balasActuales = balasPorCargador;

        if (arma != null)
        {
            posicionInicialArma = arma.localPosition;
            rotacionInicialArma = arma.localRotation;
        }

        if (AmmoUI.Instance != null)
            AmmoUI.Instance.ActualizarMunicion(balasActuales, balasPorCargador);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

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

    void Mover()
    {
        float velocidad = GameManager.Instance != null ? GameManager.Instance.GetCurrentSpeed() : velocidadBase;

        Vector3 posicion = transform.position;

        posicion += Vector3.forward * velocidad * Time.deltaTime;

        float direccion = 0f;
        if (inputMovimiento.x > 0.1f) direccion = 1f;
        else if (inputMovimiento.x < -0.1f) direccion = -1f;

        posicion.x += direccion * velocidadLateral * Time.deltaTime;

        posicion.x = Mathf.Clamp(posicion.x, -limiteX, limiteX);

        transform.position = posicion;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovimiento = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) estaDisparando = true;
        if (context.canceled) estaDisparando = false;
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

        if (AmmoUI.Instance != null)
            AmmoUI.Instance.ActualizarMunicion(balasActuales, balasPorCargador);
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

            if (arma != null)
                arma.Rotate(Vector3.forward * velocidadRotacionRecarga * Time.deltaTime);

            yield return null;
        }

        balasActuales = balasPorCargador;

        if (arma != null)
            arma.localRotation = rotacionInicialArma;

        recargando = false;

        if (AmmoUI.Instance != null)
            AmmoUI.Instance.ActualizarMunicion(balasActuales, balasPorCargador);
    }
}