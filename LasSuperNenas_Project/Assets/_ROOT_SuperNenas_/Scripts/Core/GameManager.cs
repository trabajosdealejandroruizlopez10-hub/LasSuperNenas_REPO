using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado del juego")]
    public bool isGameOver = false;
    public float metersRun = 0f;
    public int playerHealth = 3;

    [Header("Configuración de dificultad")]
    public float baseSpeed = 5f;
    public float speedIncreasePerMeter = 0.01f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isGameOver)
            metersRun += Time.deltaTime * GetCurrentSpeed();
    }

    public float GetCurrentSpeed()
    {
        return baseSpeed + (metersRun * speedIncreasePerMeter);
    }

    public void TakeDamage(int amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0) GameOver();
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER - Metros: " + Mathf.FloorToInt(metersRun));
        if (UIManager.Instance != null)
            UIManager.Instance.MostrarGameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
