using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DaddyManager : MonoBehaviour
{
    private int orderScore;
    public OrderUI orderUi;
    public float score = 0f;
    public float inputTimer = 0f;
    [SerializeField] private float timerBuffer; 
    public Canvas canvas;
    public InputRemapping InputBox;
    public CoffeeManager coffeeManager;

    public IGameDataStore GameDataStore { get { return _gameDataStore; } }  
    [SerializeField] private GameDataStore _gameDataStore;
    public static DaddyManager instance;
    public Barista barista;
    [SerializeField] private DifficultyManager difficultyManager;

    private TMP_Text objectiveText;
    
    [Header("Assign in Inspector")]
    [SerializeField] private TimerScript timerScript;
    [SerializeField] public int numberOfOrders = 1;
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

        if (PlayerPrefs.HasKey("orderScore"))
        {
            orderScore = PlayerPrefs.GetInt("orderScore");
        }
        else
        {
            orderScore = 0;
            PlayerPrefs.SetInt("orderScore", orderScore);
        }
    }
    
    //update

    public void OnInput()
    {
        inputTimer = 0f;
    }
    private void Update()
    {

        inputTimer += Time.deltaTime;
        if (inputTimer > timerBuffer)
        {
            //call heartbeat.increase(inputTimer)
        }
            
    }
    
    public void DaddyStart(Canvas can, Barista bar, InputRemapping inputRemapping, CoffeeManager coffeeManager)
    {
        //call order generator
        //insantiate order ui
        canvas = can;
        barista = bar;
        InputBox = inputRemapping;
        this.coffeeManager = coffeeManager;
        OrderUI temp = Instantiate(orderUi,canvas.transform);
        
        InputBox.gameObject.SetActive(false);
        
        difficultyManager.Initialise(coffeeManager, barista, InputBox, this);
        
        remainingOrders = numberOfOrders;
        
        difficultyManager.AdjustDifficulty(numberOfOrders - remainingOrders);

        InputBox.Initialise();

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

        if (orderViewer != null)
        {
            orderViewer.SetActive(true);
            orderViewer.GetComponentInChildren<OrderViewer>().Initialise(coffeeOrderList);
        }
        else
        {
            Debug.LogWarning("Order viewer is not assigned to DaddyManager. It will not appear in the scene");
        }
        
        InputBox.gameObject.SetActive(true);
        
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
            orderScore++;
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
                
                barista.FirstQuestion();
                
                yield break;
            }
        }
        
        barista.DisplayCloseText();
        
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("orderScore", orderScore);
        Debug.Log("We have reached the end");
        ResultsScreen();
    }

    private void ResultsScreen()
    {
        SceneManager.LoadScene(2);
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
    
}
