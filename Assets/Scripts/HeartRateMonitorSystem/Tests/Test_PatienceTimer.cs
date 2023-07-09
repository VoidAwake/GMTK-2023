using CoffeeJitters.HeartRateMonitor.Services;
using CoffeeJitters.Timer.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor.Tests
{

    public class Test_PatienceTimer : MonoBehaviour, IPatienceTimerProvider
    {

        public float maxPatience = 1f;
        public float currentPatience;

        public float GetMaxPatienceTime()
        {
            return maxPatience;
        }

        public float GetPatienceTimerInterpolatedValue()
            => Mathf.Clamp(maxPatience - currentPatience, 0f, maxPatience) / maxPatience;

        void Start()
        {
            currentPatience = maxPatience;

            HeartRateMonitor heartRateMonitor = this.GetComponent<HeartRateMonitor>();
            heartRateMonitor.InitialiseHeartMonitor(this.GetComponent<IHeartRateECG>(), this.GetComponent<IInputValueTimeoutProvider>(), this);
        }

        // Update is called once per frame
        void Update()
        {
            currentPatience -= Time.deltaTime;
        }

    }

}
