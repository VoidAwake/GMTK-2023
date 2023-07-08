using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DaddyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public OrderUI orderUi;
    public Canvas canvas;
    public InputRemapping InputBox;
    public ObjectiveLoop objectiveLoop;
    public IGameDataStore GameDataStore { get { return _gameDataStore; } }  
    [SerializeField] private GameDataStore _gameDataStore;
    public static DaddyManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    void Start()
    {
        //dont destroy on load
        DontDestroyOnLoad(this);
        //call order generator
        //insantiate order ui
        OrderUI temp = Instantiate(orderUi,canvas.transform);
        temp.OrderInit("test");
        
    }

    public void GameStart()
    {
        //SceneManager.LoadScene(1);
        Instantiate(InputBox,canvas.transform);
        objectiveLoop = Instantiate(objectiveLoop);
        //objectiveLoop.baristaText = InputBox.GetComponentInChildren<TMP_Text>();

    }
    
    

    
}
