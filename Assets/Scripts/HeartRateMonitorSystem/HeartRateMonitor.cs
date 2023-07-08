using CoffeeJitters.HeartRateMonitor.Entities;
using CoffeeJitters.Timer;
using CoffeeJitters.Timer.Services;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor
{

    public class HeartRateMonitor : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        private float currentHeartRate = 80f;
        [SerializeField]
        private float maxHeartRate;
        [SerializeField]
        private float minHeartRate;
        [SerializeField]
        private float timerSpeed = 1f;

        private IHeartRateECG heartRateECG;
        private IPatienceTimerProvider patienceTimerProvider;
        private SimpleTimer simpleTimer;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        /// <summary>
        /// Initialises and a place to perform Dependency Injection to this behaviour. This occurs during the 'Awake' event funciton.
        /// </summary>
        public void InitialiseHeartMonitor(IHeartRateECG heartRateECG, IPatienceTimerProvider patienceTimerProvider)
        {
            this.heartRateECG = heartRateECG;
            this.patienceTimerProvider = patienceTimerProvider;
        }

        private void Start() =>
            this.simpleTimer = new SimpleTimer(
                                (1f / (currentHeartRate / 60)) * timerSpeed,
                                () => this.heartRateECG.TriggerHeartRateECG(new HeartState()));

        private void Update()
        {
            // Shift interpolation value based on the patience value
            float anxietyLevel = Mathf.Clamp(
                patienceTimerProvider.GetMaxPatienceTime() - patienceTimerProvider.GetCurrentPatienceTime(),
                0f,
                patienceTimerProvider.GetMaxPatienceTime());

            float anxiousHeartRateBPM = Mathf.Lerp(minHeartRate, maxHeartRate, EaseInCirc(anxietyLevel / patienceTimerProvider.GetMaxPatienceTime()));

            float heartRate = 60f / anxiousHeartRateBPM * 1000f;

            this.simpleTimer.IntervalLength =  heartRate;

            // Run the heartbeat
            if (this.simpleTimer.CheckTimeIsUp())
            {
                this.simpleTimer.PerformCompletionAction();
                this.simpleTimer.ResetTimer();
            }

            this.simpleTimer.TickTimer(Time.deltaTime);
        }

        private float EaseInCubic(float value)
        {
            return value * value * value;
        }

        private float EaseInCirc(float value)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(value, 2));
        }

        #endregion Methods

    }

}