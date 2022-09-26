using System;
using RefinedGame.Data;

namespace RefinedGame.Logic
{
    public class Player
    {
        static PlayerData playerData;
        public static PlayerConfigData configData = new PlayerConfigData();

        public enum PlayerDataType
        {
            Money, Credits, PatientSlot, KnownDisease, UnknownDisease,
        }
        public static Action<PlayerDataType, int, int> onPlayerDataUpdated;

        #region Public Functions
        public static void InitPlayerData()
        {
            int[] initData = configData.initPlayerData;
            Money = initData[0];
            Credits = initData[1];
        }
        public static int Money
        {
            get => playerData.money; set
            {
                int amount = value - playerData.money;
                playerData.money = value;
                if (onPlayerDataUpdated != null)
                    onPlayerDataUpdated(PlayerDataType.Money, value, amount);
            }
        }
        public static bool IsHiringEnabled()
        {
            return PatientSlot < configData.maxPatientSlot && Money >= configData.hiringPrice;
        }
        public static void HandleHiringCost()
        {
            Money -= configData.hiringPrice;
        }
        public static int Credits
        {
            get => playerData.credits; set
            {
                int amount = value - playerData.credits;
                playerData.credits = value;
                if (onPlayerDataUpdated != null)
                    onPlayerDataUpdated(PlayerDataType.Credits, value, amount);
            }
        }
        public static int PatientSlot
        {
            get => playerData.patientSlot; set
            {
                int amount = value - playerData.patientSlot;
                playerData.patientSlot = value;
                if (onPlayerDataUpdated != null)
                    onPlayerDataUpdated(PlayerDataType.PatientSlot, value, amount);
            }
        }

        public static int KnownDiseaseCount
        {
            get => playerData.knownDiseaseCount; set
            {
                int amount = value - playerData.knownDiseaseCount;
                playerData.knownDiseaseCount = value;
                if (onPlayerDataUpdated != null)
                    onPlayerDataUpdated(PlayerDataType.KnownDisease, value, amount);
            }
        }
        public static int UnknownDiseaseCount
        {
            get => playerData.unknownDiseaseCount; set
            {
                int amount = value - playerData.unknownDiseaseCount;
                playerData.unknownDiseaseCount = value;
                if (onPlayerDataUpdated != null)
                    onPlayerDataUpdated(PlayerDataType.UnknownDisease, value, amount);
            }
        }
        #endregion

        #region Private Functions
        #endregion
    }
}