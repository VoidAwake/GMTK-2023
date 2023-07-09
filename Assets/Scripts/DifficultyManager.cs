using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private List<DifficultyLevel> difficultyLevels;
    [Tooltip("How many difficulty levels to loop from the end of the list.")]
    [SerializeField] private int loopLast = 1;

    [SerializeField] [Hawaiian.Utilities.ReadOnly] private int currentDifficultyLevelIndex;
    
    private CoffeeManager coffeeManager;
    private Barista barista;
    private InputRemapping inputRemapping;
    private DaddyManager daddyManager;

    public void Initialise(CoffeeManager coffeeManager, Barista barista, InputRemapping inputRemapping, DaddyManager daddyManager)
    {
        this.coffeeManager = coffeeManager;
        this.barista = barista;
        this.inputRemapping = inputRemapping;
        this.daddyManager = daddyManager;
    }

    public void AdjustDifficulty(int completedOrders)
    {
        if (completedOrders < difficultyLevels.Count)
            currentDifficultyLevelIndex = completedOrders;
        else
            currentDifficultyLevelIndex = completedOrders - (difficultyLevels.Count - loopLast) % loopLast +
                                   (difficultyLevels.Count - loopLast);

        var difficultyLevel = difficultyLevels[currentDifficultyLevelIndex];
        
        coffeeManager.fuzzyMatchThreshold = difficultyLevel.fuzzyMatchThreshold;
        coffeeManager.coffeeComplexity = difficultyLevel.coffeeComplexity;
        barista.shuffleQuestionOrder = difficultyLevel.shuffleQuestionOrder;
        inputRemapping.remapType = difficultyLevel.remapType;
        inputRemapping.numberOfRemaps = difficultyLevel.numberOfRemaps;
        daddyManager.numberOfOrders = difficultyLevel.numberOfOrders;
    }
}