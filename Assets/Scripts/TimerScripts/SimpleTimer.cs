using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.Timer
{

    public class SimpleTimer
    {

        #region - - - - - - Fields - - - - - -

        private float intervalLength;
        private float timeLeft;
        private Action timerCompletionAction;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public SimpleTimer(float intervalLength, Action timerCompletionAction)
        {
            this.intervalLength = intervalLength;
            this.timeLeft = intervalLength;
            this.timerCompletionAction = timerCompletionAction;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public float IntervalLength
        {
            get { return intervalLength; }
            set { intervalLength = value; }
        }

        public float TimeLeft { get => timeLeft; }


        #endregion Properties

        #region - - - - - - Methods - - - - - -

        public bool CheckTimeIsUp()
            => timeLeft <= 0;

        public void PerformCompletionAction()
            => timerCompletionAction?.Invoke();

        public void ResetTimer()
            => timeLeft = intervalLength;

        public void TickTimer(float deltaTime)
            => this.timeLeft -= deltaTime;

        #endregion Methods

    }

}