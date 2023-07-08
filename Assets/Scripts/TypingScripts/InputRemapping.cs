using TMPro;
using UnityEngine;

public class InputRemapping : MonoBehaviour
{
    [Header("Remap Parameters")]
    [SerializeField] private REMAP_TYPE remapType;
    [SerializeField] private float remapPercentage = 0.05f;

    [Header("Assign in Inspector")]
    [SerializeField] private TMP_InputField inputField;
    
    public void OnLetterTyped()
    {
        if (remapType == REMAP_TYPE.REPLACE_ALL)
        {
            float rand = Random.Range(0.0f, 1.0f);
            
            if (rand < remapPercentage)
            {
                ReplaceLetter();
            }
        }
    }

    private void ReplaceLetter()
    {
        string originalText = inputField.text;
        
        // Cut the last letter and replace it
        inputField.text = originalText.Substring(0, originalText.Length - 1);
        inputField.text += "z";
        
        // Move the "Caret" to the end so the player keeps typing
        inputField.MoveTextEnd(false);
    }
}
