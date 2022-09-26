using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Logic;
using TMPro;
using UnityEngine;


namespace RefinedGame.Show
{
    public class ClinicHallwayPanel : MonoBehaviour
    {
        [SerializeField] RectTransform initSpot = null;
        [SerializeField] RectTransform standingSpot = null;
        [SerializeField] RectTransform leavingSpot = null;
        [SerializeField] TMP_Text conversationTxt = null;
        [SerializeField] PatientAppearanceWidget patientAppearanceTemplate = null;
        [SerializeField] RectTransform appearanceParent = null;
        [Header("Params")]
        [SerializeField] float conversationPopSpeed = 0.1f;

        List<GameObject> usedAvatars = new List<GameObject>();
        PatientAppearanceWidget currentAvatar;
        ClinicWindow owner;


        #region Unity Functions
        private void OnDisable()
        {
            ClearUsedAvatars();
        }
        #endregion

        #region Public Functions
        public void Init(ClinicWindow clinicWindow)
        {
            this.owner = clinicWindow;
            patientAppearanceTemplate.gameObject.SetActive(false);
            conversationTxt.alpha = 1;
            conversationTxt.text = "";
        }
        public void SendAPatient()
        {
            currentAvatar = GeneratePatientAvatar(owner.CurrentPatient);
            currentAvatar.WalkToPos(standingSpot.anchoredPosition, OnPatientWalkedToPos);
        }
        #endregion

        #region Private Functions
        private void OnPatientWalkedToPos()
        {
            PopConversation(0);

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(1);
            sequence.AppendCallback(() => owner.PrescriptionSubmitted());
            sequence.AppendCallback(() => GameController.instance.CloseMedsBook());
        }
        private void PopConversation(int speechType)
        {
            List<string> targetSpeech = new List<string>();

            if (speechType == 0)
                targetSpeech = owner.CurrentPatient.FormSpeech();
            else
                targetSpeech = owner.CurrentPatient.GetSpeechSentence(speechType);

            conversationTxt.DOKill();
            conversationTxt.DOFade(0, 0.2f);
            var conversationSequnce = DOTween.Sequence(conversationTxt);

            //TODO: should have a proper dialogue queue but anyway
            foreach (var aSentence in targetSpeech)
            {
                conversationSequnce.AppendInterval(0.5f);
                conversationSequnce.AppendCallback(() =>
                {
                    conversationTxt.alpha = 1;
                    conversationTxt.text = aSentence;
                });
                conversationSequnce.Append(conversationTxt.DOScale(1.2f, 0.2f));
                conversationSequnce.Append(conversationTxt.DOScale(1f, 0.1f));
                conversationSequnce.AppendInterval(2);
                conversationSequnce.Append(conversationTxt.DOFade(0, 0.2f));

                conversationSequnce.AppendCallback(() =>
                {
                    conversationTxt.text = "";
                    conversationTxt.alpha = 1;
                });
            }
        }
        private PatientAppearanceWidget GeneratePatientAvatar(Patient patient)
        {
            var newPatientAvatar = Instantiate(patientAppearanceTemplate, appearanceParent, false);
            newPatientAvatar.GetComponent<RectTransform>().anchoredPosition = initSpot.anchoredPosition;
            newPatientAvatar.FormAppearance(patient.appearanceData);
            newPatientAvatar.SetDarkness(true);

            newPatientAvatar.gameObject.SetActive(true);
            return newPatientAvatar;
        }

        public void SendCurrentPatientAway()
        {
            PopConversation(4);
            currentAvatar.WalkToPos(leavingSpot.anchoredPosition, ClearUsedAvatars);
            usedAvatars.Add(currentAvatar.gameObject);
            currentAvatar = null;
        }

        private void ClearUsedAvatars()
        {
            foreach (var avatarObj in usedAvatars)
            {
                Destroy(avatarObj);
            }
            usedAvatars.Clear();
        }
        #endregion
    }
}
