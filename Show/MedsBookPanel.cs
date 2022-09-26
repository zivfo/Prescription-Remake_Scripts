using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using RefinedGame.Data;
using RefinedGame.Logic;
using RefinedGame.Tool.DebugUtil;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RefinedGame.Show
{
    public class MedsBookPanel : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] RectTransform animeCover = null;
        [SerializeField] RectTransform animePage = null;
        [SerializeField] RectTransform diseaseChunkTemplate = null;
        [SerializeField] TMP_Text diseaseCountTxt = null;
        [SerializeField] Transform diseaseChunkParent = null;
        [Header("Params")]
        [SerializeField] float pageMoveSpan = 25.0f;
        [SerializeField] float coverShiftSpan = 25.0f;
        [SerializeField] float coverPreOpenSpan = 10.0f;
        [SerializeField] float coverOpenSpan = 230.0f;
        [SerializeField] float coverShiftTime = 0.2f;
        [SerializeField] float coverPreOpenTime = 0.2f;
        [SerializeField] float coverOpenTime = 0.4f;
        [SerializeField] float coverCloseTime = 0.3f;
        [SerializeField] float switchSpeed = 0.04f;


        readonly Vector2 bookDockPos = new Vector2(-300.0f, -185.0f);
        readonly Vector2 bookFullPos = new Vector2(-125.0f, -5.0f);

        RectTransform animeBookRect;
        bool isDocking = true;
        bool interactable = true;
        bool isOPen = false;
        Vector2 pageHidePos;
        Vector2 coverClosePos;
        Vector2 BookPos => isDocking ? bookDockPos : bookFullPos;
        float PageOpenX => pageHidePos.x + pageMoveSpan;
        float CoverTouchDownX => coverClosePos.x - coverShiftSpan;
        float CoverPreOpenX => coverClosePos.x + coverPreOpenSpan;
        float CoverOpenX => coverClosePos.x - coverOpenSpan;

        public bool Interactable { get => interactable; set => interactable = value; }

        List<RectTransform> currentDiseaseChunks = new List<RectTransform>();

        #region Unity Functions
        private void OnDestroy()
        {
            GameController.instance.runner.diseaseHandler.UnRegisterToDiseaseUpdates(OnDiseaseUpdated);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Book_CloseMedsBook, Cheat_CloseBook);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Book_SwitchBookPos, Cheat_SwitchBookPos);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
                return;

            OpenBook(true);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable)
                return;

            CoverPreOpen(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable)
                return;

            CoverPreOpen(false);
        }
        #endregion

        #region Public Functions
        public void OnPhaseChanged(LevelHandler.Phase newPhase)
        {
            switch (newPhase)
            {
                case LevelHandler.Phase.Preparing:
                    SwitchBookPos(false);
                    break;
                case LevelHandler.Phase.OnDuty:
                    SwitchBookPos(true);
                    break;
            }
        }
        public void Init()
        {
            diseaseChunkTemplate.gameObject.SetActive(false);
            animeBookRect = GetComponent<RectTransform>();
            pageHidePos = animePage.anchoredPosition;
            coverClosePos = animeCover.anchoredPosition;

            GameController.instance.runner.diseaseHandler.RegisterToDiseaseUpdates(OnDiseaseUpdated);
            DebugEventsController.GetInstance().Subscribe(CheatType.Book_CloseMedsBook, Cheat_CloseBook);
            DebugEventsController.GetInstance().Subscribe(CheatType.Book_SwitchBookPos, Cheat_SwitchBookPos);
        }
        public void CloseBookCover()
        {
            OpenBook(false);
            //TODO: make it right
            // interactable = true;
        }
        #endregion

        #region Private Functions
        private void OnDiseaseUpdated(Disease disease)
        {
            if (!disease.data.isKnown) return;

            var newChunk = Instantiate(diseaseChunkTemplate, diseaseChunkParent, false);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{disease.data.theName} ");
            foreach (var symptom in disease.data.symptoms)
            {
                sb.AppendLine("-" + " " + symptom);
            }
            newChunk.GetComponentInChildren<TMP_Text>().text = sb.ToString();

            newChunk.gameObject.SetActive(true);
            currentDiseaseChunks.Add(newChunk);

            diseaseCountTxt.text = $"{currentDiseaseChunks.Count}";
        }
        private void CoverPreOpen(bool open)
        {
            animeCover.DOKill();
            float x = open ? CoverTouchDownX : coverClosePos.x;
            animeCover.DOAnchorPosX(x, coverShiftTime).SetEase(Ease.OutQuint);
        }
        private void CoverFullOpen(bool open)
        {
            animeCover.DOKill();
            if (!open)
                animeCover.DOAnchorPosX(coverClosePos.x, coverCloseTime).SetEase(Ease.OutCubic);
            else
                animeCover.DOAnchorPosX(CoverPreOpenX, coverPreOpenTime).SetEase(Ease.OutBack).OnComplete(() => animeCover.DOAnchorPosX(CoverOpenX, coverOpenTime).SetEase(Ease.OutCirc));
        }
        private void OpenBook(bool open)
        {
            interactable = false;

            if (!open)
            {
                CoverFullOpen(false);
                animePage.DOKill();
                animePage.DOAnchorPosX(pageHidePos.x, 0.5f).SetEase(Ease.OutQuad);
            }
            else
            {
                var sequence = DOTween.Sequence();
                sequence.AppendCallback(() => CoverPreOpen(true))
                    .AppendCallback(() =>
                    {
                        CoverFullOpen(true);
                        animePage.DOKill();
                        animePage.DOAnchorPosX(PageOpenX, 0.5f).SetEase(Ease.OutQuad);
                    });
            }
        }
        private void SwitchBookPos(bool dock)
        {
            animeBookRect.DOKill();
            isDocking = dock;

            var distanceX = Math.Abs(bookFullPos.x - bookDockPos.x);
            var distanceY = Math.Abs(bookFullPos.y - bookDockPos.y);

            var speedRec = switchSpeed;


            if (dock)
            {
                var sequence = DOTween.Sequence();
                sequence.AppendCallback(() => OpenBook(false))
                    .AppendInterval(coverCloseTime)
                    .Append(animeBookRect.DOAnchorPosX(BookPos.x, distanceX * speedRec))
                    .Append(animeBookRect.DOAnchorPosY(BookPos.y, distanceY * speedRec))
                    .AppendCallback(() =>
                    {
                        //TODO: make it right
                        // interactable = true;
                    });
            }
            else
            {
                var sequence = DOTween.Sequence();
                sequence.AppendCallback(() => OpenBook(false))
                    .AppendInterval(coverCloseTime)
                    .Append(animeBookRect.DOAnchorPosX(BookPos.x, distanceX * speedRec))
                    .Append(animeBookRect.DOAnchorPosY(BookPos.y, distanceY * speedRec))
                    .AppendCallback(() =>
                    {
                        OpenBook(true);
                    });
            }
        }
        #endregion

        #region Debug Functions
        private void Cheat_CloseBook(ACheatData data)
        {
            CloseBookCover();
        }
        private void Cheat_SwitchBookPos(ACheatData data)
        {
            SwitchBookPos(!isDocking);
        }
        #endregion
    }
}
