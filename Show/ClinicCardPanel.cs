using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Data;
using RefinedGame.Logic;
using RefinedGame.Tool.DebugUtil;
using UnityEngine;

namespace RefinedGame.Show
{
    public class ClinicCardPanel : MonoBehaviour
    {
        [SerializeField] List<RectTransform> anchorPoints = new List<RectTransform>();
        [SerializeField] RectTransform cardParent = null;
        [SerializeField] ClinicCardTemplate cardTemplate = null;

        [Header("Params")]
        [SerializeField] Ease popStyle = Ease.OutBack;
        [SerializeField] float popInterval = 0.03f;

        private List<ClinicCardTemplate> currentCards = new List<ClinicCardTemplate>();
        private ClinicWindow owner;


        #region Unity Functions
        private void OnDestroy()
        {
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Clinic_GenerateCards, Debug_GenerateCards);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Clinic_PopCards, Debug_PopCards);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Clinic_HideCards, Debug_HideCards);
            GameController.instance.runner.clinicHandler.UnRegisterToPhaseUpdates(OnClinicStatusChanged);

        }
        #endregion

        #region Public Functions
        public void Init(ClinicWindow clinicWindow)
        {
            this.owner = clinicWindow;
            cardTemplate.gameObject.SetActive(false);

            DebugEventsController.GetInstance().Subscribe(CheatType.Clinic_GenerateCards, Debug_GenerateCards);
            DebugEventsController.GetInstance().Subscribe(CheatType.Clinic_PopCards, Debug_PopCards);
            DebugEventsController.GetInstance().Subscribe(CheatType.Clinic_HideCards, Debug_HideCards);
            GameController.instance.runner.clinicHandler.RegisterToPhaseUpdates(OnClinicStatusChanged);

        }
        public void GeneratedCard(List<ClinicCardData> cardsData)
        {
            ClearAllCards();

            for (int i = 0; i < cardsData.Count; i++)
            {
                ClinicCardTemplate newCard = Instantiate(cardTemplate, cardParent, false);
                cardsData[i].hidePos = anchorPoints[i].anchoredPosition;
                newCard.Init(cardsData[i]);
                newCard.id = System.Guid.NewGuid();
                if (i >= 8)
                    newCard.pairSlotId = currentCards[i - 8].id;

                newCard.gameObject.SetActive(true);
                currentCards.Add(newCard);
            }

            PopUpCards();
        }
        public void PopAdditionalCardsIfAny()
        {
            foreach (var card in currentCards)
            {
                if (card.pairSlotId != System.Guid.Empty && FindCardById(card.pairSlotId).hasInteracted)
                    card.Pop(popStyle);
            }
        }
        public int CheckRemainingCardCount()
        {
            int count = 0;
            foreach (var card in currentCards)
            {
                if (card.isHidden == false)
                    count++;
            }
            return count;
        }
        #endregion

        #region Private Functions
        private ClinicCardTemplate FindCardById(System.Guid id)
        {
            return currentCards.Find(x => x.id == id);
        }
        private void BlockInteraction(bool setBlock)
        {
            foreach (var card in currentCards)
            {
                card.Interactable = !setBlock;
            }
        }
        private void OnClinicStatusChanged(ClinicHandler.ClinicStatus status)
        {
            switch (status)
            {
                case ClinicHandler.ClinicStatus.available: BlockInteraction(false); break;
                case ClinicHandler.ClinicStatus.busy: BlockInteraction(true); break;
            }
        }
        private void ClearAllCards()
        {
            foreach (var card in currentCards)
            {
                Destroy(card.gameObject);
            }
            currentCards.Clear();
        }
        private void PopUpCards(bool forceInstanct = false)
        {
            var sequnce = DOTween.Sequence();
            foreach (var card in currentCards)
            {
                sequnce.AppendCallback(() =>
                {
                    if (card.pairSlotId == System.Guid.Empty)
                        card.Pop(popStyle);
                });
                sequnce.AppendInterval(popInterval);
            }
        }
        private void HideCards(bool forceInstanct = false)
        {
            foreach (var card in currentCards)
            {
                card.Hide();
            }
        }

        #endregion

        #region Debug Functions
        private void Debug_OnClinicCardClicked(ClinicCardData cardData)
        {
            switch (cardData.cardType)
            {
                case ClinicCardType.LetterCard:
                    //Pop Letter
                    Debug_Log("Letter");
                    break;
                case ClinicCardType.PatientCard:
                    //Call Patient
                    Debug_Log("Patient");
                    break;

                default: break;
            }
        }
        private void Debug_GenerateCards(ACheatData data)
        {
            var cardDataList = new List<ClinicCardData>();
            bool letterOrPatient = Random.Range(0, 2) == 0 ? true : false;

            //TODO: 9
            int count = Random.Range(0, 9);
            switch (letterOrPatient)
            {
                case true:
                    //letter cards
                    for (int i = 0; i < count; i++)
                    {
                        var newCardData = new ClinicCardData();
                        newCardData.onClickCallBack = Debug_OnClinicCardClicked;
                        newCardData.cardType = ClinicCardType.LetterCard;
                        newCardData.cardTitle = "Letter";
                        newCardData.hidePos = anchorPoints[i].anchoredPosition;

                        cardDataList.Add(newCardData);
                    }

                    break;
                case false:
                    //patient cards
                    for (int i = 0; i < count; i++)
                    {
                        var newCardData = new ClinicCardData();
                        newCardData.onClickCallBack = Debug_OnClinicCardClicked;
                        newCardData.cardType = ClinicCardType.PatientCard;
                        newCardData.cardTitle = "Paitent";
                        newCardData.hidePos = anchorPoints[i].anchoredPosition;

                        cardDataList.Add(newCardData);
                    }

                    break;
                default:
            }

            GeneratedCard(cardDataList);
        }
        private void Debug_HideCards(ACheatData data)
        {
            HideCards();
        }
        private void Debug_PopCards(ACheatData data)
        {
            PopUpCards();
        }
        private void Debug_Log(string msg)
        {
            if (!PlatformConfigs.DebugEnabled) return;
            UnityEngine.Debug.Log("[ClinicCardPanel]: " + msg);
        }
        #endregion
    }
}
