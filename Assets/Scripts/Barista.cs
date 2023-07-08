using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Barista : MonoBehaviour
{
    public TMP_Text baristaText;
    public bool shuffleQuestionOrder;

    public string currentQuestion;
    private int currentQuestionIndex;

    public List<string> questions = new()
    {
        "Style",
        "Milk",
        "Size",
    };

    private IGameDataStore gameDataStore;

    private void Start()
    {
        gameDataStore = DaddyManager.instance.GameDataStore;

        var rng = new Random();

        if (shuffleQuestionOrder)
            rng.Shuffle(questions);

        //currentQuestionIndex = 0;
    }
    
    public void FirstQuestion()
    {
        currentQuestionIndex = 0;
        currentQuestion = questions[currentQuestionIndex];
        baristaText.text = gameDataStore.GetDialogueObjectByIdentifier(currentQuestion).questions.Random();
        
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;
        if(currentQuestionIndex == questions.Count)
            return;
        currentQuestion = questions[currentQuestionIndex];

        baristaText.text = gameDataStore.GetDialogueObjectByIdentifier(currentQuestion).questions.Random();
    }

    public bool HasMoreQuestions()
    {
        return currentQuestionIndex != questions.Count;
    }

    public void DisplayCloseText()
    {
        baristaText.text = gameDataStore.GetDialogueObjectByIdentifier("Close").questions.Random();
    }

    public void DisplayResponseMatch(bool responseMatched)
    {
        if (responseMatched)
        {
            // TODO: Would be nice if the gameDataStore could take string arguments. Format?
            //baristaText.text = closestResponse + "? Sure.";
            baristaText.text = gameDataStore.GetDialogueObjectByIdentifier("Response Match").questions.Random();
        }
        else
        {
            baristaText.text = gameDataStore.GetDialogueObjectByIdentifier("No Response Match").questions.Random();
            
            // Repeat the last question
            currentQuestionIndex--;
        }
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
