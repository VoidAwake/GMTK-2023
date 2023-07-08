using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoffeeJitters.DataStore.Tests
{

    public class Test_GameDataStore : MonoBehaviour
    {

        [SerializeField]
        public GameDataStore gameDataStore;

        public string test1Identifier;
        public string test2Identifier;
        public string test3Identifier;

        // Tests on start
        void Start()
        {
            this.GetDialogueObjectByIdentifier_DataIsValid_GetsDialogueObject();
            this.GetDialogueResponsesByIdentifier_DataIsValid_GetsResponses();
            this.GetDialogueQuestionByIdentifier_DataIsValid_GetsQuestion();
        }

        void GetDialogueObjectByIdentifier_DataIsValid_GetsDialogueObject()
        {
            // Arrange

            // Act
            var _Result = ((IGameDataStore)this.gameDataStore).GetDialogueObjectByIdentifier(this.test1Identifier);

            // Assert
            Debug.Log($"Test Result 1: {_Result} Success: {_Result.question == "This is question 1"}");
        }

        void GetDialogueQuestionByIdentifier_DataIsValid_GetsQuestion()
        {
            // Arrange

            // Act
            var _Result = ((IGameDataStore)this.gameDataStore).GetDialogueQuestionByIdentifier(this.test2Identifier);

            // Assert
            Debug.Log($"Test Result 2: {_Result} Success: {_Result == "This is question 2"}");
        }

        void GetDialogueResponsesByIdentifier_DataIsValid_GetsResponses()
        {
            // Arrange

            // Act
            var _Result = ((IGameDataStore)this.gameDataStore).GetDialogueResponsesByIdentifier(this.test3Identifier);

            // Assert
            Debug.Log($"Test Result 2: {_Result} Success: {_Result.First() == "This is response 3"}");
        }

    }

}
