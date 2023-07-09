using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using CoffeeJitters.HeartRateMonitor;
using CoffeeJitters.HeartRateMonitor.Services;
using TMPro;
using UnityEngine;

public class DaddyManager : MonoBehaviour, IInputValueTimeoutProvider
{
    public OrderUI orderUi;
    public float score = 0f;
    public float inputTimer = 0f;
    [SerializeField] private float timerBuffer;
    public Canvas canvas;
    public InputRemapping InputBox;
    public CoffeeManager coffeeManager;

    [Header("Heart Rate Monitoring")]
    [SerializeField]
    public InputTimeoutData inputTimeoutData;
    public HeartRateMonitor heartRateMonitor;
    public HeartToECGModifier ecgModifier;

    public IGameDataStore GameDataStore { get { return _gameDataStore; } }
    [SerializeField] private GameDataStore _gameDataStore;
    public static DaddyManager instance;
    public Barista barista;

    private TMP_Text objectiveText;

    [Header("Assign in Inspector")]
    [SerializeField] private TimerScript timerScript;
    [SerializeField] private int numberOfOrders = 1;
    [SerializeField] private GameObject orderViewer;
    private int remainingOrders;

    private string coffeeOrderList = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        this.heartRateMonitor.InitialiseHeartMonitor(this.ecgModifier, this, timerScript);
    }

    //update

    public void OnInput()
    {
        inputTimer = 0f;
        inputTimeoutData.currentInterpolatedValue = 0;
    }
    private void Update()
    {
        if (remainingOrders == 0)
        {
            //end game
            //display score
            //display end game text
            //display restart button
        }

        // Calculate input timeout
        inputTimer += Time.deltaTime;
        if (inputTimer > timerBuffer)
            this.TickInputTimeout();
    }

    void Start()
    {
        //call order generator
        //insantiate order ui
        OrderUI temp = Instantiate(orderUi,canvas.transform);

        coffeeManager = Instantiate(coffeeManager);

        remainingOrders = numberOfOrders;
        //objectiveLoop.baristaText = InputBox.GetComponentInChildren<TMP_Text>();

        coffeeManager.GenerateCoffee(numberOfOrders);

        coffeeOrderList = "";
        for (int i = 0; i < coffeeManager.GetAllOrders().Count; i++)
        {
            coffeeOrderList += coffeeManager.GetCoffeeAtIndex(i).size
                               + " "
                               + coffeeManager.GetCoffeeAtIndex(i).milk
                               + " milk "
                               + coffeeManager.GetCoffeeAtIndex(i).style
                               + "\n";
        }

        temp.OrderInit(coffeeOrderList);

        // TODO: Needs to be called for each new order
    }

    public void GameStart()
    {
        timerScript.StartTimer(45f);
        //SceneManager.LoadScene(1);
        Instantiate(InputBox,canvas.transform);

        if (orderViewer != null)
        {
            orderViewer.SetActive(true);
            orderViewer.GetComponentInChildren<OrderViewer>().Initialise(coffeeOrderList);
        }
        else
        {
            Debug.LogWarning("Order viewer is not assigned to DaddyManager. It will not appear in the scene");
        }

        barista.FirstQuestion();
    }

    public void OnTextSubmitted(StringGameEvent stringGameEvent)
    {
        coffeeManager.expectedResponse = GetExpectedResponse();

        var responseMatch = coffeeManager.CheckResponse(stringGameEvent.GetString(), GetQuestionResponses());

        barista.DisplayResponseMatch(responseMatch != CoffeeManager.ResponseMatch.No);

        StartCoroutine(NextQuestionRoutine());
    }

    private IEnumerator NextQuestionRoutine()
    {
        yield return new WaitForSeconds(2);

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

                coffeeManager.SetNextCoffee();

                barista.FirstQuestion();

                yield break;
            }
        }

        barista.DisplayCloseText();

        yield return new WaitForSeconds(2);

        Debug.Log("We have reached the end");
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
            _ => ""
        };
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
