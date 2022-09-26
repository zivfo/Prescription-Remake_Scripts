using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using RefinedGame.Data;
using RefinedGame.Tool.DebugUtil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    [Serializable]
    public class RewardSprties
    {
        public RewardData.RewardType type;
        public Sprite sprite;
    }
    public class ClinicLetterPanel : MonoBehaviour
    {
        [SerializeField] List<RewardSprties> rewardSprties = new List<RewardSprties>();
        [SerializeField] Image darkCover = null;
        [SerializeField] CanvasGroup content = null;
        [SerializeField] TMP_Text titleTxt = null;
        [SerializeField] TMP_Text txtContentSolo = null;
        [SerializeField] TMP_Text txtContentWithPhoto = null;
        [SerializeField] Image rewardIcon = null;
        [SerializeField] TMP_Text rewardAmountTxt = null;
        [SerializeField] GameObject rewardChunkTemplate = null;
        [SerializeField] Transform rewardChunkParent = null;
        [SerializeField] PatientAppearanceWidget patientAppearanceWidget = null;
        [SerializeField] GameObject pureTxtChannel = null;
        [SerializeField] GameObject txtWithPhotoChannel = null;
        [SerializeField] Button panelCloseBtn = null;

        RectTransform rect;
        ClinicWindow owner;
        List<GameObject> currentRewardChunks = new List<GameObject>();
        EventLetterData letterData;

        #region Unity Functions
        #endregion

        #region Public Functions
        public void Init(ClinicWindow clinicWindow)
        {
            this.owner = clinicWindow;
            rect = content.GetComponent<RectTransform>();

            rewardChunkTemplate.gameObject.SetActive(false);
            panelCloseBtn.onClick.AddListener(() => HideLetter());
        }
        public void SetData(EventLetterData letterData)
        {
            this.letterData = letterData;

            if (letterData.patientAppearanceId != Guid.Empty)
            {
                SetChannel(2);

                titleTxt.text = letterData.letterTitle;
                StringBuilder sb = new StringBuilder();
                foreach (var aSentence in letterData.txtContent)
                    sb.Append(aSentence + "\n");
                txtContentWithPhoto.text = $"{sb}";
                patientAppearanceWidget.FormAppearance(letterData.patientAppearanceId);
                patientAppearanceWidget.AppearDead();
            }
            else
            {
                SetChannel(1);

                titleTxt.text = letterData.letterTitle;
                StringBuilder sb = new StringBuilder();
                foreach (var aSentence in letterData.txtContent)
                    sb.Append(aSentence + "\n");
                txtContentSolo.text = $"{sb}";
            }

            foreach (var rewardChunk in currentRewardChunks)
            {
                Destroy(rewardChunk.gameObject);
            }
            currentRewardChunks.Clear();

            if (letterData.rewardData != null)
            {
                foreach (var rewardChunk in currentRewardChunks)
                {
                    Destroy(rewardChunk.gameObject);
                }
                currentRewardChunks.Clear();

                foreach (var aReward in letterData.rewardData.rewards)
                {
                    var newRewardChunk = Instantiate(rewardChunkTemplate, rewardChunkParent, false);

                    switch (aReward.Key)
                    {
                        case RewardData.RewardType.Money:
                            foreach (var sprite in rewardSprties)
                            {
                                if (sprite.type == RewardData.RewardType.Money)
                                    newRewardChunk.GetComponentInChildren<Image>().sprite = sprite.sprite;

                            }
                            break;
                        case RewardData.RewardType.Credit:
                            foreach (var sprite in rewardSprties)
                            {
                                if (sprite.type == RewardData.RewardType.Credit)
                                    newRewardChunk.GetComponentInChildren<Image>().sprite = sprite.sprite;

                            }
                            break;
                    }
                    newRewardChunk.GetComponentInChildren<TMP_Text>().text = $"{aReward.Value}";

                    newRewardChunk.gameObject.SetActive(true);
                    currentRewardChunks.Add(newRewardChunk);
                }
            }
        }
        public void PopLetter()
        {
            if (!gameObject.activeInHierarchy)
            {
                owner.PromoteLetterPanelLayer(false);
                gameObject.SetActive(true);
            }
            panelCloseBtn.interactable = false;


            darkCover.DOKill();
            darkCover.DOFade(0.5f, 0.2f);
            darkCover.raycastTarget = true;

            content.DOKill();
            content.DOFade(1, 0.2f);

            rect.DOKill();
            rect.DOPunchAnchorPos(Vector2.down * 50, 0.2f, 0, 1).OnComplete(() => panelCloseBtn.interactable = true);
        }
        #endregion

        #region Private Functions
        private void SetChannel(int channel)
        {
            switch (channel)
            {
                case 1:
                    pureTxtChannel.gameObject.SetActive(true);
                    txtWithPhotoChannel.gameObject.SetActive(false);
                    break;
                case 2:
                    pureTxtChannel.gameObject.SetActive(false);
                    txtWithPhotoChannel.gameObject.SetActive(true);
                    break;
            }
        }

        private void HideLetter()
        {
            panelCloseBtn.interactable = false;
            owner.HandleLetterPanelOpenClose(letterData.rewardData);

            darkCover.DOKill();
            darkCover.DOFade(0, 0.2f);
            darkCover.raycastTarget = false;

            content.DOKill();
            content.DOFade(0, 0.2f);

            rect.DOKill();
            rect.DOPunchAnchorPos(Vector2.up * 50, 0.2f, 0, 1).OnComplete(() =>
            {
                gameObject.SetActive(false);
                owner.PromoteLetterPanelLayer(true);
            });
        }
        #endregion
    }
}
