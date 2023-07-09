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

        float IPatienceTimerProvider.GetCurrentPatienceTime()
        {
            return currentPatience;
        }

        void Start()
        {
            currentPatience = maxPatience;

            HeartRateMonitor heartRateMonitor = this.GetComponent<HeartRateMonitor>();
            heartRateMonitor.InitialiseHeartMonitor(this.GetComponent<IHeartRateECG>(), this);
        }

        // Update is called once per frame
        void Update()
        {
            currentPatience -= Time.deltaTime;
        }

    }

}
