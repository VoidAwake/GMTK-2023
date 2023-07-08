using System.Collections;
using System.Collections.Generic;
using CoffeeJitters.DataStore;
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
        public TMP_Text baristaText;
        [SerializeField] private TMP_Text objectiveText;

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
            "Style",
            "Milk",
            "Size"
        };

        // Difficulty settings
        // public int numberOfCoffees;
        public bool shuffleQuestionOrder;
        [Tooltip("1 requires perfect score. 0 will accept anything.")]
        public float fuzzyMatchThreshold;

        public float objectiveTextDuration = 3;

        private IGameDataStore gameDataStore;

        private void Start()
        {
            gameDataStore = DaddyManager.instance.GameDataStore;

            // TODO: Will be called externally by the intro animation once complete
            NextCoffee();
        }

        public void NextCoffee()
        {
            // if (numberOfCoffeesCompleted == numberOfCoffees)
            // {
            //     orderDone.Invoke();
            //     return;
            // }
            
            currentCoffee = RandomCoffee();
            
            StartCoroutine(ShowObjectiveText());

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
                // coffeeDone.Invoke();
                // TODO: Not sure if this will be called externally
                // NextCoffee();
                
                baristaText.text = gameDataStore.GetDialogueObjectByIdentifier("Close").questions.Random();
                
                // expectedResponse = GetExpectedResponse();
                //
                // yield return new WaitForSeconds(1);
                
                questionDone.Invoke();
                            
                yield break;
            }

            currentQuestion = questions[currentQuestionIndex];

            currentQuestionIndex++;

            baristaText.text = gameDataStore.GetDialogueObjectByIdentifier(currentQuestion).questions.Random();

            expectedResponse = GetExpectedResponse();

            yield return new WaitForSeconds(1);
            
            questionDone.Invoke();
        }

        public void CheckResponse(StringGameEvent stringGameEvent)
        {
            StartCoroutine(CheckResponseRoutine(stringGameEvent.GetString()));
        }

        private IEnumerator CheckResponseRoutine(string response)
        {
            foreach (var q in GetQuestionResponses())
            {
                Debug.Log(q);
            }
            
            var closestResponse = GetQuestionResponses().MaxBy(possibleResponse =>
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
                // TODO: Would be nice if the gameDataStore could take string arguments. Format?
                //baristaText.text = closestResponse + "? Sure.";
                baristaText.text = gameDataStore.GetDialogueObjectByIdentifier("Response Match").questions.Random();
            }
            else
            {
                baristaText.text = gameDataStore.GetDialogueObjectByIdentifier("No Response Match").questions.Random();
                
                // Repeat the last question
                currentQuestionIndex--;
            }
            
            yield return new WaitForSeconds(1);

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
                case "Style":
                    return Coffee.styles;
                case "Milk":
                    return Coffee.milks;
                case "Size":
                    return Coffee.sizes;
            }

            return new List<string>();
        }
        
        private string GetExpectedResponse()
        {
            switch (currentQuestion)
            {
                case "Style":
                    return currentCoffee.style;
                case "Milk":
                    return currentCoffee.milk;
                case "Size":
                    return currentCoffee.size;
            }

            return "";
        }

        private IEnumerator ShowObjectiveText()
        {
            objectiveText.enabled = true;

            objectiveText.text = currentCoffee.size + " " + currentCoffee.milk + " milk " + currentCoffee.style;
            
            yield return new WaitForSeconds(objectiveTextDuration);

            // TODO: Replace with an animation
            objectiveText.enabled = false;
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