using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.Timer.Services
{

    public interface IPatienceTimerProvider
    {

        #region - - - - - - Methods - - - - - -

        float GetPatienceTimerInterpolatedValue();

        #endregion Methods

    }

}
