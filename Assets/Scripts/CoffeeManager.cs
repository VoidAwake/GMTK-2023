using System.Collections;
using System.Collections.Generic;
using MoreLinq;
using TMPro;
using UnityEditor.Search;
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

    public string expectedResponse;

    //public Coffee currentCoffee;
    private int numberOfCoffeesCompleted;

    // Difficulty settings
    // public int numberOfCoffees;
    
    [Tooltip("1 requires perfect score. 0 will accept anything.")]
    public float fuzzyMatchThreshold;

    public float objectiveTextDuration = 3;

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
        var closestResponse = questionResponses.MaxBy(possibleResponse =>
        {
            long score = 0;

            FuzzySearch.FuzzyMatch(response, possibleResponse, ref score);

            return score;
        });
        
        long closestScore = 0;

        FuzzySearch.FuzzyMatch(response, closestResponse, ref closestScore);
        
        long perfectScore = 0;

        FuzzySearch.FuzzyMatch(response, closestResponse, ref perfectScore);

        if (perfectScore * fuzzyMatchThreshold < closestScore)
        {
            if (closestResponse == expectedResponse)
            {
                return ResponseMatch.Correct;
            }
            else
            {
                return ResponseMatch.Incorrect;
            }
        }
        else
        {
            return ResponseMatch.No;
        }
    }

    private Coffee RandomCoffee()
    {
        var coffee = new Coffee();

        coffee.style = Coffee.styles.Random();
        coffee.milk = Coffee.milks.Random();
        coffee.size = Coffee.sizes.Random();

        return coffee;
    }

}