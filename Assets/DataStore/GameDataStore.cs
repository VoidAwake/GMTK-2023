using System.Collections;
using System.Collections.Generic;
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

    private BaristaDialogue baristaDialogueScriptableObject;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    DialogueObject IGameDataStore.GetDialogueObjectByIdentifier(string indentifier)
    {
        throw new System.NotImplementedException();
    }

    string IGameDataStore.GetDialogueQuestionByIdentifier(string indentifier)
    {
        throw new System.NotImplementedException();
    }

    List<string> IGameDataStore.GetDialogueResponsesByIdentifier(string indentifier)
    {
        throw new System.NotImplementedException();
    }

    #endregion Methods

}
