using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado del juego")]
    public bool isGameOver = false;
    public float metersRun = 0f;

    [Header("Vida")]
    public int maxHealth = 5;
    public int playerHealth;

    [Header("Velocidad")]
    public float baseSpeed = 5f;
    public float speedIncreasePerMeter = 0.01f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        isGameOver = false;
        metersRun = 0f;
        playerHealth = maxHealth;

        Time.timeScale = 1f;

        ActualizarUI();
    }

    void Update()
    {
        if (!isGameOver)
        {
            metersRun += Time.deltaTime * GetCurrentSpeed();
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

        ActualizarUI();

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

        ActualizarUI();
    }

    void ActualizarUI()
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

        Debug.Log("GAME OVER - Metros: " + Mathf.FloorToInt(metersRun));

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}