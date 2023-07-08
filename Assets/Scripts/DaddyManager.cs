using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using TMPro;
using UnityEngine;

public class DaddyManager : MonoBehaviour
{
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

    private TMP_Text objectiveText;
    
    [Header("Assign in Inspector")]
    [SerializeField] private TimerScript timerScript;
    [SerializeField] private int numberOfOrders = 1;
    private int remainingOrders;

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
    }
    
    //update

    public void OnInput()
    {
        inputTimer = 0f;
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

        inputTimer += Time.deltaTime;
        if (inputTimer > timerBuffer)
        {
            //call heartbeat.increase(inputTimer)
        }
            
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
        
        string coffeeOrderList = "";
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
}
