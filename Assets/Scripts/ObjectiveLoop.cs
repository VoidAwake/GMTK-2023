using System.Collections;
using System.Collections.Generic;
using MoreLinq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace DefaultNamespace
{
    public class ObjectiveLoop : MonoBehaviour
    {
        [SerializeField] private TMP_Text baristaText;
        
        private UnityEvent questionDone = new();
        private UnityEvent coffeeDone = new();
        private UnityEvent orderDone = new();

        public string expectedResponse;

        private Coffee currentCoffee;
        private string currentQuestion;
        private int currentQuestionIndex;
        private int numberOfCoffeesCompleted;

        private List<string> questions = new()
        {
            "Style?",
            "Milk?",
            "Size?"
        };

        // Difficulty settings
        // public int numberOfCoffees;
        public bool shuffleQuestionOrder; 

        private void Start()
        {
            // TODO: Will be called externally by the intro animation once complete
            // NextCoffee();
            NextQuestion();
        }

        public void NextCoffee()
        {
            // if (numberOfCoffeesCompleted == numberOfCoffees)
            // {
            //     orderDone.Invoke();
            //     return;
            // }
            
            currentCoffee = RandomCoffee();

            var rng = new Random();

            if (shuffleQuestionOrder)
                rng.Shuffle(questions);

            currentQuestionIndex = 0;
            
            NextQuestion();
        }

        [ContextMenu("NextQuestion")]
        public void NextQuestion()
        {
            StartCoroutine(Question());
        }

        private IEnumerator Question()
        {
            if (currentQuestionIndex == questions.Count)
            {
                coffeeDone.Invoke();
                // TODO: Not sure if this will be called externally
                NextCoffee();
                
                baristaText.text = "Anything else?";
                
                expectedResponse = GetExpectedResponse();
    
                yield return new WaitForSeconds(1);
                
                questionDone.Invoke();
                            
                yield break;
            }

            currentQuestion = questions[currentQuestionIndex];

            currentQuestionIndex++;

            baristaText.text = currentQuestion;

            expectedResponse = GetExpectedResponse();

            yield return new WaitForSeconds(1);
            
            questionDone.Invoke();
        }

        public void CheckResponse(string response)
        {
            var closestResponse = GetQuestionResponses().MaxBy(possibleResponse =>
            {
                long score = 0;

                FuzzySearch.FuzzyMatch(response, possibleResponse, ref score);

                return score;
            });

            if (closestResponse == expectedResponse)
            {
                // TODO: Good things happen
            }
            else
            {
                // TODO: Bad things happen
            }
            
            NextQuestion();
        }

        private Coffee RandomCoffee()
        {
            var coffee = new Coffee();

            coffee.style = Coffee.styles.Random();
            coffee.milk = Coffee.milks.Random();
            coffee.size = Coffee.sizes.Random();

            return coffee;
        }

        private List<string> GetQuestionResponses()
        {
            switch (currentQuestion)
            {
                case "style?":
                    return Coffee.styles;
                case "milk?":
                    return Coffee.milks;
                case "size?":
                    return Coffee.sizes;
            }

            return new List<string>();
        }
        
        private string GetExpectedResponse()
        {
            switch (currentQuestion)
            {
                case "style?":
                    return currentCoffee.style;
                case "milk?":
                    return currentCoffee.milk;
                case "size?":
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