using CoffeeJitters.HeartRateMonitor;
using CoffeeJitters.Timer.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.EventStates.FailStates
{

    public class HeartFailEventState : EventState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        [Space]
        private float thresholdHeartRate = 190f;

        private IHeartRateProvider heartRateProvider;
        private IPatienceTimerProvider patienceTimerProvider;

        #endregion Fields

        #region - - - - - - MonoBehaviour - - - - - -

        private void Start()
        {
            // Try to find if attached to object
            if (heartRateProvider == null || patienceTimerProvider == null)
            {
                heartRateProvider = DaddyManager.instance.heartRateMonitor;
                patienceTimerProvider = DaddyManager.instance.PatienceTimerProvider;
            }
        }

        private void Update()
            => this.MonitorState();

        #endregion MonoBehaviour

        #region - - - - - - Methods - - - - - -

        public override void HandleState()
            => events.Invoke();

        public override void MonitorState()
        {
            float currentHeartRate = heartRateProvider.GetHeartBeatsPerMinute();
            if (currentHeartRate > thresholdHeartRate || patienceTimerProvider.GetPatienceTimerInterpolatedValue() <= 0)
                this.HandleState();
        }

        #endregion Methods

    }

}