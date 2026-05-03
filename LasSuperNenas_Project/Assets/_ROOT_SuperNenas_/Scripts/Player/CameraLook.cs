using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensibilidad = 200f;

    [Header("Límites")]
    public float limiteVertical = 80f;
    public float limiteHorizontal = 60f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotacionY = transform.localEulerAngles.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        // Vertical
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -limiteVertical, limiteVertical);

        // Horizontal (sin tocar el player)
        rotacionY += mouseX;
        rotacionY = Mathf.Clamp(rotacionY, -limiteHorizontal, limiteHorizontal);

        transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);
    }
}