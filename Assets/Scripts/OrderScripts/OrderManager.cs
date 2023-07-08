using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: I think this is safe to delete
public class OrderManager : MonoBehaviour
{
    [Tooltip("Press this bool during runtime to generate a new order")]
    [SerializeField] private bool generateNewOrder = false;

    private Queue<CoffeeOrder> coffeeOrders = new Queue<CoffeeOrder>();
    
    private void Start()
    {
        QueueNewOrder();
    }
    
    private void Update()
    {
        if (generateNewOrder)
        {
            // TODO: Save remaining time to some kind of score
            
            coffeeOrders.Dequeue();
            QueueNewOrder();
            
            generateNewOrder = false;
        }
    }
    
    
    // Queue a new random order
    private void QueueNewOrder()
    {
        QueueNewOrder(
            EnumExtensions.GetRandom<COFFEE_TYPE>(),
            EnumExtensions.GetRandom<COFFEE_SIZE>(),
            EnumExtensions.GetRandom<MILK_TYPE>());
    }
    
    // Queue a set order
    private void QueueNewOrder(COFFEE_TYPE coffeeType, COFFEE_SIZE coffeeSize, MILK_TYPE milkType)
    {
        CoffeeOrder newOrder = new CoffeeOrder(coffeeType, coffeeSize, milkType);
        
        coffeeOrders.Enqueue(newOrder);
        
        Debug.Log("New order sent: " + newOrder.coffeeType + " of size " + newOrder.coffeeSize + " with milk type " + newOrder.milkType);
    }
}
