using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Data;
using RefinedGame.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class ClinicWindow : AUiWindow
    {
        [SerializeField] List<PlayerDataWidget> playerDataWidgets = null;
        [SerializeField] ClinicHallwayPanel clinicHallwayPanel = null;
        [SerializeField] ClinicStatusSignWidget statusSign = null;
        [SerializeField] ClinicCardPanel clinicCardPanel = null;
        [SerializeField] ClinicNotePanel clinicNotePanel = null;
        [SerializeField] ClinicLetterPanel clinicLetterPanel = null;
        [SerializeField] ClinicCounterPanel clinicCounterPanel = null;

        private Patient currentPatient;

        public Patient CurrentPatient { get => currentPatient; }


        #region Unity Functions
        private void OnDestroy()
        {
            GameController.instance.runner.eventHandler.UnRegisterToLetterDispatch(OnLetterCardsPopSignalReceived);
            GameController.instance.runner.patientHandler.UnRegisterToPatientInfected(OnPatientsInfected);
        }
        #endregion

        #region Public Functions
        public override void Init()
        {
            clinicLetterPanel.gameObject.SetActive(false);

            foreach (var widget in playerDataWidgets)
            {
                widget.Init(OnWidgetValuedChanged);
            }

            clinicHallwayPanel.Init(this);
            statusSign.Init();
            clinicCardPanel.Init(this);
            clinicNotePanel.Init(this);
            clinicLetterPanel.Init(this);
            clinicCounterPanel.Init(this);


            GameController.instance.runner.eventHandler.RegisterToLetterDispatch(OnLetterCardsPopSignalReceived);
            GameController.instance.runner.patientHandler.RegisterToPatientInfected(OnPatientsInfected);
        }

        public void PromoteLetterPanelLayer(bool reverse)
        {
            if (reverse)
            {
                clinicLetterPanel.transform.SetParent(transform);
                clinicLetterPanel.transform.SetAsLastSibling();
            }
            else
            {
                clinicLetterPanel.transform.SetParent(transform.parent);
                clinicLetterPanel.transform.SetSiblingIndex(transform.parent.childCount - 2);
            }
        }
        public void PopLetter()
        {
            clinicLetterPanel.PopLetter();
        }
        public void HandleLetterPanelOpenClose(RewardData rewardData)
        {
            GameController.instance.runner.clinicHandler.SetClinicStatus(ClinicHandler.ClinicStatus.available);

            GameController.instance.HandlePlayerRewards(rewardData);
            clinicCounterPanel.HideEnvelop();

            if (clinicCardPanel.CheckRemainingCardCount() == 0)
            {
                GameController.instance.runner.clinicHandler.ToNextStep();
            }
        }
        public void PrescriptionSubmitted()
        {
            clinicCounterPanel.PopPrescription();

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() => clinicNotePanel.PatientDataToPrescriptionNote());
        }
        public void PrescriptionDisposed()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() => clinicCounterPanel.HidePrescription());
            sequence.AppendCallback(() => clinicHallwayPanel.SendCurrentPatientAway());
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                currentPatient = null;
                GameController.instance.runner.clinicHandler.SetClinicStatus(ClinicHandler.ClinicStatus.available);

                if (clinicCardPanel.CheckRemainingCardCount() == 0)
                {
                    GameController.instance.runner.clinicHandler.ToNextStep();
                }
            });
        }
        #endregion

        #region Private Functions
        private void OnPatientsInfected(List<Patient> infectedPatients)
        {
            List<ClinicCardData> newCardsData = new List<ClinicCardData>();

            foreach (var aPatient in infectedPatients)
            {
                var newCardData = new ClinicCardData();

                newCardData.cardTitle = aPatient.data.theName;
                newCardData.cardType = ClinicCardType.PatientCard;
                newCardData.content = aPatient;
                newCardData.onClickCallBack = OnCardClicked;

                newCardsData.Add(newCardData);
            }

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(1);
            sequence.AppendCallback(() =>
            {
                clinicCardPanel.GeneratedCard(newCardsData);
                if (clinicCardPanel.CheckRemainingCardCount() == 0)
                {
                    GameController.instance.runner.clinicHandler.ToNextStep();
                };
            });
        }
        private void OnLetterCardsPopSignalReceived(List<EventLetterData> letters)
        {
            List<ClinicCardData> newCardsData = new List<ClinicCardData>();

            foreach (var aLetter in letters)
            {
                var newCardData = new ClinicCardData();
                switch (aLetter.effectType)
                {
                    case EventConfigData.EventEffectType.PatientDied: break;
                    default:
                        newCardData.cardTitle = aLetter.letterTitle;
                        newCardData.cardType = ClinicCardType.LetterCard;
                        newCardData.content = aLetter;
                        newCardData.onClickCallBack = OnCardClicked;
                        break;
                }
                newCardsData.Add(newCardData);
            }

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(2);
            sequence.AppendCallback(() =>
            {
                clinicCardPanel.GeneratedCard(newCardsData);
                if (clinicCardPanel.CheckRemainingCardCount() == 0)
                {
                    GameController.instance.runner.clinicHandler.ToNextStep();
                }
            });
        }
        private void OnCardClicked(ClinicCardData cardData)
        {
            GameController.instance.runner.clinicHandler.SetClinicStatus(ClinicHandler.ClinicStatus.busy);
            clinicCardPanel.PopAdditionalCardsIfAny();

            switch (cardData.cardType)
            {
                case ClinicCardType.LetterCard:
                    clinicLetterPanel.SetData((EventLetterData)cardData.content);
                    clinicCounterPanel.PopEnvelop();
                    break;
                case ClinicCardType.PatientCard:
                    //TODO: Call patient to clinic
                    currentPatient = (Patient)cardData.content;
                    clinicHallwayPanel.SendAPatient();
                    break;
            }
        }
        private void OnWidgetValuedChanged(Player.PlayerDataType type, int value, int amount)
        {
            switch (type)
            {
                case Player.PlayerDataType.UnknownDisease:
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
