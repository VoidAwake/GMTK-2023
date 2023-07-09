using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
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
        switch (completedOrders)
        {
            case 0:
                coffeeManager.fuzzyMatchThreshold = 0;
                barista.shuffleQuestionOrder = false;
                inputRemapping.remapType = REMAP_TYPE.REMAP_VOWELS;
                inputRemapping.numberOfRemaps = 0;
                daddyManager.numberOfOrders = 1;
                break;
            case 1:
                // TODO: Increase coffee complexity
                // For testing
                daddyManager.numberOfOrders = 2;
                break;
            case 2:
                daddyManager.numberOfOrders = 2;
                break;
            case 3:
                inputRemapping.numberOfRemaps = 0;
                break;
            case 4:
                break;
            case 5:
                // TODO: Enable once implemented
                inputRemapping.remapType = REMAP_TYPE.REMAP_ANY_LETTER;
                break;
        }
    }
}