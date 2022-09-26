using System;
using System.Collections;
using RefinedGame.Data;


namespace RefinedGame.Logic
{
    public class LevelPhase
    {
        public int phaseLevel = 0;
        public LevelHandler.Phase phase;

        public LevelPhase(int phaseLevel, LevelHandler.Phase phase)
        {
            this.phaseLevel = phaseLevel;
            this.phase = phase;
        }
    }
    public class LevelHandler
    {
        public int currentLevel = 0;
        public Phase CurrentPhase { get; private set; }
        public LevelData CurrentLevelData { get; private set; }

        Action<LevelPhase> onPhaseChanged;
        LevelConfigData configData = new LevelConfigData();

        public enum Phase
        {
            Preparing, OnDuty, Studying, Ending, Managing
        }


        #region Public Functions
        public void RegisterToPhaseUpdates(Action<LevelPhase> callBack)
        {
            onPhaseChanged += callBack;
        }
        public void UnRegisterToPhaseUpdates(Action<LevelPhase> callBack)
        {
            onPhaseChanged -= callBack;
        }
        public LevelData GetCurrentLevelData()
        {
            var newData = FormCurrentLevelData();

            return newData;
        }
        public void SetPhase(Phase phase)
        {
            CurrentPhase = phase;

            if (onPhaseChanged != null)
                onPhaseChanged(new LevelPhase(currentLevel, CurrentPhase));
        }
        #endregion

        #region Private Functions
        private LevelData FormCurrentLevelData()
        {
            LevelData newLevelData = new LevelData();

            if (configData.levelPatientConfigChart.TryGetValue(currentLevel, out newLevelData.forcedNewDiseases))
            {
                newLevelData.level = currentLevel;
            }
            else
            {
                //TODO:perhaps proceed to game over
            }

            return newLevelData;
        }
        #endregion
    }
}
