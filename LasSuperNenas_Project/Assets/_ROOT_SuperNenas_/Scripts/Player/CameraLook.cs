using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensibilidad = 200f;

    [Header("Referencias")]
    public Transform playerBody;

    private float rotacionX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        // Rotación vertical (arriba/abajo)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        // Rotación horizontal (izquierda/derecha)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}