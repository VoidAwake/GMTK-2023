using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaristaDialogue", menuName = "Barista Dialogue")]
public class BaristaDialogue : ScriptableObject
{

    // bunch of question
    public List<DialogueObject> dialogueObjects;

}

// Test placement of entities:
[System.Serializable]
public class DialogueObject
{
    public string identifier;
    public string question;
    public List<string> response;
}

