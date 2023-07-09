using CoffeeJitters.HeartRateMonitor.Entities;
using CoffeeJitters.HeartRateMonitor.Services;
using CoffeeJitters.Timer;
using CoffeeJitters.Timer.Services;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor
{

    public interface IHeartRateProvider
    {

        #region - - - - - - Methods - - - - - -

        float GetHeartBeatsPerMinute();

        #endregion Methods

    }

    public interface IHeartRateModifier
    {

        #region - - - - - - Methods - - - - - -

        void EnableHeartbeatValueMonitoring();

        void DisableHeartbeatValueMonitoring();

        #endregion Methods

    }

    public class HeartRateMonitor : MonoBehaviour, IHeartRateProvider, IHeartRateModifier
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private float currentHeartRate = 80f;
        [SerializeField]
        private float maxHeartRate;
        [SerializeField]
        private float minHeartRate;
        [SerializeField]
        private float timerSpeed = 1f;
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Balance between interpolation of patience and inputTimeOut")]
        private float interpolationBalance = 0.5f;
        [SerializeField]
        [Range(1f, 5f)]
        private float beatEventSpeed = 1f;
        [SerializeField]
        private bool enableValueTracking;

        private IHeartRateECG heartRateECG;
        private IInputValueTimeoutProvider inputValueTimeoutProvider;
        private IPatienceTimerProvider patienceTimerProvider;
        private SimpleTimer simpleTimer;

        #endregion Fields

        #region - - - - - - MonoBehaviour - - - - - -

        private void Start() =>
            this.simpleTimer =
                new SimpleTimer(
                    (1f / (currentHeartRate / 60)) * timerSpeed,
                    () =>
                    {
                        if (this.heartRateECG == null)
                            return;

                        this.heartRateECG.TriggerHeartRateECG(new HeartState()
                        {
                            BaseHeartRate = minHeartRate,
                            HeartRate = currentHeartRate,
                            MaxHeartRate = maxHeartRate
                        });
                    });

        private void Update()
        {
            if (enableValueTracking)
                this.CalculateCurrentHeartRate();

            // Run the heartbeat
            if (this.simpleTimer.CheckTimeIsUp())
            {
                this.simpleTimer.PerformCompletionAction();
                this.simpleTimer.ResetTimer();
            }

            this.simpleTimer.TickTimer(Time.deltaTime);
        }

        #endregion MonoBehaviour

        #region - - - - - - Methods - - - - - -

        /// <summary>
        /// Initialises and a place to perform Dependency Injection to this behaviour. This occurs during the 'Awake' event funciton.
        /// </summary>
        public void InitialiseHeartMonitor(
            IHeartRateECG heartRateECG,
            IInputValueTimeoutProvider inputValueTimeoutProvider,
            IPatienceTimerProvider patienceTimerProvider)
        {
            this.heartRateECG = heartRateECG;
            this.inputValueTimeoutProvider = inputValueTimeoutProvider;
            this.patienceTimerProvider = patienceTimerProvider;
        }

        private void CalculateCurrentHeartRate()
        {
            float mixedInterpolatedValue = Mathf.Lerp(
                                                patienceTimerProvider.GetPatienceTimerInterpolatedValue(),
                                                this.inputValueTimeoutProvider.GetInputTimeoutValue(),
                                                interpolationBalance);

            currentHeartRate = Mathf.Lerp(minHeartRate, maxHeartRate, this.EaseInOutQuad(mixedInterpolatedValue));

            this.simpleTimer.IntervalLength = (1 / (currentHeartRate / 60f)) / beatEventSpeed;
        }

        private float EaseInOutQuad(float value)
            => value < 0.5f ? 2 * value * value : 1 - Mathf.Pow(-2 * value + 2, 2) / 2;

        public float GetHeartBeatsPerMinute()
            => currentHeartRate;

        public void EnableHeartbeatValueMonitoring()
            => enableValueTracking = true;

        public void DisableHeartbeatValueMonitoring()
        {
            enableValueTracking = false;

            currentHeartRate = minHeartRate;
        }

        /// <summary>
        /// Debugger console output for value monitoring.
        /// </summary>
        private void HeartBeatDebugger(float patienceValue, float inputTimeoutValue, float mixedInterpolatedValue, float heartRatePerSecond, float heartRateInMilliseconds)
            => Debug.Log($" {patienceValue}, {inputTimeoutValue}, {mixedInterpolatedValue}, {currentHeartRate}, {heartRatePerSecond}, {heartRateInMilliseconds}");

        #endregion Methods

    }

}