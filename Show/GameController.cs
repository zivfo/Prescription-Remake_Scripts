using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using RefinedGame.Data;
using RefinedGame.Logic;
using RefinedGame.Tool.DebugUtil;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Show
{
    public class GameController : MonoBehaviour
    {
        //TODO: see if an open runner can cause problems
        public GameRunner runner;
        public PatientVisualController patientVisualController = null;

        [SerializeField] PreparationWindow preparationWindow = null;
        [SerializeField] StudyWindow studyWindow = null;
        [SerializeField] ClinicWindow clinicWindow = null;
        [SerializeField] SituationWindow situationWindow = null;
        [SerializeField] EndingWindow endingWindow = null;
        [SerializeField] MedsBookPanel medsBookPanel = null;
        [SerializeField] Image stageCurtain = null;

        public static GameController instance;


        #region Unity Functions
        //Execute first right after PlatformConfigs.Init()
        private void Awake()
        {
            if (instance == null)
                instance = this;

            stageCurtain.gameObject.SetActive(true);
            stageCurtain.raycastTarget = true;

            preparationWindow.gameObject.SetActive(false);
            clinicWindow.gameObject.SetActive(false);
            situationWindow.gameObject.SetActive(false);
            endingWindow.gameObject.SetActive(false);
            studyWindow.gameObject.SetActive(false);

            runner = new GameRunner();
            runner.InitLogic();
        }
        private void Start()
        {
            InitShow();
            runner.ToPreparation();

            for (int i = 0; i < 2; i++)
            {
                CallOnePatient();
            }
        }
        private void Update()
        {
            runner.Update(Time.deltaTime);
        }
        private void OnDestroy()
        {
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Data_Add100Money, Cheat_Add100Money);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Data_Remove100Money, Cheat_Remove100Money);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Data_Add10Credit, Cheat_Add10Credit);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Data_Remove10Credit, Cheat_Remove10Credit);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Game_CallAPatient, Cheat_CallAPatient);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Game_SwitchPhases, Cheat_SwitchPhases);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Game_GenerateADisease, Cheat_GenerateADisease);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Game_ClearAllPatients, Cheat_ClearAllPatients);
            DebugEventsController.GetInstance().UnSubscribe(CheatType.Game_SendAllPatientsToIcu, Cheat_SendAllPatientsToIcu);
        }
        #endregion

        #region Public Functions
        public void HandlePlayerRewards(RewardData rewardData)
        {
            if (rewardData == null)
                return;

            foreach (var aReward in rewardData.rewards)
            {
                switch (aReward.Key)
                {
                    case RewardData.RewardType.Money:
                        Player.Money += aReward.Value;
                        break;
                    case RewardData.RewardType.Credit:
                        Player.Credits += aReward.Value;
                        break;
                }
            }
        }
        public void CallOnePatient()
        {
            var appearanceId = this.patientVisualController.GenerateAppearance();
            var patient = runner.patientHandler.CallNewPatient(appearanceId);
            Player.PatientSlot++;
            Player.HandleHiringCost();

            //TODO: see if it's right to do it here
            patientVisualController.ClearUsedAppearances();
        }
        public void LogForMe(string log)
        {
            Debug_Log(log);
        }
        public void CloseMedsBook()
        {
            medsBookPanel.CloseBookCover();
        }
        public void KillAPatient(Patient patient)
        {
            patientVisualController.RemoveAppearance(runner.patientHandler.KillAPatient(patient));
        }
        #endregion

        #region Private Functions
        private void InitShow()
        {
            clinicWindow.Init();
            situationWindow.Init();
            endingWindow.Init();
            studyWindow.Init();
            preparationWindow.Init();
            patientVisualController.Init();
            medsBookPanel.Init();
            InitSelf();
        }
        private void InitSelf()
        {
            runner.levelHandler.RegisterToPhaseUpdates(OnPhaseChanged);

            DebugEventsController.GetInstance().Subscribe(CheatType.Data_Add100Money, Cheat_Add100Money);
            DebugEventsController.GetInstance().Subscribe(CheatType.Data_Remove100Money, Cheat_Remove100Money);
            DebugEventsController.GetInstance().Subscribe(CheatType.Data_Add10Credit, Cheat_Add10Credit);
            DebugEventsController.GetInstance().Subscribe(CheatType.Data_Remove10Credit, Cheat_Remove10Credit);
            DebugEventsController.GetInstance().Subscribe(CheatType.Game_CallAPatient, Cheat_CallAPatient);
            DebugEventsController.GetInstance().Subscribe(CheatType.Game_SwitchPhases, Cheat_SwitchPhases);
            DebugEventsController.GetInstance().Subscribe(CheatType.Game_GenerateADisease, Cheat_GenerateADisease);
            DebugEventsController.GetInstance().Subscribe(CheatType.Game_ClearAllPatients, Cheat_ClearAllPatients);
            DebugEventsController.GetInstance().Subscribe(CheatType.Game_SendAllPatientsToIcu, Cheat_SendAllPatientsToIcu);
        }
        private void OnPhaseChanged(LevelPhase newPhase)
        {
            medsBookPanel.OnPhaseChanged(newPhase.phase);
            PromoteWindowLayer(true);

            switch (newPhase.phase)
            {
                case LevelHandler.Phase.Preparing:
                    ToStage(preparationWindow);
                    break;
                case LevelHandler.Phase.OnDuty:
                    PromoteWindowLayer(false);
                    ToStage(clinicWindow);
                    break;
                case LevelHandler.Phase.Studying:
                    ToStage(studyWindow);
                    break;
                case LevelHandler.Phase.Ending:
                    ToStage(endingWindow);
                    break;
                case LevelHandler.Phase.Managing:
                    ToStage(situationWindow);
                    break;
            }
        }
        private void ToStage(AUiWindow nextStage)
        {
            stageCurtain.raycastTarget = true;

            stageCurtain.DOKill();
            var sequence = DOTween.Sequence();
            sequence.Append(stageCurtain.DOFade(1, 0.3f))
                    .AppendCallback(() =>
                    {
                        preparationWindow.gameObject.SetActive(false);
                        situationWindow.gameObject.SetActive(false);
                        endingWindow.gameObject.SetActive(false);
                        clinicWindow.gameObject.SetActive(false);
                        studyWindow.gameObject.SetActive(false);

                        nextStage.gameObject.SetActive(true);
                    })
                    .AppendInterval(0.1f)
                    .Append(stageCurtain.DOFade(0, 0.3f).OnComplete(() => stageCurtain.raycastTarget = false));
        }
        private void PromoteWindowLayer(bool reverse)
        {
            if (reverse)
            {
                stageCurtain.transform.SetSiblingIndex(transform.childCount - 2);
            }
            else
            {
                stageCurtain.transform.SetSiblingIndex(3);
            }
        }
        #endregion

        #region Debug Functions
        private void Debug_Log(string msg)
        {
            if (!PlatformConfigs.DebugEnabled) return;
            UnityEngine.Debug.Log("[GameController]: " + msg);
        }
        private void Debug_LogWarning(string msg)
        {
            if (!PlatformConfigs.DebugEnabled) return;
            UnityEngine.Debug.LogWarning("[GameController]: " + msg);
        }
        private void Debug_LogError(string msg)
        {
            UnityEngine.Debug.LogError("[GameController]: " + msg);
        }
        private void Cheat_Add100Money(ACheatData data)
        {
            Player.Money += 100;
        }
        private void Cheat_Remove100Money(ACheatData data)
        {
            Player.Money -= 100;
        }
        private void Cheat_TestLogError(ACheatData data)
        {
            Debug_LogError("ErrorTest");
        }
        private void Cheat_Add10Credit(ACheatData data)
        {
            Player.Credits += 10;
        }
        private void Cheat_Remove10Credit(ACheatData data)
        {
            Player.Credits -= 10;
        }
        private void Cheat_CallAPatient(ACheatData data)
        {
            var appearanceId = this.patientVisualController.GenerateAppearance();
            var patient = runner.Debug_CallAnIllPatient(appearanceId);


            Debug_Log(patient.data.theName);
            Debug_Log($"{patient.data.age}");
            Debug_Log($"{patient.data.isMale}");
            Debug_Log(patient.data.occupation);
            foreach (var sentence in patient.FormSpeech())
            {
                Debug_Log(sentence);
            }
        }
        private void Cheat_SwitchPhases(ACheatData data)
        {
            if (runner.levelHandler.CurrentPhase == LevelHandler.Phase.Preparing)
                runner.levelHandler.SetPhase(LevelHandler.Phase.OnDuty);
            else
                runner.levelHandler.SetPhase(LevelHandler.Phase.Preparing);
        }
        private void Cheat_GenerateADisease(ACheatData data)
        {
            Debug_Log(runner.Debug_GenerateADisease(1000));
        }
        private void Cheat_ClearAllPatients(ACheatData data)
        {
            patientVisualController.RemoveAllAppearances(runner.patientHandler.Debug_ClearAllPatient());
        }
        private void Cheat_SendAllPatientsToIcu(ACheatData data)
        {
            foreach (var patient in runner.patientHandler.livingPatients)
            {
                patient.abilityData.status = AbilityData.Status.InICU;
            }
        }
        #endregion
    }
}
