using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Data;
using RefinedGame.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public enum ClinicCardType
    {
        LetterCard, PatientCard,
    }

    public class ClinicCardData
    {
        public string cardTitle;
        public Action<ClinicCardData> onClickCallBack;
        public Vector2 hidePos;
        public ClinicCardType cardType;
        public object content;
    }

    public class ClinicCardTemplate : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
    {
        [SerializeField] TMP_Text titleTxt = null;
        [SerializeField] Image cardImage;
        [SerializeField] Sprite letterCardSprite = null;
        [SerializeField] Sprite patientCardSprite = null;
        [SerializeField] Color letterCardTxtColor;
        [SerializeField] Color patientCardTxtColor;
        [Header("Params")]
        [SerializeField] float cardPopTime = 0.2f;
        [SerializeField] float cardHideTime = 0.2f;

        readonly float popPosY = 25.0f;
        readonly float touchDownY = 5.0f;
        readonly float hidePosY = 80.0f;

        public System.Guid id;
        public System.Guid pairSlotId = Guid.Empty;

        public bool isHidden = false;
        public bool hasInteracted = false;
        public bool Interactable { get => interactable; set => interactable = value; }

        ClinicCardData cardData;
        Action<ClinicCardData> onCardClicked;
        RectTransform rect;
        Vector2 hidePos = Vector2.one;
        bool interactable = true;



        #region Unity Functions
        private void OnDestroy()
        {
            rect.DOKill();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable)
                return;
            CardTouch(false);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable)
                return;
            CardTouch(true);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
                return;

            hasInteracted = true;

            if (onCardClicked != null)
                onCardClicked(cardData);

            Hide();
        }
        #endregion

        #region Public Functions
        public void Pop(Ease popStyle, bool forceInstanct = false)
        {
            if (hasInteracted)
                return;

            isHidden = false;
            gameObject.SetActive(true);
            rect.DOKill();

            if (forceInstanct)
            {
                rect.DOAnchorPos(hidePos, 0.0f);
                interactable = true;
            }
            else
            {
                interactable = false;
                rect.DOAnchorPosY(popPosY, cardPopTime).SetEase(popStyle).OnComplete(() => interactable = true);
            }
        }
        public void Hide(bool forceInstanct = false)
        {
            isHidden = true;
            interactable = false;
            rect.DOKill();
            if (forceInstanct)
                rect.DOAnchorPos(hidePos, 0.0f).OnComplete(() => gameObject.SetActive(false));
            else
                rect.DOAnchorPosY(hidePosY, cardHideTime).OnComplete(() => gameObject.SetActive(false));
        }
        public void Init(ClinicCardData cardData)
        {
            this.cardData = cardData;

            rect = GetComponent<RectTransform>();
            onCardClicked = cardData.onClickCallBack;

            cardImage.sprite = cardData.cardType == ClinicCardType.LetterCard ? letterCardSprite : patientCardSprite;
            titleTxt.color = cardData.cardType == ClinicCardType.LetterCard ? letterCardTxtColor : patientCardTxtColor;

            this.hidePos = cardData.hidePos;
            rect.anchoredPosition = hidePos;

            titleTxt.text = cardData.cardTitle;
        }
        #endregion

        #region Private Functions
        private void CardTouch(bool recover)
        {
            rect.DOKill();
            var pos = recover ? popPosY : touchDownY;
            rect.DOAnchorPosY(pos, 0.1f);
        }
        #endregion
    }
}
