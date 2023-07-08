using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaddyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public OrderUI orderUi;
    public Canvas canvas;

    //singleton
    //[SerializeField] private GameDataStore _gameDataStore;
    
    public static DaddyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    
    void Start()
    {
        //call order generator
        //insantiate order ui
        OrderUI temp = Instantiate(orderUi,canvas.transform);
        temp.OrderInit("test");
        
    }

    
}
