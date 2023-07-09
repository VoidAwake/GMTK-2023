using CoffeeJitters.HeartRateMonitor;
using CoffeeJitters.HeartRateMonitor.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor.Tests
{
    public class Test_HeartRateECG : MonoBehaviour, IHeartRateECG
    {

        public void TriggerHeartRateECG(HeartState heartState)
        {
            Debug.Log("Heart Beat");
        }

    }
}
