using CoffeeJitters.Timer.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour, IPatienceTimerProvider
{
    [Tooltip("How long the order should last (in seconds)")]
    [SerializeField] private float maxPatienceTime = 1.0f;
    private float currentPatience = 0;
    private bool isRunning = false;

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        StartTimer(maxPatienceTime);
    }

    public void StartTimer(float overrideMaxPatienceTime)
    {
        Debug.Log("Timer start");

        isRunning = true;

        currentPatience = overrideMaxPatienceTime;
    }

    private void Update()
    {
        if(isRunning)
            ReducePatience();
    }

    private void ReducePatience()
    {
        currentPatience -= Time.deltaTime;

        if (currentPatience <= 0)
            OutOfPatience();
    }

    private void OutOfPatience()
    {
        currentPatience = 0;
        isRunning = false;

        Debug.Log("Out of patience, you lose");
    }

    /// <summary>
    /// Produces patience value between 0 and 1.
    /// </summary>
    float IPatienceTimerProvider.GetPatienceTimerInterpolatedValue()
        => Mathf.Clamp(maxPatienceTime - currentPatience, 0f, maxPatienceTime) / maxPatienceTime;

}
