using CoffeeJitters.ResultsScreen.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeJitters.ResultsScreen.Tests
{
    public class Test_ResultScreen : MonoBehaviour
    {

        public ResultsScreenController controller;

        public int testInputScore;
        public string testInputRanking;

        private void Update()
        {
            controller.UpdateResultsScreen(new CoffeeReceipt()
            {
                Ranking = testInputRanking,
                TotalScore = testInputScore
            });
        }

    }

}