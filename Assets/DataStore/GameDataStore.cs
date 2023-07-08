using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public interface IGameDataStore
{

    #region - - - - - - Methods - - - - - -

    DialogueObject GetDialogueObjectByIdentifier(string indentifier);

    string GetDialogueQuestionByIdentifier(string indentifier);

    List<string> GetDialogueResponsesByIdentifier(string indentifier);

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
        if (baristaDialogueScriptableObject == null)
        {
            Debug.LogWarning("Barista Dialogue Scriptable Object not loaded.", baristaDialogueScriptableObject);
            return null;
        }

        return baristaDialogueScriptableObject.dialogueObjects
            .Where(bdo => bdo.identifier == indentifier)
            .SingleOrDefault();
    }

    string IGameDataStore.GetDialogueQuestionByIdentifier(string indentifier)
    {
        if (baristaDialogueScriptableObject == null)
        {
            Debug.LogWarning("Barista Dialogue Scriptable Object not loaded.", baristaDialogueScriptableObject);
            return null;
        }

        return baristaDialogueScriptableObject.dialogueObjects
            .Where(bdo => bdo.identifier == indentifier)
            .Select(bdo => bdo.question)
            .SingleOrDefault();
    }

    List<string> IGameDataStore.GetDialogueResponsesByIdentifier(string indentifier)
    {
        if (baristaDialogueScriptableObject == null)
        {
            Debug.LogWarning("Barista Dialogue Scriptable Object not loaded.", baristaDialogueScriptableObject);
            return null;
        }

        return baristaDialogueScriptableObject.dialogueObjects
            .Where(bdo => bdo.identifier == indentifier)
            .Select(bdo => bdo.response)
            .SingleOrDefault();
    }

    #endregion Methods

}
