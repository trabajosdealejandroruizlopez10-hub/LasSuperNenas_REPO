using UnityEngine;

public class Fade : MonoBehaviour
{
    public CanvasGroup cg;

    void Update()
    {
        cg.alpha -= Time.deltaTime;
    }
}