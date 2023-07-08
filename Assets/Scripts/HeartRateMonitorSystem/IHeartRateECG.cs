using CoffeeJitters.HeartRateMonitor.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor
{

    public interface IHeartRateECG
    {

        #region - - - - - - Methods - - - - - -

        void TriggerHeartRateECG(HeartState heartState);

        #endregion Methods

    }

}
