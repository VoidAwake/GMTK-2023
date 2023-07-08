using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace DefaultNamespace
{
    public class ObjectiveLoop : MonoBehaviour
    {
        [SerializeField] private TMP_Text baristaText;
        
        private UnityEvent questionGenerated;
        private UnityEvent done;

        [NonSerialized] public string expectedResponse;

        private Coffee currentCoffee;
        private string currentQuestion;
        private int currentQuestionIndex;

        private List<string> questions = new()
        {
            "Style?",
            "Milk?",
            "Size?"
        };

        private void Start()
        {
            currentCoffee = RandomCoffee();

            var rng = new Random();

            rng.Shuffle(questions);
            
            StartQuestion();
        }

        public void StartQuestion()
        {
            StartCoroutine(Question());
        }

        private IEnumerator Question()
        {
            if (currentQuestionIndex == questions.Count)
            {
                done.Invoke();
                yield break;
            }

            currentQuestion = questions[currentQuestionIndex];

            currentQuestionIndex++;

            baristaText.text = currentQuestion;

            expectedResponse = GetExpectedResponse();

            yield return new WaitForSeconds(1);
            
            questionGenerated.Invoke();
        }

        private Coffee RandomCoffee()
        {
            var coffee = new Coffee();

            coffee.style = Coffee.styles.Random();
            coffee.milk = Coffee.milks.Random();
            coffee.size = Coffee.sizes.Random();

            return coffee;
        }
        
        private string GetExpectedResponse()
        {
            switch (currentQuestion)
            {
                case "Style?":
                    return currentCoffee.style;
                case "Milk?":
                    return currentCoffee.milk;
                case "Size?":
                    return currentCoffee.size;
            }

            return "";
        }
    }
    // From https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
    static class RandomExtensions
    {
        public static void Shuffle<T> (this Random rng, List<T> array)
        {
            int n = array.Count;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static T Random<T> (this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}