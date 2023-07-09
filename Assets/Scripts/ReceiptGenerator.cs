using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class ReceiptGenerator : MonoBehaviour
    {
        [SerializeField] private TMP_Text leftBody;
        [SerializeField] private TMP_Text rightBody;

        private int score;

        private void Awake()
        {
            var daddyManager = FindObjectOfType<DaddyManager>();

            foreach (var actualCoffee in daddyManager.actualCoffees)
            {
                leftBody.text += "\n";
                rightBody.text += "\n";
            
                foreach (var actualCoffeeProperty in actualCoffee)
                {
                    leftBody.text += actualCoffeeProperty.propertyName + " - " + actualCoffeeProperty.value + "\n";
                    rightBody.text += actualCoffeeProperty.score + "\n";

                    score += actualCoffeeProperty.score;
                }
            }

            leftBody.text += "\n";
            rightBody.text += "\n";
            
            // TODO: Full line of dashes
            
            leftBody.text += "SCORE: " + score;
        }
    }
}