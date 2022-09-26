using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class PatientChunkTemplate : MonoBehaviour
    {
        [SerializeField] PatientAppearanceWidget patientAppearanceWidget = null;
        [SerializeField] TMP_Text nameTxt = null;
        [SerializeField] TMP_Text skillTxt = null;
        [SerializeField] TMP_Text talentTxt = null;
        [SerializeField] TMP_Text growthTxt = null;
        [SerializeField] Button ondutyBtn = null;
        [SerializeField] Button lvlUpBtn = null;
        [SerializeField] Color originalTxtColor = Color.black;



        public Patient patient;

        public void PatientSkillGrow()
        {
            skillTxt.text = $"{patient.abilityData.skill}";
        }
        public void Init(Patient patient)
        {
            this.patient = patient;

            patientAppearanceWidget.FormAppearance(patient.appearanceData);
            nameTxt.text = patient.data.theName;
            skillTxt.text = $"{patient.abilityData.skill}";
            talentTxt.text = patient.abilityData.talentData.content;
            talentTxt.color = GameController.instance.patientVisualController.GetLevelColor(patient.abilityData.level);
            growthTxt.text = $"{patient.abilityData.growth}";

            lvlUpBtn.GetComponentInChildren<TMP_Text>().text = $"{patient.abilityData.lvlUpCost}";

            ondutyBtn.onClick.AddListener(() =>
            {
                GameController.instance.runner.patientHandler.SwitchPatientStatus(patient);
                ondutyBtn.transform.GetChild(1).GetComponent<Image>().sprite = GameController.instance.patientVisualController.GetStatusSprite(patient.abilityData.status);
            });
            lvlUpBtn.onClick.AddListener(() =>
            {
                GameController.instance.runner.patientHandler.LevelUpPatient(patient);

                var lvlTxt = lvlUpBtn.GetComponentInChildren<TMP_Text>();
                lvlTxt.DOKill();
                lvlTxt.GetComponent<RectTransform>().DOPunchAnchorPos(Vector2.down * 10, 0.1f, 0);
                lvlTxt.text = $"{patient.abilityData.lvlUpCost}";

                growthTxt.DOKill();
                growthTxt.DOCounter(patient.abilityData.growth - 1, patient.abilityData.growth, 0.2f, false);
                growthTxt.DOColor(Color.green, 0.2f).OnComplete(() => growthTxt.DOColor(originalTxtColor, 0.2f));
            });
        }
        public void UpdateStatusIcon()
        {
            ondutyBtn.transform.GetChild(1).GetComponent<Image>().sprite = GameController.instance.patientVisualController.GetStatusSprite(patient.abilityData.status);
        }
        public void CheckLvlUpAvailable(int value)
        {
            lvlUpBtn.interactable = value >= patient.abilityData.lvlUpCost ? true : false;
        }
    }
}
