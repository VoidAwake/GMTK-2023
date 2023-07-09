using System;
using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    public BaristaObject currentBarista; 
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

    [SerializeField] private GameEvent baristaPatienceLost;

    [NonSerialized] public UnityEvent<string> textDisplayed = new();
    
    [Header("Barista Patience Parameters")]
    [SerializeField] private float patiencePerState = 20.0f;
    private float remainingPatience = 20.0f;

    [SerializeField] private AudioSource audioSource;
    
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

        remainingPatience = patiencePerState;

        //currentQuestionIndex = 0;
        
        audioSource.clip = currentBarista.music.Random();

        audioSource.loop = true;
        
        audioSource.Play();
    }

    private void Update()
    {
        remainingPatience -= Time.deltaTime;

        if (remainingPatience <= 0)
        {
            ProgressState();
        }
    }

    private void ProgressState()
    {
        switch (baristaState)
        {
            case baristaStates.nuetral: ChangeState(baristaStates.confused);
                break;
            case baristaStates.confused: ChangeState(baristaStates.angry);
                break;
            default:
                Debug.Log("GET A GAME OVER HERE");
                break;
        }
    }

    private void ChangeState(baristaStates newState)
    {
        baristaState = newState;
        OnStateChange();
    }

    private void OnStateChange()
    {
        remainingPatience = patiencePerState;
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
        gameDataStore = DaddyManager.instance.GameDataStore;
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

    public void DisplayNextOrderText()
    {
        SetText(gameDataStore.GetDialogueObjectByIdentifier(currentBarista.Identifier + "Next").questions.Random(), true);
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
            
            ProgressState();
            baristaPatienceLost.Raise();
            
            // Repeat the last question
            currentQuestionIndex--;
        }
    }
    
    public void SetText(string text, bool response)
    {
        textAnim.SetText(text);
        
        textDisplayed.Invoke(text);
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
