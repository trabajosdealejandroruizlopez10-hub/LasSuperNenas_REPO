using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado del juego")]
    public bool isGameOver = false;
    public float metersRun = 0f;
    public int playerHealth = 5;

    [Header("Configuración de dificultad")]
    public float baseSpeed = 5f;
    public float speedIncreasePerMeter = 0.01f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        if (isGameOver) return;

        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 5);
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER - Metros: " + Mathf.FloorToInt(metersRun));

        if (UIManager.Instance != null)
        {
            UIManager.Instance.MostrarGameOver();
        }

        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        isGameOver = true;
        Debug.Log("YOU WIN");

        if (UIManager.Instance != null)
        {
            UIManager.Instance.MostrarVictoria();
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}