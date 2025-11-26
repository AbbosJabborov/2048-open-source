using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public float totalTime = 60f;
    public Image fillImage;   // radial or bar image
    public bool isRunning = false;

    public System.Action OnTimerEnd;

    private float timeLeft;

    void Start()
    {
        timeLeft = totalTime;
        UpdateVisual();
    }

    void Update()
    {
        if (!isRunning) return;
        timeLeft -= Time.deltaTime;
        UpdateVisual();

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            isRunning = false;
            OnTimerEnd?.Invoke();
        }
    }

    void UpdateVisual()
    {
        if (fillImage != null)
            fillImage.fillAmount = timeLeft / totalTime;
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public float GetRemainingTime() => timeLeft;
}