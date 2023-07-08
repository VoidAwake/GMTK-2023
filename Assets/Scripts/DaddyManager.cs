using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using TMPro;
using UnityEngine;

public class DaddyManager : MonoBehaviour
{
    public OrderUI orderUi;
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
    
    
    void Start()
    {
        //dont destroy on load
        //call order generator
        //insantiate order ui
        OrderUI temp = Instantiate(orderUi,canvas.transform);
        
        coffeeManager = Instantiate(coffeeManager);
        //objectiveLoop.baristaText = InputBox.GetComponentInChildren<TMP_Text>();

        coffeeManager.GenerateCoffee();
        
        temp.OrderInit(coffeeManager.currentCoffee.size + " " + coffeeManager.currentCoffee.milk + " milk " + coffeeManager.currentCoffee.style);

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

        if (barista.HasMoreQuestions()) yield break;
        
        barista.DisplayCloseText();

        yield return new WaitForSeconds(2);

        // TODO: Next order
        Debug.Log("We have reached the end");
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
            "Style" => coffeeManager.currentCoffee.style,
            "Milk" => coffeeManager.currentCoffee.milk,
            "Size" => coffeeManager.currentCoffee.size,
            _ => ""
        };
    }
}
