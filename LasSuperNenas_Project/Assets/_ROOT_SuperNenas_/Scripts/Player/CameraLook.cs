using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensibilidad = 200f;

    [Header("Referencias")]
    public Transform playerBody;

    [Header("Límites")]
    public float limiteVertical = 80f;
    public float limiteHorizontal = 60f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotacionY = playerBody.eulerAngles.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        // Rotación vertical (arriba/abajo)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -limiteVertical, limiteVertical);

        // Rotación horizontal (izquierda/derecha con límite)
        rotacionY += mouseX;
        rotacionY = Mathf.Clamp(rotacionY, -limiteHorizontal, limiteHorizontal);

        // Aplicar rotaciones
        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        playerBody.rotation = Quaternion.Euler(0f, rotacionY, 0f);
    }
}