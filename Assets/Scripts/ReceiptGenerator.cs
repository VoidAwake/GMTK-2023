using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class ReceiptGenerator : MonoBehaviour
    {
        [SerializeField] private TMP_Text leftBodyTemp;
        [SerializeField] private TMP_Text rightBodyTemp;
        [SerializeField] private TMP_Text mainLeft;
        [SerializeField] private TMP_Text mainRight;
        private int score;

        private void Awake()
        {
            var daddyManager = FindObjectOfType<DaddyManager>();

            foreach (var actualCoffee in daddyManager.actualCoffees)
            {
                leftBodyTemp.text += "\n";
                rightBodyTemp.text += "\n";
                mainLeft.text = leftBodyTemp.text;
                mainRight.text = rightBodyTemp.text;

                foreach (var actualCoffeeProperty in actualCoffee)
                {
                    leftBodyTemp.text += actualCoffeeProperty.propertyName + " : " + "\n" + actualCoffeeProperty.value + "\n";
                    rightBodyTemp.text += actualCoffeeProperty.score + "\n" + "\n";
                    mainLeft.text = leftBodyTemp.text;
                    mainRight.text = rightBodyTemp.text;
                    score += actualCoffeeProperty.score;
                }
            }
            
            foreach (var actualCoffee in daddyManager.actualCoffees)
            {
                leftBodyTemp.text += "\n";
                rightBodyTemp.text += "\n";
                mainLeft.text = leftBodyTemp.text;
                mainRight.text = rightBodyTemp.text;

                foreach (var actualCoffeeProperty in actualCoffee)
                {
                    leftBodyTemp.text += actualCoffeeProperty.propertyName + " : " + "\n" + actualCoffeeProperty.value + "\n";
                    rightBodyTemp.text += actualCoffeeProperty.score + "\n" + "\n";
                    mainLeft.text = leftBodyTemp.text;
                    mainRight.text = rightBodyTemp.text;
                    score += actualCoffeeProperty.score;
                }
            }

            leftBodyTemp.text += "\n";
            rightBodyTemp.text += "\n";
            mainLeft.text = leftBodyTemp.text;
            mainRight.text = rightBodyTemp.text;
            // TODO: Full line of dashes

            leftBodyTemp.text += "SCORE: " + score;
            mainLeft.text = leftBodyTemp.text;
        }
    }
}