using UnityEngine;

[CreateAssetMenu]
public class DifficultyLevel : ScriptableObject
{
    public float fuzzyMatchThreshold = 0;
    public int coffeeComplexity = 0;
    public bool shuffleQuestionOrder = false;
    public REMAP_TYPE remapType = REMAP_TYPE.REMAP_VOWELS;
    public int numberOfRemaps = 0;
    public int numberOfOrders = 1;
}