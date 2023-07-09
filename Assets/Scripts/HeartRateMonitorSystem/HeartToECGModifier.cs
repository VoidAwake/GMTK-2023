using CoffeeJitters.HeartRateMonitor.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor
{

    public class HeartToECGModifier : MonoBehaviour, IHeartRateECG
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private EcgAnimation ecgAnimation;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        void IHeartRateECG.TriggerHeartRateECG(HeartState heartState)
            => ecgAnimation.currentPanic = (heartState.HeartRate - heartState.BaseHeartRate) / (heartState.MaxHeartRate - heartState.BaseHeartRate);

        #endregion Methods

    }

}