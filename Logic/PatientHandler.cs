using System.Collections;
using System.Collections.Generic;


namespace RefinedGame.Logic
{
    using System;
    using System.Text;
    using RefinedGame.Data;


    public class PatientHandler
    {
        public List<Patient> livingPatients = new List<Patient>();
        public List<Patient> deadPatients = new List<Patient>();

        PatientConfigData configData = new PatientConfigData();
        AbilityConfigData abilityConfigData = new AbilityConfigData();
        TalentConfigData talentConfigData = new TalentConfigData();

        Action<Patient, bool> onPatientUpdated;
        Action<Patient> onPatientLeveledUp;
        Action<List<Patient>> onPatientsInfected;
        GameRunner owner;

        public PatientHandler(GameRunner owner)
        {
            this.owner = owner;
        }

        #region Public Functions
        public void RegisterToPatientInfected(Action<List<Patient>> callBack) { onPatientsInfected += callBack; }
        public void UnRegisterToPatientInfected(Action<List<Patient>> callBack) { onPatientsInfected -= callBack; }
        public void RegisterToPatientLevelUp(Action<Patient> callBack) { onPatientLeveledUp += callBack; }
        public void UnRegisterToPatientLevelUp(Action<Patient> callBack) { onPatientLeveledUp -= callBack; }
        public void RegisterToPatientUpdates(Action<Patient, bool> callBack) { onPatientUpdated += callBack; }
        public void UnRegisterToPatientUpdates(Action<Patient, bool> callBack) { onPatientUpdated -= callBack; }

        public void CatchADisease(Patient patient, Disease disease)
        {
            patient.history.Add(new DiagnosisData(disease));
        }
        public void InfectAll(List<Disease> availableDiseases)
        {
            List<Patient> theInfected = new List<Patient>();
            foreach (var patient in livingPatients)
            {
                if (patient.abilityData.status == AbilityData.Status.Onduty)
                {
                    var theDisease = availableDiseases[MyRandom.Range(availableDiseases.Count)];

                    if (MyRandom.Chance(patient.abilityData.immunityX10))
                        owner.eventHandler.CreateEvent(EventConfigData.EventType.InfectionImmuned, new InfectionImmuned(patient));
                    else
                    {
                        CatchADisease(patient, theDisease);
                        theInfected.Add(patient);
                    }
                }
                else if (patient.abilityData.status == AbilityData.Status.InICU)
                {
                    theInfected.Add(patient);
                }
            }
            if (onPatientsInfected != null)
                onPatientsInfected(theInfected);
        }
        public Patient CallNewPatient(Guid appearanceId)
        {
            Patient newPaitent = new Patient(new PatientData());
            newPaitent.data.id = System.Guid.NewGuid();

            newPaitent.data.isMale = MyRandom.Range(0, 2) == 0 ? true : false;

            var theName = "";
            bool noDuplicates = true;
            int count = 0;
            do
            {
                count++;
                theName = newPaitent.data.isMale ? configData.manNames[MyRandom.Range(0, configData.manNames.Count)] : configData.womanNames[MyRandom.Range(0, configData.womanNames.Count)];
                foreach (var patient in livingPatients)
                {
                    if (theName != patient.data.theName)
                        continue;
                    else
                        noDuplicates = false;
                }
            } while (!noDuplicates && count < 100);

            newPaitent.data.theName = new LocalizableString(theName);
            newPaitent.data.age = MyRandom.Range(18, 100);

            List<string> occupations = new List<string>();
            newPaitent.data.category = (PatientConfigData.Category)MyRandom.Range(0, 4);
            if (configData.occupations.TryGetValue(newPaitent.data.category, out occupations))
            {
                string occupation = occupations[MyRandom.Range(0, occupations.Count)];
                newPaitent.data.occupation = new LocalizableString("Occupation_A") + new LocalizableString(occupation);
            }

            newPaitent.data.characteristic = MyRandom.Range(-1, 2);
            if (newPaitent.data.category == PatientConfigData.Category.rogue)
                newPaitent.data.characteristic = -1;

            if (newPaitent.data.characteristic == -1)
            {
                List<string> greeting = new List<string>();
                List<string> rejoin = new List<string>();
                List<string> detail = new List<string>();
                List<string> tone = new List<string>();
                List<string> reply = new List<string>();
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.badGreeting, out greeting)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.badRejoin, out rejoin)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.symptomDetail, out detail)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.badTone, out tone)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.badReply, out reply)) { }
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Greeting, greeting);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Rejoin, rejoin);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Detail, detail);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Tone, tone);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Reply, reply);
            }
            else if (newPaitent.data.characteristic == 0)
            {
                List<string> greeting = new List<string>();
                List<string> rejoin = new List<string>();
                List<string> detail = new List<string>();
                List<string> tone = new List<string>();
                List<string> reply = new List<string>();
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.niceGreeting, out greeting)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.niceRejoin, out rejoin)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.symptomDetail, out detail)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.neutralTone, out tone)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.niceReply, out reply)) { }
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Greeting, greeting);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Rejoin, rejoin);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Detail, detail);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Tone, tone);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Reply, reply);
            }
            else if (newPaitent.data.characteristic == 1)
            {
                List<string> greeting = new List<string>();
                List<string> rejoin = new List<string>();
                List<string> detail = new List<string>();
                List<string> tone = new List<string>();
                List<string> reply = new List<string>();
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.niceGreeting, out greeting)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.niceRejoin, out rejoin)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.symptomDetail, out detail)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.niceTone, out tone)) { }
                if (configData.speeches.TryGetValue(PatientConfigData.DetailedSpeechType.niceReply, out reply)) { }
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Greeting, greeting);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Rejoin, rejoin);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Detail, detail);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Tone, tone);
                newPaitent.data.sentences.Add(PatientConfigData.SpeechType.Reply, reply);
            }

            newPaitent.appearanceData = appearanceId;
            FormAbility(newPaitent);

            livingPatients.Add(newPaitent);
            owner.progressHandler.CalculateSpeed(livingPatients);

            if (onPatientUpdated != null)
                onPatientUpdated(newPaitent, true);
            return newPaitent;
        }


        internal void LivingPatientsSkillGrow()
        {
            foreach (var patient in livingPatients)
            {
                if (patient.abilityData.status == AbilityData.Status.Onduty)
                    patient.abilityData.skill += patient.abilityData.growth;

                if (onPatientLeveledUp != null)
                    onPatientLeveledUp(patient);
            }
            owner.progressHandler.CalculateSpeed(livingPatients);
        }

        public void LevelUpPatient(Patient patient)
        {
            patient.abilityData.growth++;
            Player.Money -= patient.abilityData.lvlUpCost;

            patient.abilityData.lvlUpCost += abilityConfigData.lvlUpCostIncrement;

            owner.progressHandler.CalculateSpeed(livingPatients);
        }
        public void SwitchPatientStatus(Patient patient)
        {
            if (patient.abilityData.status != AbilityData.Status.InICU)
            {
                patient.abilityData.status = patient.abilityData.status == AbilityData.Status.Onduty ? AbilityData.Status.Offduty : AbilityData.Status.Onduty;
                owner.progressHandler.CalculateSpeed(livingPatients);
            }
        }
        public Guid KillAPatient(Patient patient)
        {
            livingPatients.Remove(patient);
            deadPatients.Add(patient);
            Player.PatientSlot--;

            if (onPatientUpdated != null)
                onPatientUpdated(patient, false);

            owner.progressHandler.CalculateSpeed(livingPatients);

            return patient.appearanceData;
        }
        #endregion

        #region Private Functions
        private void FormAbility(Patient patient, int forcedLevel = -1)
        {
            var level = -1;
            TalentData talentData = new TalentData();

            if (forcedLevel >= 0 && forcedLevel < 4)
            {
                level = forcedLevel;
                var allMatchingTalents = talentConfigData.allTalents.FindAll(x => x.level == level);
                talentData = allMatchingTalents[MyRandom.Range(0, allMatchingTalents.Count)];
            }
            else
            {
                var roll = MyRandom.Range(1, 101);
                if (roll <= abilityConfigData.level3Rate)
                    level = 3;
                else if (roll > abilityConfigData.level3Rate && roll <= abilityConfigData.level2Rate)
                    level = 2;
                else if (roll > abilityConfigData.level2Rate && roll <= abilityConfigData.level1Rate)
                    level = 1;
                else
                    level = 0;

                var allMatchingTalents = talentConfigData.allTalents.FindAll(x => x.level == level);
                talentData = allMatchingTalents[MyRandom.Range(0, allMatchingTalents.Count)];
            }
            patient.abilityData.talentData.chanceX10 = talentData.chanceX10;
            patient.abilityData.talentData.content = talentData.content;
            patient.abilityData.talentData.level = talentData.level;
            patient.abilityData.talentData.type = talentData.type;
            patient.abilityData.talentData.chanceX10 = MyRandom.Range(talentConfigData.minRateInclusive, talentConfigData.maxRateExclusive);

            patient.abilityData.level = level;
            int skillLevelMultiper = level == 0 ? 1 : level;
            patient.abilityData.skill = MyRandom.Range(abilityConfigData.minInitSkillBasis, abilityConfigData.maxInitSkillBasisExclusive) * skillLevelMultiper;

            switch (patient.abilityData.talentData.type)
            {
                case TalentData.TalentType.ExtraGrowth:
                    patient.abilityData.talentData.content = $"({patient.abilityData.talentData.chanceX10}) {patient.abilityData.talentData.content}";
                    patient.abilityData.growth = abilityConfigData.initGrowth + patient.abilityData.talentData.chanceX10;
                    patient.abilityData.lvlUpCost = abilityConfigData.lvlUpCost;
                    patient.abilityData.immunityX10 = abilityConfigData.initImmunity;
                    break;
                case TalentData.TalentType.Immunity:
                    patient.abilityData.talentData.content = $"({patient.abilityData.talentData.chanceX10 * 0.1}) {patient.abilityData.talentData.content}";
                    patient.abilityData.growth = abilityConfigData.initGrowth;
                    patient.abilityData.lvlUpCost = abilityConfigData.lvlUpCost;
                    patient.abilityData.immunityX10 = abilityConfigData.initImmunity + patient.abilityData.talentData.chanceX10;
                    break;
                case TalentData.TalentType.LessCost:
                    patient.abilityData.talentData.content = $"({patient.abilityData.talentData.chanceX10 * 0.1}) {patient.abilityData.talentData.content}";
                    patient.abilityData.growth = abilityConfigData.initGrowth;
                    patient.abilityData.lvlUpCost = abilityConfigData.lvlUpCost * patient.abilityData.talentData.chanceX10 / 10;
                    patient.abilityData.immunityX10 = abilityConfigData.initImmunity;
                    break;
                case TalentData.TalentType.None:
                    patient.abilityData.talentData.content = "-";
                    patient.abilityData.growth = abilityConfigData.initGrowth;
                    patient.abilityData.lvlUpCost = abilityConfigData.lvlUpCost;
                    patient.abilityData.immunityX10 = abilityConfigData.initImmunity;
                    break;
            }
        }

        public List<Guid> Debug_ClearAllPatient()
        {
            List<Guid> clearedAppearanceIds = new List<Guid>();
            foreach (var patient in livingPatients)
            {
                deadPatients.Add(patient);
                clearedAppearanceIds.Add(patient.appearanceData);
                Player.PatientSlot--;

                if (onPatientUpdated != null)
                    onPatientUpdated(patient, false);
            }
            livingPatients.Clear();
            owner.progressHandler.CalculateSpeed(livingPatients);

            return clearedAppearanceIds;
        }
        #endregion
    }
}


