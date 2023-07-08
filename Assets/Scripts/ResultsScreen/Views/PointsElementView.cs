using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace CoffeeJitters.ResultsScreen.View
{

    public class PointsElementView : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private TMP_Text valueText;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        public void UpdateView(PointsElementViewModel viewModel)
            => valueText.text = viewModel.Value;

        #endregion Methods

    }

    public class PointsElementViewModel
    {

        #region - - - - - - Properties - - - - - -

        public string Value { get; set; }

        #endregion Properties

    }

}
