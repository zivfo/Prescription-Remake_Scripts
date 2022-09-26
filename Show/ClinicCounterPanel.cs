using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Data;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class ClinicCounterPanel : MonoBehaviour
    {
        [SerializeField] RectTransform letterInitPos = null;
        [SerializeField] RectTransform letterPopPos = null;
        [SerializeField] Button letterEnvelopBtn = null;
        [SerializeField] RectTransform prescriptionInitPos = null;
        [SerializeField] RectTransform prescriptionPopPos = null;
        [SerializeField] RectTransform prescriptionRect = null;


        private RectTransform envelopRect;
        private ClinicWindow owner;


        #region Unity Functions
        #endregion

        #region Public Functions
        public void Init(ClinicWindow clinicWindow)
        {
            this.owner = clinicWindow;

            letterEnvelopBtn.gameObject.SetActive(false);
            prescriptionRect.gameObject.SetActive(false);

            envelopRect = letterEnvelopBtn.GetComponent<RectTransform>();

            letterEnvelopBtn.onClick.AddListener(() =>
            {
                owner.PopLetter();
            });
        }
        public void PopEnvelop()
        {
            letterEnvelopBtn.interactable = false;
            envelopRect.anchoredPosition = letterInitPos.anchoredPosition;
            letterEnvelopBtn.gameObject.SetActive(true);

            envelopRect.DOKill();
            envelopRect.DOAnchorPos(letterPopPos.anchoredPosition, 0.2f).SetEase(Ease.OutQuart).OnComplete(() => letterEnvelopBtn.interactable = true);
        }
        public void HideEnvelop()
        {
            letterEnvelopBtn.interactable = false;

            envelopRect.DOKill();
            envelopRect.DOAnchorPos(letterInitPos.anchoredPosition, 0.2f).SetEase(Ease.OutCirc).OnComplete(() => letterEnvelopBtn.gameObject.SetActive(false));
        }
        public void PopPrescription()
        {
            prescriptionRect.anchoredPosition = prescriptionInitPos.anchoredPosition;
            prescriptionRect.gameObject.SetActive(true);

            prescriptionRect.DOKill();
            prescriptionRect.DOAnchorPos(prescriptionPopPos.anchoredPosition, 0.3f).SetEase(Ease.OutFlash);
        }
        public void HidePrescription()
        {
            prescriptionRect.DOKill();
            prescriptionRect.DOAnchorPos(prescriptionInitPos.anchoredPosition, 0.3f).SetEase(Ease.InOutCubic).OnComplete(() => prescriptionRect.gameObject.SetActive(false));
        }
        #endregion
    }
}
