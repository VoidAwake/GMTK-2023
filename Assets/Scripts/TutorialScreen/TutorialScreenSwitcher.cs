using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoffeeJitters.TutorialScreen
{

    public class TutorialScreenSwitcher : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        public UnityEvent OnStart;
        public UnityEvent OnTransition;
        public UnityEvent OnClose;

        #endregion Fields

        #region - - - - - - MonoBehaviour - - - - - -

        private void OnEnable() 
            => this.OnScreenStart();

        #endregion MonoBehaviour

        #region - - - - - - Methods - - - - - -

        public void OnScreenStart() 
            => OnStart.Invoke();

        public void OnInputToggle() 
            => OnTransition.Invoke();

        public void OnScreenClose() 
            => OnClose.Invoke();

        #endregion Methods

    }

}