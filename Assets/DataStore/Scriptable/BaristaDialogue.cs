using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaristaDialogue", menuName = "Barista Dialogue")]
public class BaristaDialogue : ScriptableObject
{

    #region - - - - - - Fields - - - - - -

    [Header("Barista Dialogue")]
    [Space]
    public List<DialogueObject> dialogueObjects;

    #endregion Fields

}

