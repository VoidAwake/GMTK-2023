using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor.Services
{

    public class Test_InputTimeoutService : MonoBehaviour, IInputValueTimeoutProvider
    {

        public float currentTimeoutValue = 0f;

        private void Update()
        {
            currentTimeoutValue += Time.deltaTime;
        }

        public float GetInputTimeoutValue()
        {
            return Mathf.Clamp(currentTimeoutValue, 0, 1);
        }

    }

}