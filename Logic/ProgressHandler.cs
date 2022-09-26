using System;
using System.Collections.Generic;
using RefinedGame.Data;


namespace RefinedGame.Logic
{
    public class ProgressHandler
    {
        ProgressData progressData = new ProgressData();
        ProgressConfigData configData = new ProgressConfigData();

        GameRunner owner;
        Action<ProgressData.Type, int, int> onProgressValueUpdates;

        public int ProgressLength
        {
            get { return configData.progressLength; }
        }
        public ProgressHandler(GameRunner owner)
        {
            this.owner = owner;
        }

        public int GetData(ProgressData.Type type)
        {
            return progressData.allData[type];
        }
        public void SetData(ProgressData.Type type, int value)
        {
            var oldValue = progressData.allData[type];
            progressData.allData[type] = value;

            if (onProgressValueUpdates != null)
                onProgressValueUpdates(type, value, value - oldValue);
        }

        public void RegisterToProgressUpdates(Action<ProgressData.Type, int, int> callback)
        {
            onProgressValueUpdates += callback;
        }
        public void UnRegisterToProgressUpdates(Action<ProgressData.Type, int, int> callback)
        {
            onProgressValueUpdates -= callback;
        }

        public void InitProgress()
        {
            SetData(ProgressData.Type.PlayerProgress, configData.initPlayerProgress);
            SetData(ProgressData.Type.WorldSpeed, configData.initWorldSpeed);
            SetData(ProgressData.Type.WorldAcceleration, configData.initWorldAcceleration);
            SetData(ProgressData.Type.PlayerSpeed, configData.initPlayerSpeed);
            SetData(ProgressData.Type.PlayerAcceleration, configData.initPlayerAcceleration);
        }

        public void CalculateSpeed(List<Patient> currentPatients)
        {
            int playerSpeedNext = 0;
            int playerAccelerationNext = 0;

            foreach (var patient in currentPatients)
            {
                if (patient.abilityData.status == AbilityData.Status.Onduty)
                {
                    playerSpeedNext += patient.abilityData.skill;
                    playerAccelerationNext += patient.abilityData.growth;
                }
            }

            SetData(ProgressData.Type.PlayerSpeed, playerSpeedNext);
            SetData(ProgressData.Type.PlayerAcceleration, playerAccelerationNext);
        }
        public void CalculateProgress()
        {
            SetData(ProgressData.Type.PlayerProgress, GetData(ProgressData.Type.PlayerProgress) + GetData(ProgressData.Type.PlayerSpeed) - GetData(ProgressData.Type.WorldSpeed));
            SetData(ProgressData.Type.WorldSpeed, GetData(ProgressData.Type.WorldSpeed) + GetData(ProgressData.Type.WorldAcceleration));
        }
        public void CreateEventUponProgressPercentage()
        {
            int currentPercentageX100 = GetCurrentPercentageX100();
        }
        public int GetCurrentPercentageX100()
        {
            return (int)Math.Truncate((double)GetData(ProgressData.Type.PlayerProgress) / GetData(ProgressData.Type.WorldSpeed) * 100);
        }
    }
}