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

    private Vector2 inputMovimiento;
    private bool estaDisparando = false;
    private float tiempoProximoDisparo = 0f;
    private bool estaEnSuelo = false;
    private Camera camara;
    private Rigidbody rb;

    void Start()
    {
        camara = Camera.main;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        MoverLateral();

        if (estaDisparando && Time.time >= tiempoProximoDisparo)
        {
            tiempoProximoDisparo = Time.time + cadenciaDisparo;
            Disparar();
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
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) estaDisparando = true;
        if (context.canceled) estaDisparando = false;
    }

    void MoverLateral()
    {
        Vector3 movimiento = new Vector3(inputMovimiento.x * velocidadLateral * Time.deltaTime, 0f, 0f);
        transform.Translate(movimiento);

        float x = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void Disparar()
    {
        Ray rayo = new Ray(camara.transform.position, camara.transform.forward);
        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, alcanceDisparo, capaEnemigos))
        {
            EnemyHealth enemigo = impacto.collider.GetComponent<EnemyHealth>();
            if (enemigo != null)
                enemigo.RecibirDaño(1);

            if (efectoImpacto != null)
                Instantiate(efectoImpacto, impacto.point, Quaternion.identity);
        }
    }
}