using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class InputRemapping : MonoBehaviour
{
    [Header("Remap Parameters")]
    [SerializeField]
    public REMAP_TYPE remapType;
    
    // Letter swap params
    // I use strings because it's nicer to store spaces
    private string[] vowels = {"A", "E", "I", "O", "U"};
    private List<int> vowelPositionList = new List<int> { 0, 4, 8, 14, 20 };
    private string[] alphabet = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
    private string[] newAlphabetOrder = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
    private bool letterSwapTriggered = false;
    
    // Double letter params
    [SerializeField] private bool doubleLettersEnabled = false;
    [SerializeField] private float doubleLettersChance = 0.05f;
    private bool doubleLetterTriggered = false;
    
    public int numberOfRemaps = 1;
    
    [Header("Assign in Inspector")]
    [SerializeField] public TMP_InputField inputField;
    [SerializeField] private StringGameEvent sendCompletedTextEvent;
    
    private string currentText = "";
    private string previousText = "";
    private string lastTypedCharacter;
    private int lastTypedCharacterPosition = 0;
    
    public bool isProgramChangingText = false;

    [NonSerialized] public UnityEvent normalLetterTyped = new();
    [NonSerialized] public UnityEvent swappedLetterTyped = new();
    [NonSerialized] public UnityEvent backspaceTyped = new();
    [NonSerialized] public UnityEvent doubleLetterTyped = new();
    // SO events
    [SerializeField] private GameEvent backspacedPressedEvent;

    private bool orderViewerActive = false;
    private bool isBaristaResponding = false;

    public void Initialise()
    {
        switch (remapType)
        {
            case REMAP_TYPE.REMAP_VOWELS:
                for (int i = 0; i < numberOfRemaps; i++)
                {
                    SwapVowels();
                }
                break;
            
            case REMAP_TYPE.REMAP_ANY_LETTER:
                // Always swap 1 set of vowels
                SwapVowels();
                
                // Now swap a number of times equal to the number of remaps (i=1 as we already did 1 swap)
                for (int i = 1; i < numberOfRemaps; i++)
                {
                    SwapAny();
                }
                break;
            
            case REMAP_TYPE.MULTISWAP:
                // Always swap 1 set of vowels AND get the position of one of the vowels
                int letterPos = SwapVowels();
                
                // Now swap a number of times equal to the number of remaps (i=1 as we already did 1 swap)
                for (int i = 1; i < numberOfRemaps; i++)
                {
                    letterPos = SwapWithInput(letterPos);
                }
                break;
            default:
                break;
        }
        
        EnableTyping();
    }

    // OPTIONAL RETURN: sends back the alphabet position of the first random vowel chosen
    private int SwapVowels()
    {
        int rand1 = Random.Range(0, 5);
        int rand2 = rand1;

        while (rand2 == rand1)
        {
            rand2 = Random.Range(0, 5);
        }
            
        Debug.Log("Swapping " + vowels[rand1] + " and " + vowels[rand2]);
        SwapItems(newAlphabetOrder, vowelPositionList[rand1], vowelPositionList[rand2]);

        return vowelPositionList[rand1];
    }
    
    // OPTIONAL RETURN: sends back the alphabet position of the first random letter chosen
    private int SwapAny()
    {
        int rand1 = Random.Range(0, 26);
        int rand2 = rand1;

        while (rand2 == rand1)
        {
            rand2 = Random.Range(0, 26);
        }
                
        Debug.Log("Swapping " + newAlphabetOrder[rand1] + " and " + newAlphabetOrder[rand2]);
        SwapItems(newAlphabetOrder, rand1, rand2);

        return rand1;
    }
    
    // OPTIONAL RETURN: sends back the alphabet position of the random letter chosen
    private int SwapWithInput(int positionToSwap)
    {
        int rand1 = positionToSwap;
        int rand2 = Random.Range(0, 26);
        
        while (rand2 == rand1)
        {
            rand2 = Random.Range(0, 26);
        }
        
        Debug.Log("Swapping " + newAlphabetOrder[rand1] + " and " + newAlphabetOrder[rand2]);
        SwapItems(newAlphabetOrder, rand1, rand2);

        return rand1;
    }

    public void SetOrderViewerActive(bool _orderViewerActive)
    {
        orderViewerActive = _orderViewerActive;
    }
    
    public void IsBaristaResponding(bool _isBaristaResponding)
    {
        isBaristaResponding = _isBaristaResponding;
    }
    
    public void EnableTyping()
    {
        // Do not enable typing if the order is in the player's face
        if (!orderViewerActive && !isBaristaResponding)
        {
            inputField.readOnly = false;
            inputField.Select();
            inputField.ActivateInputField();
        }
    }
    
    public void DisableTyping()
    {
        inputField.readOnly = true;
    }
    
    private void Update()
    {
        // Always focus
        if (!inputField.isFocused)
            inputField.Select();
        
        // Always move caret to end
        if (inputField.isFocused && inputField.caretPosition != inputField.text.Length)
        {
            inputField.caretPosition = inputField.text.Length;
        }
    }
    
    public void OnLetterTyped()
    {
        // Convert to uppercase AND RETURN TO NOT RUN THE REST
        // Updating the inputField to uppercase will run this function again anyway
        if (inputField.text != inputField.text.ToUpper())
        {
            inputField.text = inputField.text.ToUpper();
            return;
        }
        
        currentText = inputField.text;
        DaddyManager.instance.OnInput();
        
        // If the change is made by the program, don't do anything
        if (isProgramChangingText)
        {
            isProgramChangingText = false;
            return;
        }
        
        if (inputField.text.Contains("\n"))
        {
            SendCompletedText();
            return;
        }

        GetLastTypedCharacter();
        ChangeLetterIfApplicable();
        RandomiseDoubleLetterChance();

        PlaySoundEvent();
        
        previousText = currentText;
    }

    private void PlaySoundEvent()
    {
        if (doubleLetterTriggered)
        {
            doubleLetterTyped.Invoke();
            doubleLetterTriggered = false;
        }
        else if(letterSwapTriggered)
        {
            swappedLetterTyped.Invoke();
            letterSwapTriggered = false;
        }
        else if (currentText.Length <= previousText.Length)
        {
            backspaceTyped.Invoke();
            backspacedPressedEvent.Raise();
        }
        else if (currentText.Length > previousText.Length)
        {
            normalLetterTyped.Invoke();
        }
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
        else if (currentText.Length < previousText.Length && previousText.Length - currentText.Length >= 2 &&
                 currentText != "")
        {
            for (int i = 0; i < previousText.Length; i++)
            {
                if (i >= currentText.Length || currentText[i] != previousText[i])
                {
                    lastTypedCharacter = currentText[i].ToString();
                    lastTypedCharacterPosition = i;
                    //Debug.Log(currentText[i].ToString());
                    
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

    private void ChangeLetterIfApplicable()
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
        
        if(GetNewCharacter() != lastTypedCharacter)
            ReplaceLastTypedCharacter();
    }
    
    private void RandomiseDoubleLetterChance()
    {
        if (!doubleLettersEnabled)
            return;
        
        // Don't bother if the last typed character is a space
        if (lastTypedCharacter == " ")
            return;

        if (Random.Range(0.0f, 1.0f) < doubleLettersChance)
        {
            isProgramChangingText = true;
            
            string newText;
            
            // This is used later for the letter swapping rules
            // See comment [Now apply the letter swapping rules if applicable]
            previousText = currentText;
            
            // Modifying the middle of text
            if (lastTypedCharacterPosition + 1 < currentText.Length)
            {
                string unmodifiedText = currentText;
                
                newText = currentText.Substring(0, lastTypedCharacterPosition + 1);
                newText += lastTypedCharacter;
                
                // This will inheritly change currentText. This is why unmodifyText is introduced
                inputField.text = newText;
                inputField.MoveTextEnd(false);
                
                // We add a +1 to the end of text because the word is 1 letter longer
                inputField.text += unmodifiedText.Substring(lastTypedCharacterPosition + 1, unmodifiedText.Length - newText.Length + 1);
            }
            else // Modifying the end of text
            {
                newText = currentText + lastTypedCharacter;
                inputField.text = newText;
                inputField.MoveTextEnd(false);
            }
            
            // Now apply the letter swapping rules if applicable
            GetLastTypedCharacter();
            ChangeLetterIfApplicable();
            
            doubleLetterTriggered = true;
            
            //isProgramChangingText = false;
            
            //Debug.Log("Double letter");
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
        
        letterSwapTriggered = true;
        
        //isProgramChangingText = false;
    }

    private string GetNewCharacter()
    {
        for (int i = 0; i < alphabet.Length; i++)
        {
            if (alphabet[i] == lastTypedCharacter)
            {
                return newAlphabetOrder[i];
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

        isProgramChangingText = true;
        inputField.text = "";
    }
}
