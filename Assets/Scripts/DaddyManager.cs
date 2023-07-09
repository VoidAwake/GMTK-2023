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
using UnityEngine.SceneManagement;

public class DaddyManager : MonoBehaviour, IInputValueTimeoutProvider
{
    private int levelsCompleted;
    public OrderUI orderUi;
    public float score = 0f;
    public float inputTimer = 0f;
    [SerializeField] private float timerBuffer;
    public Canvas canvas;
    public InputRemapping InputBox;
    public CoffeeManager coffeeManager;
    
    public List<List<ActualCoffeeProperty>> actualCoffees = new();

    [Header("Heart Rate Monitoring")]
    [SerializeField]
    public InputTimeoutData inputTimeoutData;
    public HeartRateMonitor heartRateMonitor;
    public HeartToECGModifier ecgModifier;
    GameObject ecgObject;

    [SerializeField] private GameDataStore _gameDataStore;
    public static DaddyManager instance;
    public Barista barista;
    [SerializeField] private DifficultyManager difficultyManager;

    private TMP_Text objectiveText;

    [Header("Assign in Inspector")]
    [SerializeField] private EndGameTransition endGameTransition;
    [SerializeField] private TimerScript timerScript;
    [SerializeField] public int numberOfOrders = 1;
    [SerializeField] private GameObject orderViewer;
    
    private OrderViewerHoverTrigger orderHoverTrigger;
    private int remainingOrders;

    private string coffeeOrderList = "";

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
    }

    //update

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

    public void DaddyStart(Canvas can, Barista bar, InputRemapping inputRemapping, CoffeeManager coffeeManager, GameDataStore gameDataStore, GameObject orderViewer, HeartRateMonitor monitor, HeartToECGModifier modifier, GameObject Object)
    {
        if (PlayerPrefs.HasKey("levelsCompleted"))
        {
            levelsCompleted = PlayerPrefs.GetInt("levelsCompleted");
        }
        else
        {
            levelsCompleted = 0;
            PlayerPrefs.SetInt("levelsCompleted", levelsCompleted);
        }
        
        heartRateMonitor = monitor;
        ecgModifier = modifier;
        ecgObject = Object;
        //call order generator
        //insantiate order ui
        canvas = can;
        barista = bar;
        InputBox = inputRemapping;
        this.orderViewer = orderViewer;
        this._gameDataStore = gameDataStore;
        this.coffeeManager = coffeeManager;
        OrderUI temp = Instantiate(orderUi,canvas.transform);
        
        this.heartRateMonitor.InitialiseHeartMonitor(this.ecgModifier, this, timerScript);
        InputBox.gameObject.SetActive(false);
        ecgObject.SetActive(false);
        //barista.gameObject.SetActive(false);
        
        difficultyManager.Initialise(coffeeManager, barista, InputBox, this);

        difficultyManager.AdjustDifficulty(levelsCompleted);

        remainingOrders = numberOfOrders;

        //objectiveLoop.baristaText = InputBox.GetComponentInChildren<TMP_Text>();
        
        NewActualCoffee();

        coffeeManager.GenerateCoffee(numberOfOrders);

        coffeeOrderList = "";
        for (int i = 0; i < coffeeManager.GetAllOrders().Count; i++)
        {
            coffeeOrderList += coffeeManager.GetCoffeeAtIndex(i).size
                               + " "
                               + coffeeManager.GetCoffeeAtIndex(i).milk
                               + " Milk "
                               + coffeeManager.GetCoffeeAtIndex(i).style;

            if (coffeeManager.GetCoffeeAtIndex(i).questionAmount == 5)
            {
                coffeeOrderList += " with " + coffeeManager.GetCoffeeAtIndex(i).topping + " on top"
                                            + " and a " + coffeeManager.GetCoffeeAtIndex(i).side
                                            + " on the side";

            }
            else if(coffeeManager.GetCoffeeAtIndex(i).questionAmount == 4)
            {
                coffeeOrderList += " with " + coffeeManager.GetCoffeeAtIndex(i).side + " on the side";
            }
            coffeeOrderList+= ". \n";

        }

        temp.OrderInit(coffeeOrderList);
    }

    public void GameStart()
    {
        timerScript.StartTimer(45f);
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
        // Disable typing and hover trigger
        InputBox.IsBaristaResponding(true);
        InputBox.DisableTyping();
        orderHoverTrigger.SetCollision(false);
        
        yield return new WaitForSeconds(2);
        
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
                barista.DisplayCloseText();

                Debug.Log("You have NOT reached the end");

                difficultyManager.AdjustDifficulty(numberOfOrders - remainingOrders);

                coffeeManager.SetNextCoffee();
                
                NewActualCoffee();

                barista.FirstQuestion(coffeeManager.GetAllOrders()[0].questionAmount);

                yield break;
            }
        }

        barista.DisplayCloseText();

        var textAnimationComplete = false;
        
        FindObjectOfType<TextAnim>().animationCompleted.AddListener(() => textAnimationComplete = true);

        yield return new WaitUntil(() => textAnimationComplete);
        
        FindObjectOfType<TextAnim>().animationCompleted.RemoveListener(() => textAnimationComplete = true);
        
        // Disable typing and hover trigger
        InputBox.IsBaristaResponding(true);
        InputBox.DisableTyping();
        orderHoverTrigger.SetCollision(false);
        
        // End transition before the next screen
        endGameTransition.gameObject.SetActive(true);
        endGameTransition.StartTransition(true);
        
        //yield return new WaitForSeconds(2);
        
        levelsCompleted++;
        PlayerPrefs.SetInt("levelsCompleted", levelsCompleted);
        Debug.Log("We have reached the end");
        //ResultsScreen();
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
    {
        actualCoffees.Last().Add(new ActualCoffeeProperty(barista.currentQuestion, correct, score, value));
    }

    public void NewActualCoffee()
    {
        actualCoffees.Add(new List<ActualCoffeeProperty>());
    }

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

    public void GameOver()
    {
        endGameTransition.gameObject.SetActive(true);
        endGameTransition.StartTransition(false);
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
