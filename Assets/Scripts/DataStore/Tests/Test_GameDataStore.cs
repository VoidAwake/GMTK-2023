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

        [Header("Test Parameters")]
        [Space]
        public string inputIdentifier;
        public string expectedResponse;

        // Tests on start
        void Start()
            => this.GetDialogueObjectByIdentifier_DataIsValid_GetsDialogueObject();

        void GetDialogueObjectByIdentifier_DataIsValid_GetsDialogueObject()
        {
            // Arrange

            // Act
            var _Result = ((IGameDataStore)this.gameDataStore).GetDialogueObjectByIdentifier(this.inputIdentifier);

            // Assert
            Debug.Log($"Test Result 1: {_Result} => Success: {_Result.questions[0] == expectedResponse}");
        }


    }

}
