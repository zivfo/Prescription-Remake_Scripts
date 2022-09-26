using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{

    public class StudyWindow : AUiWindow
    {
        [SerializeField] Button backBtn = null;


        #region Unity Functions
        #endregion

        #region Public Functions
        public override void Init()
        {
            backBtn.onClick.AddListener(() => GameController.instance.runner.ToPreparation());
        }
        #endregion

        #region Private Functions
        #endregion
    }
}