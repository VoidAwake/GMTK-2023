using CoffeeJitters.DataStore.Entities;
using CoffeeJitters.DataStore.Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace CoffeeJitters.DataStore
{

    public interface IGameDataStore
    {

        #region - - - - - - Methods - - - - - -

        DialogueObject GetDialogueObjectByIdentifier(string indentifier);

        #endregion Methods

    }

    public class GameDataStore : MonoBehaviour, IGameDataStore
    {

        #region - - - - - - Fields - - - - - -

        public BaristaDialogue baristaDialogueScriptableObject;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        DialogueObject IGameDataStore.GetDialogueObjectByIdentifier(string indentifier)
        {
            if (CheckBaristaDialogueScriptableObjectIsValid())
                return null;

            var dialogueObjectByIdentifier = baristaDialogueScriptableObject.dialogueObjects
                .Where(bdo => bdo.identifier == indentifier)
                .SingleOrDefault();
            
            if (dialogueObjectByIdentifier == null)
                Debug.LogWarning($"No dialogue found for {indentifier}");
            
            return dialogueObjectByIdentifier;
        }

        bool CheckBaristaDialogueScriptableObjectIsValid()
        {
            if(baristaDialogueScriptableObject == null)
                Debug.LogWarning("Barista Dialogue Scriptable Object not loaded.", baristaDialogueScriptableObject);

            return baristaDialogueScriptableObject == null;
        }

        #endregion Methods

    }

}