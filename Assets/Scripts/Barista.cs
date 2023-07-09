using System;
using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Barista : MonoBehaviour
{
    public TMP_Text baristaText;
    public bool shuffleQuestionOrder;

    public string currentQuestion;
    private int currentQuestionIndex;
    private int questionCount = 3;

    [SerializeField] private Image baristaImage;
    [SerializeField] private List<BaristaObject> baristas;
    private BaristaObject currentBarista; 
    public TextAnim textAnim;
    private enum baristaStates
    {
        nuetral,
        confused,
        angry,
    };
    private baristaStates baristaState;

    [NonSerialized] public List<string> questions = new()
    {
        "Style",
        "Milk",
        "Size",
        "Side",
        "Topping"
    };

    private IGameDataStore gameDataStore;

    private void Start()
    {
        gameDataStore = DaddyManager.instance.GameDataStore;
        currentBarista = baristas.Random();
        ChangeState(baristaStates.nuetral);
        var rng = new Random();

        if (shuffleQuestionOrder)
            rng.Shuffle(questions);
        baristaImage.enabled = false;
        baristaText.enabled = false;

        //currentQuestionIndex = 0;
    }

    private void ChangeState(baristaStates newState)
    {
        baristaState = newState;
        OnStateChange();
    }

    private void OnStateChange()
    {
        switch (baristaState)
        {
            case baristaStates.nuetral: baristaImage.sprite = currentBarista.Nuetral;
                break;
            case baristaStates.confused: baristaImage.sprite = currentBarista.Confused;
                break;
            case baristaStates.angry: baristaImage.sprite = currentBarista.Angry;
                break;
        }
    }
    
    public void FirstQuestion(int questionAmount)
    {
        baristaImage.enabled = true;
        baristaText.enabled = true;
        currentQuestionIndex = 0;
        questionCount = questionAmount;
        currentQuestion = questions[currentQuestionIndex];
        SetText(gameDataStore.GetDialogueObjectByIdentifier(currentBarista.Identifier + currentQuestion).questions.Random(), false);
        DaddyManager.instance.InputBox.EnableTyping();
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;
        if(currentQuestionIndex >= questionCount)
            return;
        currentQuestion = questions[currentQuestionIndex];

        SetText(gameDataStore.GetDialogueObjectByIdentifier(currentBarista.Identifier + currentQuestion).questions.Random(), false);
        DaddyManager.instance.InputBox.EnableTyping();
    }

    public bool HasMoreQuestions()
    {
        return currentQuestionIndex != questionCount;
    }

    public void DisplayCloseText()
    {
        SetText(gameDataStore.GetDialogueObjectByIdentifier(currentBarista.Identifier + "Close").questions.Random(), true);
    }

    public void DisplayResponseMatch(bool responseMatched)
    {
        if (responseMatched)
        {
            // TODO: Would be nice if the gameDataStore could take string arguments. Format?
            //baristaText.text = closestResponse + "? Sure.";
            SetText(gameDataStore.GetDialogueObjectByIdentifier(currentBarista.Identifier + "Response Match").questions.Random(), true);
        }
        else
        {
            
            SetText(gameDataStore.GetDialogueObjectByIdentifier(currentBarista.Identifier + "No Response Match").questions.Random(), true);
            
            // Repeat the last question
            currentQuestionIndex--;
        }
    }
    
    public void SetText(string text, bool response)
    {
        textAnim.SetText(text, response);
    }
}

// From https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
static class RandomExtensions
{
    public static void Shuffle<T> (this Random rng, List<T> array)
    {
        int n = array.Count;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public static T Random<T> (this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    
}
