using System;
using System.Threading.Tasks;


namespace RefinedGame.Logic
{
    public class ClinicHandler
    {
        public enum ClinicStatus
        {
            available, busy
        }
        public Action<ClinicStatus> onPhaseChanged;

        private ClinicStatus currentStatus;
        private GameRunner owner;
        public int CurrentStep { get; private set; }

        public ClinicHandler(GameRunner owner)
        {
            this.owner = owner;
        }

        #region Public Functions
        public void ToNextStep(bool firstStep = false)
        {
            if (firstStep)
                ToStep(1);
            else
            {
                CurrentStep++;
                ToStep(CurrentStep);
            }
        }
        public void RegisterToPhaseUpdates(Action<ClinicStatus> callBack)
        {
            onPhaseChanged += callBack;
        }
        public void UnRegisterToPhaseUpdates(Action<ClinicStatus> callBack)
        {
            onPhaseChanged -= callBack;
        }
        public void SetClinicStatus(ClinicStatus status)
        {
            currentStatus = status;

            if (onPhaseChanged != null)
                onPhaseChanged(currentStatus);
        }
        #endregion

        #region Private Functions
        private void ToStep(int step)
        {
            CurrentStep = step;

            switch (step)
            {
                case 1:
                    owner.eventHandler.AddEvents(Data.EventConfigData.EventType.Progression);
                    owner.eventHandler.SendLetters(true);
                    break;
                case 2:
                    owner.InfectAll();
                    break;
                case 3:
                    //TODO: don't make known directly
                    owner.eventHandler.CreateEvent(Data.EventConfigData.EventType.LevelRewards,
                        new LevelRewards(owner.patientHandler.livingPatients.Count, owner.diseaseHandler.GetKnownDiseaseCount(), 1));
                    owner.diseaseHandler.GenerateSomeUnknownDiseases(owner.levelHandler.GetCurrentLevelData(), true);
                    owner.eventHandler.SendLetters(false);
                    break;
                case 4:
                    ExitClinic();
                    break;
            }
        }
        private async void ExitClinic()
        {
            await Task.Delay(1500);

            CurrentStep = 0;
            owner.eventHandler.ResolveEvents();
            owner.LevelSettlement();

            if (owner.progressHandler.GetData(Data.ProgressData.Type.PlayerProgress) <= 0)
            {
                owner.ToEnd();
            }
            else
            {
                owner.ToPreparation();
            }
        }

        #endregion
    }
}