using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RefinedGame.Logic;
using RefinedGame.Tool.DebugUtil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class ClinicNotePanel : MonoBehaviour
    {
        [SerializeField] CanvasGroup prescriptionChannel = null;
        [SerializeField] TMP_Text patientName = null;
        [SerializeField] TMP_Text patientSex = null;
        [SerializeField] TMP_Text patientAge = null;
        [SerializeField] TMP_Text patientSkill = null;
        [SerializeField] TMP_Text patientGrowth = null;
        [SerializeField] PatientAppearanceWidget patientAppearanceWidget = null;
        [SerializeField] Transform symptomChunkParent = null;
        [SerializeField] GameObject symptomChunkTemplate = null;
        [SerializeField] Transform diagnosisBtnParent = null;
        [SerializeField] ClinicNoteDiagnosisBtnTemplate diagnosisBtnTemplate = null;
        [SerializeField] Button signatureBtn = null;
        [SerializeField] GameObject signatureHintObj = null;
        [SerializeField] RectTransform signatureRect = null;
        [SerializeField] GameObject signatureDisabledObj = null;
        [SerializeField] Button emergencyBtn = null;
        [SerializeField] GameObject alreadyEmergencyObj = null;
        [SerializeField] GameObject sentToEmergencyObj = null;
        [SerializeField] GameObject emergencyDisabledObj = null;


        [Header("Params")]
        [SerializeField] float switchSpeed = 0.04f;

        readonly Vector2 normalPos = new Vector2(399, -142);
        readonly Vector2 popPos = new Vector2(163, -52);


        ClinicWindow owner;
        RectTransform bookRect;
        bool isDocking = true;
        Vector2 BookPos => isDocking ? normalPos : popPos;
        List<GameObject> currentSymptomChunks = new List<GameObject>();
        List<ClinicNoteDiagnosisBtnTemplate> currentDiagnosisBtns = new List<ClinicNoteDiagnosisBtnTemplate>();
        System.Guid selectedDiagnosisId;
        bool noDiagnosisBtnClickedYet = true;


        #region Unity Functions
        private void OnDestroy()
        {
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Clinic_SwitchNotePos, Cheat_SwitchNotePos);
            GameController.instance.runner.diseaseHandler.UnRegisterToDiseaseUpdates(OnDiseaseUpdated);
        }
        #endregion

        #region Public Functions
        public void Init(ClinicWindow clinicWindow)
        {
            this.owner = clinicWindow;
            prescriptionChannel.alpha = 0;
            bookRect = GetComponent<RectTransform>();
            symptomChunkTemplate.gameObject.SetActive(false);
            diagnosisBtnTemplate.gameObject.SetActive(false);

            //Add Empty Diagnosis
            var newDiagnosisChunk = Instantiate(diagnosisBtnTemplate, diagnosisBtnParent, false);
            newDiagnosisChunk.Init(null, OnDiagnosisBtnClicked);
            newDiagnosisChunk.gameObject.SetActive(true);
            currentDiagnosisBtns.Add(newDiagnosisChunk);

            signatureBtn.onClick.AddListener(() =>
            {
                if (signatureDisabledObj.activeSelf)
                    return;

                signatureBtn.interactable = false;
                signatureRect.gameObject.SetActive(true);
                signatureHintObj.gameObject.SetActive(false);

                signatureRect.DOKill();
                signatureRect.DOPunchAnchorPos(Vector2.right * 20, 0.1f, 0);
                bookRect.DOKill();
                bookRect.DOPunchAnchorPos(Vector2.down * 30, 0.3f, 0);

                ConfirmDiagnosis(selectedDiagnosisId == owner.CurrentPatient.GetCurrentDisease().data.id);
                var sequence = DOTween.Sequence();
                sequence.AppendInterval(0.3f);
                sequence.AppendCallback(() => SwitchPos(!isDocking));
            });
            emergencyBtn.onClick.AddListener(() =>
            {
                if (alreadyEmergencyObj.activeSelf || emergencyDisabledObj.activeSelf)
                    return;

                sentToEmergencyObj.gameObject.SetActive(!sentToEmergencyObj.activeSelf);
                owner.CurrentPatient.abilityData.status = sentToEmergencyObj.activeSelf ? Data.AbilityData.Status.InICU : Data.AbilityData.Status.Onduty;

                if (sentToEmergencyObj.activeSelf)
                {
                    if (noDiagnosisBtnClickedYet)
                    {
                        signatureHintObj.gameObject.SetActive(false);
                        signatureDisabledObj.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (selectedDiagnosisId == Guid.Empty)
                        {
                            signatureHintObj.gameObject.SetActive(true);
                            signatureDisabledObj.gameObject.SetActive(false);
                        }
                        else
                        {
                            signatureHintObj.gameObject.SetActive(false);
                            signatureDisabledObj.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    if (noDiagnosisBtnClickedYet)
                    {
                        signatureHintObj.gameObject.SetActive(false);
                        signatureDisabledObj.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (selectedDiagnosisId == Guid.Empty)
                        {
                            signatureHintObj.gameObject.SetActive(false);
                            signatureDisabledObj.gameObject.SetActive(true);
                        }
                        else
                        {
                            signatureHintObj.gameObject.SetActive(true);
                            signatureDisabledObj.gameObject.SetActive(false);
                        }
                    }
                }

            });

            DebugEventsController.GetInstance().Subscribe(CheatType.Clinic_SwitchNotePos, Cheat_SwitchNotePos);
            GameController.instance.runner.diseaseHandler.RegisterToDiseaseUpdates(OnDiseaseUpdated);

        }
        public void PatientDataToPrescriptionNote()
        {
            SwitchPos(!isDocking);
            signatureBtn.interactable = true;
            var patient = owner.CurrentPatient;

            //TODO: use music pitch to show right or wrong for diagnosis
            patientName.text = patient.data.theName;
            patientSex.text = patient.data.isMale ? "M" : "F";
            patientAge.text = $"{patient.data.age}";
            patientSkill.text = $"{patient.abilityData.skill}";
            patientGrowth.text = $"{patient.abilityData.growth}";
            patientAppearanceWidget.FormAppearance(patient.appearanceData);

            if (patient.abilityData.status == Data.AbilityData.Status.InICU)
            {
                alreadyEmergencyObj.gameObject.SetActive(true);
                emergencyDisabledObj.gameObject.SetActive(false);
            }
            else
            {
                alreadyEmergencyObj.gameObject.SetActive(false);
                emergencyDisabledObj.gameObject.SetActive(false);
            }

            noDiagnosisBtnClickedYet = true;
            signatureDisabledObj.gameObject.SetActive(true);
            signatureHintObj.gameObject.SetActive(false);
            signatureRect.gameObject.SetActive(false);
            sentToEmergencyObj.gameObject.SetActive(false);

            GenerateSymptoms(patient);
            ResetDiagnosisChoice();
        }
        public void ConfirmDiagnosis(bool diagnosisResult)
        {
            //TODO: sent to icu patints, triggers different reply

            owner.PrescriptionDisposed();

            if (diagnosisResult)
            {
                if (owner.CurrentPatient.abilityData.status == Data.AbilityData.Status.InICU)
                    owner.CurrentPatient.abilityData.status = Data.AbilityData.Status.Onduty;

                owner.CurrentPatient.DiagnoseCurrentDisease();
            }
            else if (selectedDiagnosisId == Guid.Empty)
            {
                //
            }
            else
            {
                owner.CurrentPatient.DiagnoseCurrentDisease();

                GameController.instance.runner.eventHandler.CreateEvent(Data.EventConfigData.EventType.PatientDied, new PatientDied(owner.CurrentPatient));
                GameController.instance.KillAPatient(owner.CurrentPatient);
            }
        }
        #endregion

        #region Private Functions
        private void OnDiseaseUpdated(Disease disease)
        {
            if (!disease.data.isKnown) return;

            var newDiagnosisChunk = Instantiate(diagnosisBtnTemplate, diagnosisBtnParent, false);
            newDiagnosisChunk.Init(disease, OnDiagnosisBtnClicked);
            newDiagnosisChunk.gameObject.SetActive(true);

            currentDiagnosisBtns.Add(newDiagnosisChunk);
        }
        private void OnDiagnosisBtnClicked(Guid selectedDiseaseId)
        {
            noDiagnosisBtnClickedYet = false;

            foreach (var btn in currentDiagnosisBtns)
            {
                btn.ResetBtnSelection();
            }
            this.selectedDiagnosisId = selectedDiseaseId;

            if (selectedDiagnosisId == Guid.Empty)
            {
                if (alreadyEmergencyObj.activeSelf)
                {
                    signatureDisabledObj.gameObject.SetActive(true);
                    signatureHintObj.gameObject.SetActive(false);
                }
                else
                {
                    if (sentToEmergencyObj.activeSelf)
                    {
                        signatureDisabledObj.gameObject.SetActive(false);
                        signatureHintObj.gameObject.SetActive(true);
                    }
                    else
                    {
                        signatureDisabledObj.gameObject.SetActive(true);
                        signatureHintObj.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (alreadyEmergencyObj.activeSelf)
                {
                    signatureDisabledObj.gameObject.SetActive(false);
                    signatureHintObj.gameObject.SetActive(true);
                }
                else
                {
                    if (sentToEmergencyObj.activeSelf)
                    {
                        signatureDisabledObj.gameObject.SetActive(true);
                        signatureHintObj.gameObject.SetActive(false);
                    }
                    else
                    {
                        signatureDisabledObj.gameObject.SetActive(false);
                        signatureHintObj.gameObject.SetActive(true);
                    }
                }
            }
        }
        private void ResetDiagnosisChoice()
        {
            foreach (var btn in currentDiagnosisBtns)
            {
                btn.ResetBtnSelection();
            }
        }
        private void GenerateSymptoms(Patient patient)
        {
            foreach (var chunk in currentSymptomChunks)
            {
                Destroy(chunk.gameObject);
            }
            currentSymptomChunks.Clear();

            foreach (var symptom in patient.GetCurrentDisease().data.symptoms)
            {
                var newSymptomChunk = Instantiate(symptomChunkTemplate, symptomChunkParent, false);
                newSymptomChunk.GetComponentInChildren<TMP_Text>().text = symptom;
                newSymptomChunk.gameObject.SetActive(true);

                currentSymptomChunks.Add(newSymptomChunk);
            }
        }
        private void FadeChannelContent(bool set)
        {
            var endValue = set ? 1 : 0;
            prescriptionChannel.DOKill();
            prescriptionChannel.DOFade(endValue, 0.5f);
        }
        private void FireDiagnosis(Patient patient)
        {

        }
        private void SwitchPos(bool dock)
        {
            bookRect.DOKill();
            isDocking = dock;

            var distanceX = Mathf.Abs(popPos.x - normalPos.x);
            var distanceY = Mathf.Abs(popPos.y - normalPos.y);

            var speedRec = switchSpeed;

            //interactable


            if (dock)
            {
                bookRect.DOAnchorPosX(BookPos.x, distanceX * speedRec).SetEase(Ease.OutCubic).OnComplete(() =>
                    bookRect.DOAnchorPosY(BookPos.y, distanceY * speedRec).SetEase(Ease.OutCubic));

                FadeChannelContent(false);
            }
            else
            {
                bookRect.DOAnchorPosY(BookPos.y, distanceY * speedRec).SetEase(Ease.OutCubic).OnComplete(() =>
                    bookRect.DOAnchorPosX(BookPos.x, distanceX * speedRec).SetEase(Ease.OutCubic));

                FadeChannelContent(true);
            }

        }
        #endregion

        #region Debug Functions
        private void Cheat_SwitchNotePos(ACheatData data)
        {
            SwitchPos(!isDocking);
        }
        #endregion
    }
}
