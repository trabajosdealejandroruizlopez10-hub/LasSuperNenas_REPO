using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado del juego")]
    public bool isGameOver = false;
    public float metersRun = 0f;
    public float tiempoVivo = 0f;

    [Header("Vida")]
    public int maxHealth = 5;
    public int playerHealth;

    [Header("Velocidad")]
    public float baseSpeed = 5f;
    public float speedIncreasePerMeter = 0.01f;

    [Header("UI Game Over")]
    public GameObject panelGameOver;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        isGameOver = false;
        metersRun = 0f;
        tiempoVivo = 0f;
        playerHealth = maxHealth;

        Time.timeScale = 1f;

        
        if (panelGameOver != null)
            panelGameOver.SetActive(false);

       
        ActualizarUIVida();

        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            float velocidad = GetCurrentSpeed();

            metersRun += Time.deltaTime * velocidad;
            tiempoVivo += Time.deltaTime;

            
            if (TimerUI.Instance != null)
                TimerUI.Instance.ActualizarTiempo(tiempoVivo);
        }
    }

    public float GetCurrentSpeed()
    {
        return baseSpeed + (metersRun * speedIncreasePerMeter);
    }

    public void TakeDamage(int amount)
    {
        if (isGameOver) return;

        playerHealth -= amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);

        ActualizarUIVida();

        
        if (PlayerUI.Instance != null)
            PlayerUI.Instance.MostrarDaño();

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        if (isGameOver) return;

        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);

        ActualizarUIVida();
    }

    void ActualizarUIVida()
    {
        if (PlayerUI.Instance != null)
        {
            float vidaNormalizada = (float)playerHealth / maxHealth;
            PlayerUI.Instance.ActualizarVida(vidaNormalizada);
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        Debug.Log("GAME OVER - Tiempo: " + Mathf.FloorToInt(tiempoVivo) + "s");

        
        if (panelGameOver != null)
            panelGameOver.SetActive(true);

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}