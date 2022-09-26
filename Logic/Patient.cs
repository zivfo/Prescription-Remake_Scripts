using System.Collections.Generic;

namespace RefinedGame.Logic
{
    using System;
    using System.Text;
    using RefinedGame.Data;

    public class DiagnosisData
    {
        public bool isDiagnosed = false;
        public Disease disease = null;
        public bool isCurrent = true;

        public DiagnosisData(Disease disease)
        {
            this.disease = disease;
        }
    }
    public class Patient
    {
        public PatientData data;
        public System.Guid appearanceData;
        public bool inClinic = false;
        public RewardData rewardData = new RewardData();
        public AbilityData abilityData = new AbilityData();
        public List<DiagnosisData> history = new List<DiagnosisData>();

        public Patient(PatientData data)
        {
            this.data = data;
        }

        public List<string> GetSpeechSentence(int speechType)
        {
            List<string> speech = new List<string>();
            foreach (var aSpeech in data.sentences)
            {
                if (aSpeech.Key == (PatientConfigData.SpeechType)speechType)
                {
                    speech.Add(new LocalizableString(aSpeech.Value[MyRandom.Range(aSpeech.Value.Count)]).ToString());
                }
                else
                {
                    continue;
                }
            }

            return speech;
        }
        public List<string> FormSpeech()
        {
            List<string> speech = new List<string>();

            foreach (var aSpeech in data.sentences)
            {
                if (aSpeech.Key == PatientConfigData.SpeechType.Rejoin
                    || aSpeech.Key == PatientConfigData.SpeechType.Reply)
                    continue;

                if (aSpeech.Key == PatientConfigData.SpeechType.Detail)
                {
                    StringBuilder sb = new StringBuilder();

                    //TODO: currently no need to check null, see if it changes
                    Disease toCureDisease = GetCurrentDisease();
                    if (toCureDisease == null)
                        continue;

                    for (int i = 0; i < toCureDisease.data.symptoms.Count; i++)
                    {
                        sb.Append(new LocalizableString(toCureDisease.data.symptoms[i]));
                        if (i < toCureDisease.data.symptoms.Count - 1)
                            sb.Append(", ");
                    }
                    speech.Add(new LocalizableString(aSpeech.Value[MyRandom.Range(aSpeech.Value.Count)]) + " " + sb.ToString());
                }
                else
                    speech.Add(new LocalizableString(aSpeech.Value[MyRandom.Range(aSpeech.Value.Count)]));
            }
            return speech;
        }
        public Disease GetCurrentDisease()
        {
            return history.Find(x => x.isCurrent == true).disease;
        }
        public void DiagnoseCurrentDisease()
        {
            var current = history.Find(x => x.isCurrent == true);
            current.isDiagnosed = true;
            current.isCurrent = false;
        }
        public void RewardMultipler(float times, RewardData.RewardType rewardType = RewardData.RewardType.None)
        {
            if (rewardType == RewardData.RewardType.None)
            {
                Dictionary<RewardData.RewardType, int> newRewards = new Dictionary<RewardData.RewardType, int>();

                foreach (var key in rewardData.rewards.Keys)
                {
                    newRewards.Add(rewardType, (int)Math.Truncate(rewardData.rewards[key] * times));
                }

                rewardData.rewards = newRewards;
            }
            else
            {
                rewardData.rewards[rewardType] = (int)Math.Truncate(rewardData.rewards[rewardType] * times);
            }
        }
    }
}


