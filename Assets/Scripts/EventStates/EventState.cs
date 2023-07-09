using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CoffeeJitters.EventStates
{

    public interface IEventState
    {

        #region - - - - - - Methods - - - - - -

        void HandleState();

        void MonitorState();

        #endregion Methods

    }

    public abstract class EventState : MonoBehaviour, IEventState
    {

        #region - - - - - - Fields - - - - - -

        [Space]
        public UnityEvent events = new();

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        public virtual void HandleState() { }

        public virtual void MonitorState() { }

        #endregion Methods

    }

}
