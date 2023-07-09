using System.Collections.Generic;
using DefaultNamespace;
using MoreLinq;
using TMPro;
using Hawaiian.Utilities;
using UnityEngine;

public class CoffeeManager : MonoBehaviour
{
    public enum ResponseMatch
    {
        No,
        Incorrect,
        Correct
    }
    
    [SerializeField] private TMP_Text objectiveText;
    
    private List<Coffee> coffeeOrders = new List<Coffee>();
    public int coffeeComplexity;


    public string expectedResponse;

    //public Coffee currentCoffee;
    private int numberOfCoffeesCompleted;

    // Difficulty settings
    // public int numberOfCoffees;
    
    [Tooltip("1 requires perfect score. 0 will accept anything.")]
    public float fuzzyMatchThreshold;

    public float objectiveTextDuration = 3;

    [SerializeField] private GameEvent incorrectItem;

    public void GenerateCoffee(int numberOfOrders)
    {
        for (int i = 0; i < numberOfOrders; i++)
        {
            coffeeOrders.Add(RandomCoffee());
        }
    }
    
    public Coffee GetCoffeeAtIndex(int index)
    {
        return coffeeOrders[index];
    }
    
    public Coffee GetCurrentCoffee()
    {
        if(coffeeOrders[0] != null)
            return coffeeOrders[0];
        else
        {
            Debug.LogWarning("No coffee orders remain");
            return null;
        }
    }
    
    public List<Coffee> GetAllOrders()
    {
        return coffeeOrders;
    }
    
    public void SetNextCoffee()
    {
        coffeeOrders.RemoveAt(0);
    }
    
    public ResponseMatch CheckResponse(string response, List<string> questionResponses)
    {
        response = response.ToUpper();
        
        var closestResponse = questionResponses.MaxBy(possibleResponse =>
        {
            long score = 0;

            possibleResponse = possibleResponse.ToUpper();

            FuzzySearch.FuzzyMatch(response, possibleResponse, ref score);

            return score;
        });

        closestResponse = closestResponse.ToUpper();
        
        long closestScore = 0;

        FuzzySearch.FuzzyMatch(response, closestResponse, ref closestScore);
        
        long perfectScore = 0;

        FuzzySearch.FuzzyMatch(response, closestResponse, ref perfectScore);

        var score = 0;
        var responseMatch = ResponseMatch.Correct;
        
        if (perfectScore * fuzzyMatchThreshold < closestScore)
        {
            if (closestResponse == expectedResponse)
            {
                score = 20;
                DaddyManager.instance.UpdateScore(20f);
                responseMatch = ResponseMatch.Correct;
            }
            else
            {
                score = -30;
                DaddyManager.instance.UpdateScore(-30f);
                responseMatch = ResponseMatch.Incorrect;
                
                incorrectItem.Raise();
            }
        }
        else
        {
            score = -15;
            responseMatch = ResponseMatch.No;
        }
        
        DaddyManager.instance.SetActualCoffeeProperty(closestResponse, closestResponse == expectedResponse, score);
        
        DaddyManager.instance.UpdateScore(score);

        return responseMatch;
    }


    private Coffee RandomCoffee()
    {
        var coffee = new Coffee();
        coffee.style = Coffee.styles.Random();
        coffee.milk = Coffee.milks.Random();
        coffee.size = Coffee.sizes.Random();
        if (coffeeComplexity > 1)
        {
            coffee.side = Coffee.sides.Random();
            coffee.topping = Coffee.toppings.Random();
            coffee.questionAmount = 5;
        }
        else if (coffeeComplexity > 0)
        {
            coffee.side = Coffee.sides.Random();
            coffee.questionAmount = 4;
        }
        
        return coffee;
    }

}