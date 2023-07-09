using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.Timer.Services
{

    public interface IPatienceTimerProvider
    {

        #region - - - - - - Methods - - - - - -

        float GetCurrentPatienceTime();

        float GetMaxPatienceTime();

        #endregion Methods

    }

}
