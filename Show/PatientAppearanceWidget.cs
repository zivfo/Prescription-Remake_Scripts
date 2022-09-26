using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Data;
using RefinedGame.Logic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class PatientAppearanceWidget : MonoBehaviour
    {
        [SerializeField] Image hand = null;
        [SerializeField] Image body = null;
        [SerializeField] Image head = null;
        [SerializeField] Image mouth = null;
        [SerializeField] Image eye = null;
        [SerializeField] Image hair = null;
        [Header("Params")]
        [SerializeField] float speedRec = 0.009f;
        [SerializeField] int oneStepLengthMax = 70;
        [SerializeField] int oneStepLengthMin = 40;

        RectTransform rect;

        #region Unity Functions
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        #endregion
        #region Public Functions
        public void FormAppearance(Guid patientAppearanceId)
        {
            var appearanceData = GameController.instance.patientVisualController.GetDataFromId(patientAppearanceId);

            this.hand.sprite = appearanceData.hand;
            this.body.sprite = appearanceData.body;
            this.head.sprite = appearanceData.head;
            this.mouth.sprite = appearanceData.mouth;
            this.eye.sprite = appearanceData.eye;
            this.hair.sprite = appearanceData.hair;
        }
        public void SetDarkness(bool inDark = false)
        {
            if (!inDark)
            {
                hand.color = Color.white;
                body.color = Color.white;
                head.color = Color.white;
                mouth.color = Color.white;
                eye.color = Color.white;
                hair.color = Color.white;
            }
            else
            {
                hand.color = Color.black;
                body.color = Color.black;
                head.color = Color.black;
                mouth.color = Color.black;
                eye.color = Color.black;
                hair.color = Color.black;
            }
        }
        public void AppearDead(bool reverse = false)
        {
            if (reverse)
            {
                hand.color = Color.white;
                body.color = Color.white;
                head.color = Color.white;
                mouth.color = Color.white;
                eye.color = Color.white;
                hair.color = Color.white;
            }
            else
            {
                hand.color = GameController.instance.patientVisualController.deadBodyColor;
                body.color = GameController.instance.patientVisualController.deadHairColor;
                head.color = GameController.instance.patientVisualController.deadBodyColor;
                mouth.color = GameController.instance.patientVisualController.deadMouthColor;
                eye.color = GameController.instance.patientVisualController.deadEyeColor;
                hair.color = GameController.instance.patientVisualController.deadHairColor;
            }
        }
        public void WalkToPos(Vector2 pos, Action callBackWhenDone)
        {
            rect.DOKill();

            var stepLength = UnityEngine.Random.Range(oneStepLengthMin, oneStepLengthMax);
            int stepsTake = Mathf.RoundToInt((pos - rect.anchoredPosition).magnitude / stepLength);
            Vector2 direction = (pos - rect.anchoredPosition).normalized;
            var sequence = DOTween.Sequence();
            Vector2 nextPos = rect.anchoredPosition;
            var speed = speedRec + UnityEngine.Random.Range(-0.002f, 0.002f);

            List<Vector2> waypoints = new List<Vector2>();
            for (int i = 0; i < stepsTake; i++)
            {
                waypoints.Add(nextPos);
                nextPos = nextPos + direction * stepLength;
            }

            foreach (var point in waypoints)
            {
                sequence.AppendCallback(() => rect.DOAnchorPosX(point.x, stepLength * speed));
                sequence.Append(rect.DOAnchorPosY(point.y + UnityEngine.Random.Range(3, 6), stepLength * speed * UnityEngine.Random.Range(0.3f, 0.50f)));
                sequence.Append(rect.DOAnchorPosY(point.y, stepLength * speed * UnityEngine.Random.Range(0.3f, 0.50f)).SetEase(Ease.OutBack));
                sequence.AppendInterval(UnityEngine.Random.Range(0.0f, 0.01f));
            }

            sequence.AppendCallback(() => rect.DOAnchorPosX(pos.x, stepLength * speed));
            sequence.Append(rect.DOAnchorPosY(pos.y + 6, stepLength * speed * 0.5f));
            sequence.Append(rect.DOAnchorPosY(pos.y, stepLength * speed * 0.5f).SetEase(Ease.OutBack));
            sequence.AppendCallback(() =>
            {
                if (callBackWhenDone != null)
                {
                    rect.DOKill();
                    callBackWhenDone();
                }
            });
        }
        #endregion

        #region Private Functions
        #endregion


    }

}