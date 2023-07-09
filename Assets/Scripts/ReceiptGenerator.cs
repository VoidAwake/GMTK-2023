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
        private int mistakes;

        private void Awake()
        {
            var daddyManager = FindObjectOfType<DaddyManager>();

            if (daddyManager == null)
            {
                leftBodyTemp.text += "---------------\n";
                mainLeft.text = leftBodyTemp.text;
                // for (int i = 0; i < 100; i++)
                // {
                //     mainLeft.text += "test\n";
                //     leftBodyTemp.text += "test\n";
                // }

                return;
            }

            foreach (var actualCoffee in daddyManager.actualCoffees)
            {
                // leftBodyTemp.text += "\n";
                // rightBodyTemp.text += "\n";
                // mainLeft.text = leftBodyTemp.text;
                // mainRight.text = rightBodyTemp.text;
                

                foreach (var actualCoffeeProperty in actualCoffee)
                {
                    // leftBodyTemp.text += actualCoffeeProperty.propertyName + " : " + "\n" + actualCoffeeProperty.value + "\n";
                    // rightBodyTemp.text += actualCoffeeProperty.score + "\n" + "\n";
                    // mainLeft.text = leftBodyTemp.text;
                    // mainRight.text = rightBodyTemp.text;
                    //score += actualCoffeeProperty.score;
                    mistakes += actualCoffeeProperty.correct == false ? 1 : 0;
                }
            }

            leftBodyTemp.text += "Order Score: \n";
            rightBodyTemp.text += daddyManager.score + " \n";
            leftBodyTemp.text += "Total Score: \n";
            rightBodyTemp.text += daddyManager.highscore + " \n";
            leftBodyTemp.text += "---------------\n";
            leftBodyTemp.text += "We Miss You :D \n";
            mainLeft.text = leftBodyTemp.text;
            mainRight.text = rightBodyTemp.text;
            // TODO: Full line of dashes
            
            // leftBodyTemp.text += "SCORE: " + score;
            // mainLeft.text = leftBodyTemp.text;
        }
    }
}