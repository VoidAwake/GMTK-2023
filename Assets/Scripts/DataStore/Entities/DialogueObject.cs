using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.DataStore.Entities
{

    [System.Serializable]
    public class DialogueObject
    {

        #region - - - - - - Fields - - - - - -

        /// <summary>
        /// The unique identifier for the dialogue object.
        /// </summary>
        public string identifier;

        /// <summary>
        /// The question literal for the dialogue.
        /// </summary>
        public List<string> questions = new();

        /// <summary>
        /// The optional response literals for additional dialogue.
        /// </summary>
        public List<string> responses = new();

        #endregion Fields

    }

}
