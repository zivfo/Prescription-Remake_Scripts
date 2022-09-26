using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static RefinedGame.Logic.ClinicHandler;

namespace RefinedGame.Show
{
    public class ClinicStatusSignWidget : MonoBehaviour
    {
        [SerializeField] Image sign = null;
        [Header("Params")]
        [SerializeField] float colorChangeTime = 0.3f;

        ClinicStatus currentStatus = ClinicStatus.available;

        ClinicStatus CurrentStatus
        {
            get => currentStatus;
            set
            {
                currentStatus = value;
                SetStatusSign();
            }
        }

        #region Unity Functions
        private void OnDestroy()
        {
            GameController.instance.runner.clinicHandler.UnRegisterToPhaseUpdates(OnClinicStatusChanged);
        }
        #endregion

        #region Public Functions
        public void Init()
        {
            GameController.instance.runner.clinicHandler.RegisterToPhaseUpdates(OnClinicStatusChanged);
        }
        public void OnClinicStatusChanged(ClinicStatus status)
        {
            CurrentStatus = status;
        }
        #endregion

        #region Private Functions
        private void SetStatusSign()
        {
            sign.DOKill();
            Color color = Color.white;
            switch (currentStatus)
            {
                case ClinicStatus.busy: color = Color.red; break;
                case ClinicStatus.available: color = Color.green; break;
            }
            sign.DOColor(color, colorChangeTime);
        }
        #endregion
    }
}
