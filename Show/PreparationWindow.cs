using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{

    public class PreparationWindow : AUiWindow
    {
        [SerializeField] List<PlayerDataWidget> playerDataWidgets = null;
        [SerializeField] Button toWorkBtn = null;
        [SerializeField] Button toStudyBtn = null;
        [SerializeField] Button toSituationBtn = null;

        #region Unity Functions
        private void OnDestroy()
        {
            GameController.instance.runner.levelHandler.UnRegisterToPhaseUpdates(OnPhaseChanged);
        }
        #endregion

        #region Public Functions
        public override void Init()
        {
            foreach (var widget in playerDataWidgets)
            {
                widget.Init(OnWidgetValuedChanged);
            }
            toWorkBtn.onClick.AddListener(() => GameController.instance.runner.ToWork());
            toStudyBtn.onClick.AddListener(() => GameController.instance.runner.ToStudy());
            toSituationBtn.onClick.AddListener(() => GameController.instance.runner.ToSituation());
            toSituationBtn.interactable = false;
            toStudyBtn.interactable = false;

            GameController.instance.runner.levelHandler.RegisterToPhaseUpdates(OnPhaseChanged);
        }
        #endregion

        #region Private Functions
        private void OnPhaseChanged(LevelPhase obj)
        {
            if (obj.phaseLevel == 1 || obj.phaseLevel == 2)
            {
                var sequence = DOTween.Sequence();
                sequence.AppendInterval(2);
                sequence.AppendCallback(() =>
                {
                    //TODO: perhaps this should be related to event properly, not  hard coded;
                    if (obj.phaseLevel == 1)
                        toSituationBtn.interactable = true;

                    if (obj.phaseLevel == 2)
                        toStudyBtn.interactable = true;
                });
            }
        }
        private void OnWidgetValuedChanged(Player.PlayerDataType type, int value, int amount)
        {

            switch (type)
            {
                case Player.PlayerDataType.KnownDisease:

                    break;
                default: break;
            }
        }
        #endregion
    }
}
