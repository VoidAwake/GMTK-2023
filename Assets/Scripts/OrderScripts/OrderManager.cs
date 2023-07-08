using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [Tooltip("Press this bool during runtime to generate a new order")]
    [SerializeField] private bool generateNewOrder = false;

    [Header("Assign in Inspector")]
    [SerializeField] private TimerScript timerScript;

    private Queue<Coffee> coffeeOrders = new Queue<Coffee>();
    
    private void Update()
    {
        if (generateNewOrder)
        {
            // TODO: Save remaining time to some kind of score
            
            coffeeOrders.Dequeue();
            QueueNewOrder(new Coffee());
            
            generateNewOrder = false;
        }
    }
    
    // Queue a set order
    private void QueueNewOrder(Coffee coffee)
    {
        timerScript.StartTimer(45f);
        
        coffeeOrders.Enqueue(coffee);
        
        Debug.Log("New order sent: " + coffee.style + " of size " + coffee.size + " with milk type " + coffee.milk);
    }
}
