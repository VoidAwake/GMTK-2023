using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class InputRemapping : MonoBehaviour
{
    [Header("Remap Parameters")]
    [SerializeField] private REMAP_TYPE remapType;
    
    // [SerializeField] private float remapPercentage = 0.05f;
    
    // I use strings because it's nicer to store spaces
    [SerializeField] private string[] vowels = {"a", "e", "i", "o", "u"};
    private string[] newVowelOrder = {"a", "e", "i", "o", "u"};
    
    [SerializeField] private int numberOfRemaps = 1;
    
    [Header("Assign in Inspector")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private StringGameEvent sendCompletedTextEvent;
    
    private string currentText = "";
    private string previousText = "";
    private string lastTypedCharacter;
    private int lastTypedCharacterPosition = 0;
    
    private bool isProgramChangingText;
    
    private void Start()
    {
        if (remapType == REMAP_TYPE.REMAP_VOWELS)
        {
            for (int i = 0; i < numberOfRemaps; i++)
            {
                int rand1 = Random.Range(0, 5);
                int rand2 = rand1;

                while (rand2 == rand1)
                {
                    rand2 = Random.Range(0, 5);
                }
                
                SwapItems(newVowelOrder, rand1, rand2);  
            }
        }
    }

    public void OnLetterTyped()
    {
        currentText = inputField.text;
        
        // If the change is made by the program, don't do anything
        if (isProgramChangingText)
        {
            return;
        }
        
        if (inputField.text.Contains("\n"))
        {
            SendCompletedText();
            return;
        }

        GetLastTypedCharacter();
        RandomiseLetterChange();
        previousText = currentText;
    }

    private void GetLastTypedCharacter()
    {
        // Standard check for if a new character is added
        if (currentText.Length > previousText.Length)
        {
            for (int i = 0; i < currentText.Length; i++)
            {
                if (i >= previousText.Length || currentText[i] != previousText[i])
                {
                    lastTypedCharacter = currentText[i].ToString();
                    lastTypedCharacterPosition = i;
                    break;
                }
            }
        }
        // Edge case check for if words are highlighted and replaced with a single letter
        else if (currentText.Length < previousText.Length && previousText.Length - currentText.Length >= 2)
        {
            for (int i = 0; i < previousText.Length; i++)
            {
                if (i >= currentText.Length || currentText[i] != previousText[i])
                {
                    lastTypedCharacter = currentText[i].ToString();
                    lastTypedCharacterPosition = i;
                    Debug.Log(currentText[i].ToString());
                    break;
                }
            }
        }
        else
        {
            lastTypedCharacter = " ";
        }
    }
    
    private void SwapItems(string[] array, int index1, int index2)
    {
        // Check for out-of-bounds errors
        if (index1 < 0 || index1 >= array.Length || index2 < 0 || index2 >= array.Length)
        {
            throw new IndexOutOfRangeException("Index is outside the bounds of the array.");
        }
        
        // Swap the value
        (array[index1], array[index2]) = (array[index2], array[index1]);
    }

    private void RandomiseLetterChange()
    {
        // Don't bother if the string is less than 1 character long
        // if (currentText.Length <= 1)
        //     return;
        
        // Don't bother if the last typed character is a space
        if (lastTypedCharacter == " ")
            return;
        
        // Don't bother if the last typed character is a backspace
        // if (previousText.Length > currentText.Length)
        //     return;

        switch (remapType)
        {
            case REMAP_TYPE.REMAP_VOWELS:
                foreach (var vowel in vowels)
                {
                    if (vowel == lastTypedCharacter)
                    {
                        ReplaceLastTypedCharacter();
                        break;
                    }
                }
                break;
            
            case REMAP_TYPE.REMAP_ANY_LETTER:
                break;
            
            case REMAP_TYPE.REMAP_TO_CLOSEBY_LETTER:
                break;
            
            default:
                Debug.LogWarning("Non-implemented remap type");
                break;
        }
    }

    private void ReplaceLastTypedCharacter()
    {
        isProgramChangingText = true;
        
        string newText;
        newText = currentText.Substring(0, lastTypedCharacterPosition);
        newText += GetNewCharacter();
        
        // If you are editing the end of the textbox, don't worry about adding the previous text to the end
        if(lastTypedCharacterPosition + 1 < currentText.Length)
            newText += currentText.Substring(lastTypedCharacterPosition + 1, currentText.Length - newText.Length);
        
        inputField.text = newText;
        
        isProgramChangingText = false;
    }

    private string GetNewCharacter()
    {
        if (remapType == REMAP_TYPE.REMAP_VOWELS)
        {
            for (int i = 0; i < vowels.Length; i++)
            {
                if (vowels[i] == lastTypedCharacter)
                {
                    return newVowelOrder[i];
                }
            }
        }
        
        return "?";
    }
    
    private void SendCompletedText()
    {
        currentText = inputField.text;
        currentText = currentText.Replace("\n", "");
        
        sendCompletedTextEvent.SetString(currentText);
        sendCompletedTextEvent.Raise();
        
        Debug.Log("Sending string: " + currentText);
        
        previousText = "";
        currentText = "";
        inputField.text = "";
    }
}
