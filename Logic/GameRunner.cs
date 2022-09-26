//for debug use only
using RefinedGame.Show;

using System;
using System.Collections;
using System.Collections.Generic;

namespace RefinedGame.Logic
{
    public static class MyRandom
    {
        static System.Random random = new System.Random();

        public static bool RollTheDice(int rateX10)
        {
            return Range(1, 11) <= rateX10;
        }
        public static int Range(int minInclusive, int maxExclusive)
        {
            return random.Next(minInclusive, maxExclusive);
        }
        public static int Range(int maxExclusive)
        {
            return random.Next(0, maxExclusive);
        }
        public static bool Chance(int chanceX10)
        {
            return chanceX10 >= random.Next(1, 11);
        }
    }
    public class GameRunner
    {
        public DiseaseHandler diseaseHandler;
        public PatientHandler patientHandler;
        public LevelHandler levelHandler;
        public ClinicHandler clinicHandler;
        public EventHandler eventHandler;
        public ProgressHandler progressHandler;

        bool firstGame = true;

        #region Public Functions
        public void InitLogic()
        {
            diseaseHandler = new DiseaseHandler(this);
            patientHandler = new PatientHandler(this);
            levelHandler = new LevelHandler();
            clinicHandler = new ClinicHandler(this);
            eventHandler = new EventHandler(this);
            progressHandler = new ProgressHandler(this);
        }
        public void ToPreparation()
        {
            //init first level
            if (firstGame)
            {
                firstGame = false;
                Player.InitPlayerData();
                progressHandler.InitProgress();
                diseaseHandler.GenerateSomeUnknownDiseases(levelHandler.GetCurrentLevelData(), true);
            }

            levelHandler.SetPhase(LevelHandler.Phase.Preparing);
        }
        public void LevelSettlement()
        {
            progressHandler.CalculateProgress();
            patientHandler.LivingPatientsSkillGrow();
        }
        public void ToStudy()
        {
            levelHandler.SetPhase(LevelHandler.Phase.Studying);
        }
        public void ToWork()
        {
            levelHandler.currentLevel++;
            levelHandler.SetPhase(LevelHandler.Phase.OnDuty);

            clinicHandler.ToNextStep(true);
        }
        public void ToEnd()
        {
            levelHandler.SetPhase(LevelHandler.Phase.Ending);
        }
        public void ToSituation()
        {
            levelHandler.SetPhase(LevelHandler.Phase.Managing);
        }
        public void Update(float deltaTime)
        {

        }
        public void InfectAll()
        {
            patientHandler.InfectAll(diseaseHandler.AllDiseases);
        }
        #endregion

        #region Private Functions
        #endregion

        #region Debug Functions
        public Patient Debug_CallAnIllPatient(Guid appearanceId)
        {
            var newPatient = patientHandler.CallNewPatient(appearanceId);
            patientHandler.CatchADisease(newPatient, diseaseHandler.Debug_GetARandomDisease());

            return newPatient;
        }
        public void Debug_Log(string log)
        {
            GameController.instance.LogForMe("[GameRunner]: " + log);
        }
        public string Debug_GenerateADisease(int level)
        {
            if (level > 999)
            {
                level = MyRandom.Range(1, 4);
                var newD = diseaseHandler.GenerateNewUnknownDisease(level);
                diseaseHandler.MakeKnown(newD);
                return diseaseHandler.Debug_DisplayDiseaseInText(newD);
            }
            else
            {
                var newD = diseaseHandler.GenerateNewUnknownDisease(level);
                diseaseHandler.MakeKnown(newD);
                return diseaseHandler.Debug_DisplayDiseaseInText(newD);
            }
        }
        #endregion
    }
}
