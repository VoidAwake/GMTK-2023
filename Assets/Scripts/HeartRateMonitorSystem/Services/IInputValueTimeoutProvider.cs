using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.HeartRateMonitor.Services
{

    public interface IInputValueTimeoutProvider
    {

        #region - - - - - - Methods - - - - - -

        float GetInputTimeoutValue();

        #endregion Methods

    }

}
