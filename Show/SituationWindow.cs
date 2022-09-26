using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RefinedGame.Data;
using RefinedGame.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class SituationWindow : AUiWindow
    {
        [SerializeField] List<PlayerDataWidget> playerDataWidgets = null;
        [SerializeField] Button skillLabelBtn = null;
        [SerializeField] Button growthLabelBtn = null;
        [SerializeField] Button talentLabelBtn = null;
        [SerializeField] Button backBtn = null;
        [SerializeField] Button hireBtn = null;
        [SerializeField] Button deadBtn = null;
        [SerializeField] Transform patientChunkParent = null;
        [SerializeField] PatientChunkTemplate patientChunkTemplate = null;
        [SerializeField] TMP_Text hiringPriceHintTxt = null;
        [SerializeField] SituationBarWidget situationBarWidget = null;

        List<PatientChunkTemplate> currentChunks = new List<PatientChunkTemplate>();

        public enum LabelType
        {
            Skill, Talent, Growth,
        }

        #region Unity Functions
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameController.instance.runner.LevelSettlement();
            }
        }
        private void OnEnable()
        {
            foreach (var patientChunk in currentChunks)
            {
                patientChunk.UpdateStatusIcon();
            }
        }
        private void OnDestroy()
        {
            GameController.instance.runner.patientHandler.UnRegisterToPatientUpdates(OnPatientsUpdated);
            GameController.instance.runner.patientHandler.UnRegisterToPatientLevelUp(OnPatientLeveledUp);
        }
        #endregion

        #region Public Functions
        public override void Init()
        {
            GameController.instance.runner.patientHandler.RegisterToPatientUpdates(OnPatientsUpdated);
            GameController.instance.runner.patientHandler.RegisterToPatientLevelUp(OnPatientLeveledUp);

            situationBarWidget.Init();
            foreach (var widget in playerDataWidgets)
            {
                widget.Init(OnWidgetValuedChanged);
            }
            patientChunkTemplate.gameObject.SetActive(false);


            hiringPriceHintTxt.text = $"[HIRE: {Player.configData.hiringPrice}]";
            hireBtn.onClick.AddListener(() => GameController.instance.CallOnePatient());
            backBtn.onClick.AddListener(() => GameController.instance.runner.ToPreparation());
            skillLabelBtn.onClick.AddListener(() => SortChunksByLabelOrder(LabelType.Skill));
            talentLabelBtn.onClick.AddListener(() => SortChunksByLabelOrder(LabelType.Talent));
            growthLabelBtn.onClick.AddListener(() => SortChunksByLabelOrder(LabelType.Growth));
            deadBtn.onClick.AddListener(() => throw new NotImplementedException());
        }
        #endregion

        #region Private Functions
        private void SortChunksByLabelOrder(LabelType skill)
        {
            switch (skill)
            {
                case LabelType.Skill:
                    currentChunks = currentChunks.OrderByDescending(x => x.patient.abilityData.skill).ToList();
                    break;
                case LabelType.Talent:
                    currentChunks = currentChunks.OrderByDescending(x => x.patient.abilityData.talentData.content).ToList();
                    break;
                case LabelType.Growth:
                    currentChunks = currentChunks.OrderByDescending(x => x.patient.abilityData.growth).ToList();
                    break;
            }
            for (int i = 0; i < currentChunks.Count; i++)
            {
                currentChunks[i].transform.SetSiblingIndex(i);
            }
        }
        private void OnPatientLeveledUp(Patient patient)
        {
            var chunk = currentChunks.Find(x => x.patient == patient);
            chunk.PatientSkillGrow();
        }
        private void OnPatientsUpdated(Patient patient, bool set)
        {
            if (set)
            {
                var newPaitentChunk = Instantiate(patientChunkTemplate, patientChunkParent, false);
                newPaitentChunk.Init(patient);
                newPaitentChunk.gameObject.SetActive(true);

                newPaitentChunk.transform.SetAsFirstSibling();
                hiringPriceHintTxt.transform.SetAsLastSibling();
                currentChunks.Add(newPaitentChunk);
            }
            else
            {
                var chunk = currentChunks.Find(x => x.patient == patient);
                currentChunks.Remove(chunk);
                Destroy(chunk.gameObject);
            }

        }
        private void OnWidgetValuedChanged(Player.PlayerDataType type, int value, int amount)
        {
            switch (type)
            {
                case Player.PlayerDataType.Money:
                    hireBtn.interactable = Player.IsHiringEnabled();
                    foreach (var chunk in currentChunks)
                    {
                        chunk.CheckLvlUpAvailable(value);
                    }
                    break;
                case Player.PlayerDataType.PatientSlot:
                    hireBtn.interactable = Player.IsHiringEnabled();
                    hireBtn.GetComponentInChildren<TMP_Text>().text = $"Hire ({value} | 8)";
                    break;
                default: break;
            }
        }
        #endregion
    }
}
