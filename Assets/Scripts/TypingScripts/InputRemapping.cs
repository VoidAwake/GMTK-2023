using TMPro;
using UnityEngine;

public class InputRemapping : MonoBehaviour
{
    [Header("Remap Parameters")]
    [SerializeField] private REMAP_TYPE remapType;
    [SerializeField] private float remapPercentage = 0.05f;

    [Header("Assign in Inspector")]
    [SerializeField] private TMP_InputField inputField;
    
    private string currentText = "";
    private string previousText = "";
    private string lastTypedCharacter;
    private int lastTypedCharacterPosition = 0;

    private bool isProgramChangingText;
    
    public void OnLetterTyped()
    {
        currentText = inputField.text;
        
        // If the change is made by the program, don't do anything
        if (isProgramChangingText)
        {
            isProgramChangingText = false;
            return;
        }
        
        GetLastTypedCharacter();
        RandomiseLetterChange();
        previousText = currentText;
    }
    
    private void GetLastTypedCharacter()
    {
        if (currentText.Length > previousText.Length)
        {
            for (int i = 0; i < currentText.Length; i++)
            {
                if (i >= previousText.Length || currentText[i] != previousText[i])
                {
                    lastTypedCharacter = currentText[i].ToString();
                    lastTypedCharacterPosition = i;
                    //Debug.Log("Last typed character pos: " + lastTypedCharacterPosition);
                    break;
                }
            }
        }
        previousText = currentText;
    }

    // private void CompareToPreviousText()
    // {
    //     currentText = inputField.text;
    //     
    //     // Only run if a new letter is added AND if the string is greater than 1
    //     if (currentText.Length > previousText.Length && currentText.Length > 1)
    //     {
    //         // Loop through each letter
    //         for (int i = 0; i < currentText.Length; i++)
    //         {
    //             // Find the position of the character that was added
    //             if (i >= previousText.Length || currentText[i] != previousText[i])
    //             {
    //                 // Don't try to replace spaces
    //                 if (currentText[i].ToString() != " ")
    //                 {
    //                     lastTypedCharacterPosition = i;
    //                     RandomiseLetterChange();
    //                     break;
    //                 }
    //             }
    //         }
    //     }
    // }

    private void RandomiseLetterChange()
    {
        if (remapType == REMAP_TYPE.REPLACE_ALL)
        {
            float rand = Random.Range(0.0f, 1.0f);
            
            if (rand < remapPercentage)
            {
                ReplaceLastTypedCharacter();
            }
        }
    }

    private void ReplaceLastTypedCharacter()
    {
        // Don't bother if the string is less than 1 character long
        if (currentText.Length <= 1)
            return;
        
        // Don't bother if the last typed character is a space
        if (lastTypedCharacter == " ")
            return;
        
        Debug.Log(currentText.Length);
        
        // Cut the last letter and replace it
        int replacementPosition = lastTypedCharacterPosition - 1;
        
        isProgramChangingText = true;
        
        string newText;
        Debug.Log(replacementPosition);
        newText = currentText.Substring(0, lastTypedCharacterPosition);
        newText += "z";
        //newText += currentText.Substring(lastTypedCharacterPosition + 1, currentText.Length - newText.Length + 2);
        
        inputField.text = newText;
        
        // Move the "Caret" to the end so the player keeps typing
        //inputField.MoveTextEnd(false);
    }
}
