using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Efectos de disparo")]
    public AudioSource sonidoDisparo;
    public Transform arma;
    public float recoilFuerza = 0.1f;
    public float recoilVelocidad = 10f;

    private Vector2 inputMovimiento;
    private bool estaDisparando = false;
    private float tiempoProximoDisparo = 0f;
    private bool estaEnSuelo = false;

    private Camera camara;
    private Rigidbody rb;

    private Vector3 posicionInicialArma;

    void Start()
    {
        camara = Camera.main;
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (arma != null)
        {
            posicionInicialArma = arma.localPosition;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        Mover();

        if (estaDisparando && Time.time >= tiempoProximoDisparo)
        {
            tiempoProximoDisparo = Time.time + cadenciaDisparo;
            Disparar();
        }

        // Volver arma a posición original (recoil recovery)
        if (arma != null)
        {
            arma.localPosition = Vector3.Lerp(
                arma.localPosition,
                posicionInicialArma,
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
        float velocidad = GameManager.Instance.GetCurrentSpeed();

        // Movimiento hacia delante automático
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        // Movimiento lateral
        Vector3 movimiento = new Vector3(inputMovimiento.x * velocidadLateral * Time.deltaTime, 0f, 0f);
        transform.Translate(movimiento);

        float x = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void Disparar()
    {
        // Sonido
        if (sonidoDisparo != null)
        {
            sonidoDisparo.Play();
        }

        // Recoil
        if (arma != null)
        {
            arma.localPosition -= new Vector3(0f, 0f, recoilFuerza);
        }

        Ray rayo = new Ray(camara.transform.position, camara.transform.forward);
        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, alcanceDisparo, capaEnemigos))
        {
            EnemyHealth enemigo = impacto.collider.GetComponent<EnemyHealth>();

            if (enemigo != null)
            {
                enemigo.TakeDamage(1);
            }

            if (efectoImpacto != null)
            {
                Instantiate(efectoImpacto, impacto.point, Quaternion.identity);
            }
        }
    }
}