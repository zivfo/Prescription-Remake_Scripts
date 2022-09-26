using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class SituationBarWidget : MonoBehaviour
    {
        [SerializeField] Slider frontBar = null;
        [SerializeField] Slider backBar = null;

        [SerializeField] TMP_Text playerProgressTxt = null;
        [SerializeField] TMP_Text playerSpeedTxt = null;
        [SerializeField] TMP_Text playerAccelerationTxt = null;
        [SerializeField] TMP_Text worldProgressTxt = null;
        [SerializeField] TMP_Text worldSpeedTxt = null;
        [SerializeField] TMP_Text worldAccelerationTxt = null;
        [SerializeField] TMP_Text movingAmountTxt = null;

        [SerializeField] Color incrementalColor = Color.black;
        [SerializeField] Color decreasingColor = Color.black;
        [SerializeField] Color originalTxtColor = Color.black;

        [SerializeField] float sliderMovingSpeedReciprocal = 0.005f;

        int sliderProgressValue;
        int sliderExpectedValue;
        int sliderMovingAmountValue;
        int playerSpeed;
        int worldSpeed;


        #region Unity Functions
        private void OnDestroy()
        {
            GameController.instance.runner.progressHandler.UnRegisterToProgressUpdates(OnProgressValueUpdated);
        }
        #endregion

        #region Public Functions
        public void Init()
        {
            GameController.instance.runner.progressHandler.RegisterToProgressUpdates(OnProgressValueUpdated);

            sliderMovingSpeedReciprocal *= 1000 / GameController.instance.runner.progressHandler.ProgressLength;
            backBar.maxValue = GameController.instance.runner.progressHandler.ProgressLength;
            frontBar.maxValue = GameController.instance.runner.progressHandler.ProgressLength;
        }
        #endregion

        #region Private Functions
        private void OnProgressValueUpdated(ProgressData.Type type, int value, int amount)
        {
            switch (type)
            {
                case ProgressData.Type.PlayerSpeed:
                    playerSpeed = value;
                    DoTxtAnime(playerSpeedTxt, value, amount);
                    break;
                case ProgressData.Type.PlayerAcceleration:
                    DoTxtAnime(playerAccelerationTxt, value, amount);
                    break;
                case ProgressData.Type.WorldSpeed:
                    worldSpeed = value;
                    DoTxtAnime(worldSpeedTxt, value, amount);
                    break;
                case ProgressData.Type.WorldAcceleration:
                    DoTxtAnime(worldAccelerationTxt, value, amount);
                    break;
                case ProgressData.Type.PlayerProgress:
                    DoTxtAnime(worldProgressTxt, GameController.instance.runner.progressHandler.ProgressLength - value,
                        value - (GameController.instance.runner.progressHandler.ProgressLength - sliderProgressValue));
                    sliderProgressValue = value;
                    DoTxtAnime(playerProgressTxt, value, amount);
                    break;
            }
            CalculateMovingAmount();
            DoSliderAnime(sliderExpectedValue, sliderProgressValue);
        }
        private void CalculateMovingAmount()
        {
            DoTxtAnime(movingAmountTxt, playerSpeed - worldSpeed, (playerSpeed - worldSpeed) - sliderMovingAmountValue);
            sliderMovingAmountValue = playerSpeed - worldSpeed;

            sliderExpectedValue = sliderProgressValue + playerSpeed - worldSpeed;
        }
        private void DoSliderAnime(int expectedProgress, int progress)
        {
            backBar.DOKill();
            frontBar.DOKill();

            if (expectedProgress >= progress)
            {
                backBar.fillRect.GetComponent<Image>().color = incrementalColor;

                frontBar.DOValue(progress, Mathf.Abs(frontBar.value - progress) * sliderMovingSpeedReciprocal);
                backBar.DOValue(expectedProgress, Mathf.Abs(backBar.value - expectedProgress) * sliderMovingSpeedReciprocal);
            }
            else
            {
                backBar.fillRect.GetComponent<Image>().color = decreasingColor;

                backBar.DOValue(progress, Mathf.Abs(backBar.value - progress) * sliderMovingSpeedReciprocal);
                frontBar.DOValue(expectedProgress, Mathf.Abs(frontBar.value - expectedProgress) * sliderMovingSpeedReciprocal);
            }
        }
        private void DoTxtAnime(TMP_Text txtDisplay, int value, int amount)
        {
            if (amount == 0)
                return;

            txtDisplay.DOKill();
            txtDisplay.DOCounter(value - amount, value, 0.5f, false);

            // var targetColor = amount > 0 ? Color.green : Color.red;
            // txtDisplay.DOColor(targetColor, 0.5f).OnComplete(() => txtDisplay.DOColor(originalTxtColor, 0.5f));
        }
        #endregion
    }
}
