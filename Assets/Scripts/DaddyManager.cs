using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoffeeJitters.DataStore;
using CoffeeJitters.HeartRateMonitor;
using CoffeeJitters.HeartRateMonitor.Services;
using DefaultNamespace;
using CoffeeJitters.Timer.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DaddyManager : MonoBehaviour, IInputValueTimeoutProvider
{

    #region - - - - - - Fields - - - - - -

    public static DaddyManager instance;

    [Header("Level Modifiers")]
    public int levelsCompleted;
    public float inputTimer = 0f;
    public float score = 0f;
    public float highscore;
    [SerializeField] private float timerBuffer;

    [Header("User Interfaces")]
    public OrderUI orderUi;
    public Canvas canvas;
    public InputRemapping InputBox;
    public CoffeeManager coffeeManager;

    [Header("Heart Rate Monitoring")]
    [SerializeField] private InputTimeoutData inputTimeoutData;
    public HeartRateMonitor heartRateMonitor;
    [SerializeField] private HeartToECGModifier ecgModifier;
    GameObject ecgObject;

    [Header("Data Context")]
    [SerializeField] private GameDataStore _gameDataStore;

    [Header("Assign in Inspector")]
    [SerializeField] private EndGameTransition endGameTransition;
    [SerializeField] private TimerScript timerScript;
    [SerializeField] public int numberOfOrders = 1;
    [SerializeField] private GameObject orderViewer;
    public Barista barista;
    [SerializeField] private DifficultyManager difficultyManager;
    public GameObject kill;

    // Internal fields
    public List<List<ActualCoffeeProperty>> actualCoffees = new();
    private OrderViewerHoverTrigger orderHoverTrigger;
    private int remainingOrders;
    private string coffeeOrderList = "";
    private GAME_OVER_TYPE gameOverType = GAME_OVER_TYPE.NONE;

    // Unity Events
    [NonSerialized] public UnityEvent DaddyStarted = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public IGameDataStore GameDataStore { get { return _gameDataStore; } }

    public IPatienceTimerProvider PatienceTimerProvider { get { return this.timerScript; } }

    #endregion Properties

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            PlayerPrefs.SetInt("levelsCompleted", 0);
        }
        else
        {
            Destroy(gameObject);
        }

        highscore = 0; // Might not be necessary
    }

    public void OnInput()
    {
        inputTimer = 0f;
        inputTimeoutData.currentInterpolatedValue = 0;
    }
    private void Update()
    {
        // Calculate input timeout
        inputTimer += Time.deltaTime;
        if (inputTimer > timerBuffer)
            this.TickInputTimeout();
    }

    /// <summary>
    /// Responsible for initialising the game manager behaviour from the scale of the scene scope.
    /// </summary>
    public void DaddyStart(
        Canvas canvas,
        Barista barista,
        InputRemapping inputRemapping,
        CoffeeManager coffeeManager,
        GameDataStore gameDataStore,
        GameObject orderViewer,
        HeartRateMonitor monitor,
        HeartToECGModifier modifier,
        GameObject Object,
        GameObject continueObject)
    {
        // Check whether the level has been completed
        if (PlayerPrefs.HasKey("levelsCompleted"))
        {
            levelsCompleted = PlayerPrefs.GetInt("levelsCompleted");
        }
        else
        {
            levelsCompleted = 0;
            PlayerPrefs.SetInt("levelsCompleted", levelsCompleted);
        }

        this.kill = continueObject;
        this.actualCoffees.Clear();
        this.score = 0;

        // Initialise related heart rate fields
        this.heartRateMonitor = monitor;
        this.ecgModifier = modifier;
        this.ecgObject = Object;
        this.ecgObject.SetActive(false);

        // Initialise user interfaces
        this.canvas = canvas;
        this.barista = barista;
        this.InputBox = inputRemapping;
        this.InputBox.gameObject.SetActive(false);
        this.orderViewer = orderViewer;
        this.coffeeManager = coffeeManager;

        // Initialise data store
        this._gameDataStore = gameDataStore;

        // Initialise heart rate monitor
        this.heartRateMonitor.InitialiseHeartMonitor(this.ecgModifier, this, timerScript);

        // Initialise difficulty manager
        this.difficultyManager.Initialise(coffeeManager, barista, InputBox, this);
        this.difficultyManager.AdjustDifficulty(levelsCompleted);

        this.remainingOrders = numberOfOrders;

        // Creates a new coffee property
        this.NewActualCoffee();
        coffeeManager.GenerateCoffee(numberOfOrders);

        // Create a new coffee list
        coffeeOrderList = "";
        for (int i = 0; i < coffeeManager.GetAllOrders().Count; i++)
        {
            coffeeOrderList += coffeeManager.GetCoffeeAtIndex(i).size
                               + " "
                               + coffeeManager.GetCoffeeAtIndex(i).milk
                               + " Milk "
                               + coffeeManager.GetCoffeeAtIndex(i).style;

            if (coffeeManager.GetCoffeeAtIndex(i).questionAmount == 5)
                coffeeOrderList += " with " + coffeeManager.GetCoffeeAtIndex(i).topping + " on top"
                                            + " and a " + coffeeManager.GetCoffeeAtIndex(i).side
                                            + " on the side";

            else if(coffeeManager.GetCoffeeAtIndex(i).questionAmount == 4)
                coffeeOrderList += " with " + coffeeManager.GetCoffeeAtIndex(i).side + " on the side";

            coffeeOrderList+= ". \n";

        }

        // Create a new coffee order interface
        Instantiate(orderUi, this.canvas.transform).OrderInit(coffeeOrderList);

        DaddyStarted.Invoke();
    }

    // TODO: Rename this method to something better
    public void GameStart()
    {
        Destroy(kill);
        timerScript.StartTimer(45f);
        heartRateMonitor.StartIncreaseHeartRate();
        //SceneManager.LoadScene(1);

        if (orderViewer != null)
        {
            orderViewer.SetActive(true);
            orderViewer.GetComponentInChildren<OrderViewer>().Initialise(coffeeOrderList);
            orderHoverTrigger = orderViewer.GetComponentInChildren<OrderViewerHoverTrigger>();
        }
        else
        {
            Debug.LogWarning("Order viewer is not assigned to DaddyManager. It will not appear in the scene");
        }

        gameOverType = GAME_OVER_TYPE.NONE;

        InputBox.gameObject.SetActive(true);
        //barista.gameObject.SetActive(true);
        ecgObject.SetActive(true);
        InputBox.Initialise();

        barista.FirstQuestion(coffeeManager.GetAllOrders()[0].questionAmount);
    }

    public void OnTextSubmitted(StringGameEvent stringGameEvent)
    {
        barista.textAnim.onTextEnd();
        coffeeManager.expectedResponse = GetExpectedResponse();

        var responseMatch = coffeeManager.CheckResponse(stringGameEvent.GetString(), GetQuestionResponses());

        barista.DisplayResponseMatch(responseMatch != CoffeeManager.ResponseMatch.No);
        StartCoroutine(NextQuestionRoutine());
    }

    private IEnumerator NextQuestionRoutine()
    {
        var textAnimationComplete = false;

        barista.textAnim.animationCompleted.AddListener(() => textAnimationComplete = true);
        // Disable typing and hover trigger
        InputBox.IsBaristaResponding(true);
        InputBox.DisableTyping();
        orderHoverTrigger.SetCollision(false);
        yield return new WaitUntil(() => textAnimationComplete);
        textAnimationComplete = false;
        yield return new WaitForSeconds(0.5f);

        // Don't continue if it's a game over state
        if (gameOverType != GAME_OVER_TYPE.NONE)
        {
            yield break;
        }

        // Enable typing and hover trigger
        InputBox.IsBaristaResponding(false);
        InputBox.EnableTyping();
        orderHoverTrigger.SetCollision(true);

        barista.NextQuestion();

        if (barista.HasMoreQuestions())
        {
            yield break;
        }
        else if(remainingOrders > 0)
        {
            remainingOrders--;

            if (remainingOrders > 0)
            {
                // TODO: Barista to ask about the next order first

                barista.DisplayNextOrderText();
                yield return new WaitUntil(() => textAnimationComplete);
                textAnimationComplete = false;
                yield return new WaitForSeconds(0.5f);

                Debug.Log("You have NOT reached the end");

                difficultyManager.AdjustDifficulty(numberOfOrders - remainingOrders);

                coffeeManager.SetNextCoffee();

                NewActualCoffee();

                barista.FirstQuestion(coffeeManager.GetAllOrders()[0].questionAmount);

                yield break;
            }
        }

        barista.DisplayCloseText();



        yield return new WaitUntil(() => textAnimationComplete);

        barista.textAnim.animationCompleted.RemoveListener(() => textAnimationComplete = true);

        // Disable typing and hover trigger
        InputBox.IsBaristaResponding(true);
        InputBox.DisableTyping();
        orderHoverTrigger.SetCollision(false);

        // End transition before the next screen
        endGameTransition.gameObject.SetActive(true);
        endGameTransition.StartTransition(true);

        //yield return new WaitForSeconds(2);

        levelsCompleted++;
        highscore += score;
        PlayerPrefs.SetFloat("highscore", highscore);
        PlayerPrefs.SetInt("levelsCompleted", levelsCompleted);
        Debug.Log("We have reached the end");
        //ResultsScreen();
    }


    public void TweenUiIn()
    {

    }
    private void ResultsScreen()
    {
        //SceneManager.LoadScene(2);
    }

    public void UpdateScore(float amount)
    {
        this.score += amount;
        Debug.Log(score);
    }

    private List<string> GetQuestionResponses()
    {
        return barista.currentQuestion switch
        {
            "Style" => Coffee.styles,
            "Milk" => Coffee.milks,
            "Size" => Coffee.sizes,
            "Topping" => Coffee.toppings,
            "Side" => Coffee.sides,
            _ => new List<string>()
        };
    }

    private string GetExpectedResponse()
    {
        return barista.currentQuestion switch
        {
            "Style" => coffeeManager.GetCurrentCoffee().style,
            "Milk" => coffeeManager.GetCurrentCoffee().milk,
            "Size" => coffeeManager.GetCurrentCoffee().size,
            "Topping" => coffeeManager.GetCurrentCoffee().topping,
            "Side" => coffeeManager.GetCurrentCoffee().side,
            _ => ""
        };
    }

    public void SetActualCoffeeProperty(string value, bool correct, int score)
        => actualCoffees.Last().Add(new ActualCoffeeProperty(barista.currentQuestion, correct, score, value));

    public void NewActualCoffee()
        => actualCoffees.Add(new List<ActualCoffeeProperty>());

    private void TickInputTimeout()
    {
        inputTimeoutData.currentInterpolatedValue = Mathf.Clamp((inputTimer - timerBuffer) / inputTimeoutData.maxTimeoutTime, 0f, 1f);
        if (inputTimer > timerBuffer + inputTimeoutData.maxTimeoutTime)
            inputTimeoutData.currentInterpolatedValue = 1;
    }

    /// <summary>
    /// Provides the gradually increasing timeout value and interpolated between 0 and 1.
    /// </summary>
    /// <returns>Interpolated value between 0 and 1.</returns>
    float IInputValueTimeoutProvider.GetInputTimeoutValue()
        => this.inputTimeoutData.currentInterpolatedValue;

    public void GameOver(GAME_OVER_TYPE _gameOverType)
    {
        endGameTransition.gameObject.SetActive(true);

        gameOverType = _gameOverType;

        endGameTransition.StartTransition(false);
    }

    public GAME_OVER_TYPE GetGameOverType()
    {
        return gameOverType;
    }
}

[System.Serializable]
public struct InputTimeoutData
{

    #region - - - - - - Fields - - - - - -

    /// <summary>
    /// Value between 0 and 1 representing the timer past the timer buffer.
    /// </summary>
    public float currentInterpolatedValue;

    /// <summary>
    /// The maximum amount of time after the timer buffer.
    /// </summary>
    public float maxTimeoutTime;

    #endregion Fields

}
