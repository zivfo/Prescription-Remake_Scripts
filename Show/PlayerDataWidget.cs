using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using DG.Tweening;
using RefinedGame.Logic;

namespace RefinedGame.Show
{
    public class PlayerDataWidget : MonoBehaviour
    {
        public Player.PlayerDataType type;
        public Action<Player.PlayerDataType, int, int> onWidgetValueChanged;

        [SerializeField] TMP_Text txtDisplay = null;
        [SerializeField] bool skipAnime = false;

        Color originalColor;


        #region Unity Functions
        private void OnDestroy()
        {
            Player.onPlayerDataUpdated -= OnPlayerDataUpdated;
        }
        #endregion

        #region Public Functions
        public void Init(Action<Player.PlayerDataType, int, int> callBack)
        {
            Player.onPlayerDataUpdated += OnPlayerDataUpdated;
            onWidgetValueChanged += callBack;
            originalColor = txtDisplay.color;
        }
        #endregion

        #region Private Functions
        private void OnPlayerDataUpdated(Player.PlayerDataType type, int value, int amount)
        {
            if (type != this.type)
                return;
            else
            {
                if (skipAnime || !gameObject.activeSelf)
                {
                    txtDisplay.text = $"{value}";
                }
                else
                {
                    txtDisplay.DOKill();
                    txtDisplay.DOCounter(value - amount, value, 0.5f, type != Player.PlayerDataType.Credits);
                    var targetColor = amount > 0 ? Color.green : Color.red;
                    txtDisplay.DOColor(targetColor, 0.5f).OnComplete(() => txtDisplay.DOColor(originalColor, 0.5f));
                }
                if (onWidgetValueChanged != null)
                    onWidgetValueChanged(type, value, amount);
            }
        }
        #endregion
    }
}
