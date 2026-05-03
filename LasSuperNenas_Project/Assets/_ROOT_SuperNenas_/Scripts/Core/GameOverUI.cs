using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void Retry()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
}