using CoffeeJitters.ResultsScreen.Entities;
using CoffeeJitters.ResultsScreen.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CoffeeJitters.ResultsScreen
{

    public class ResultsScreenController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private PointsElementView scoreView;
        [SerializeField]
        private PointsElementView rankingView;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        void Start()
        {
            
        }

        public void UpdateResultsScreen(CoffeeReceipt receipt)
        {
            this.scoreView.UpdateView(new PointsElementViewModel() { Value = receipt.TotalScore.ToString() });
            this.rankingView.UpdateView(new PointsElementViewModel() { Value = receipt.Ranking });
        }
        
        public void Continue(InputAction.CallbackContext context)
        {
            if (context.performed)
                SceneManager.LoadScene(1);
            //DaddyManager.instance.GameStart();
        }

        #endregion Methods

    }

}