using System;
using System.Collections.Generic;

namespace RefinedGame.Logic
{
    using RefinedGame.Data;

    public abstract class AEventCreationData
    {
        public EventConfigData.EventType eventType;

        public AEventCreationData(EventConfigData.EventType eventType)
        {
            this.eventType = eventType;
        }
    }
    public class InfectionImmuned : AEventCreationData
    {
        public Patient patient;
        public InfectionImmuned(Patient patient) : base(EventConfigData.EventType.InfectionImmuned)
        {
            this.patient = patient;
        }
    }
    public class LevelRewards : AEventCreationData
    {
        public int memberCoeff;
        public int diseaseCoeff;
        public int titleCoeff;
        public LevelRewards(int memberCoeff, int diseaseCoeff, int titleCoeff) : base(EventConfigData.EventType.LevelRewards)
        {
            this.memberCoeff = memberCoeff;
            this.diseaseCoeff = diseaseCoeff;
            this.titleCoeff = titleCoeff;
        }
    }
    public class PatientDied : AEventCreationData
    {
        public Patient patient;
        public PatientDied(Patient patient) : base(EventConfigData.EventType.PatientDied)
        {
            this.patient = patient;
        }
    }
    public class DiseaseDiscovered : AEventCreationData
    {
        public int number;
        public DiseaseDiscovered(int number) : base(EventConfigData.EventType.DiseaseDiscovered)
        {
            this.number = number;
        }
    }

    public class EventHandler
    {
        EventConfigData configData = new EventConfigData();
        List<EventData> eventList = new List<EventData>();
        GameRunner owner;
        Action<List<EventLetterData>> onLettersDispatched;


        public void RegisterToLetterDispatch(Action<List<EventLetterData>> callBack)
        {
            onLettersDispatched += callBack;
        }
        public void UnRegisterToLetterDispatch(Action<List<EventLetterData>> callBack)
        {
            onLettersDispatched -= callBack;
        }
        public EventHandler(GameRunner owner)
        {
            this.owner = owner;
        }
        public void AddEvents(EventConfigData.EventType eventType)
        {
            switch (eventType)
            {
                case EventConfigData.EventType.Progression:
                    foreach (var anEvent in configData.allEventData)
                    {
                        foreach (var aLetter in anEvent.letters)
                        {
                            if (anEvent.specificLevel + aLetter.levelShift == owner.levelHandler.currentLevel
                                && !eventList.Contains(anEvent))
                            {
                                eventList.Add(anEvent);
                                break;
                            }
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void ResolveEvents()
        {
            List<EventData> toRemoveEvents = new List<EventData>();
            foreach (var anEvent in eventList)
            {
                if (anEvent.specificLevel == owner.levelHandler.currentLevel)
                {
                    toRemoveEvents.Add(anEvent);
                }
            }
            foreach (var anEvent in toRemoveEvents)
            {
                eventList.Remove(anEvent);
            }
        }
        public void CreateEvent(EventConfigData.EventType eventType, AEventCreationData creationData, int levelShift = 0)
        {
            switch (eventType)
            {
                case EventConfigData.EventType.InfectionImmuned:

                    var infectionImmunedData = creationData as InfectionImmuned;
                    foreach (EventData anEvent in configData.allEventData)
                    {
                        if (anEvent.eventType == eventType)
                        {
                            var newEvent = new EventData();

                            //TODO: combine creation data

                            eventList.Add(newEvent);
                        }
                    }
                    break;
                case EventConfigData.EventType.LevelRewards:

                    var levelRewardsData = creationData as LevelRewards;
                    foreach (EventData anEvent in configData.allEventData)
                    {
                        //
                        if (anEvent.eventType == eventType)
                        {
                            var newEvent = new EventData();
                            newEvent.specificLevel = owner.levelHandler.currentLevel;
                            newEvent.eventType = anEvent.eventType;

                            var newLetter = new EventLetterData();
                            newLetter.effectType = anEvent.letters[0].effectType;
                            newLetter.levelShift = anEvent.letters[0].levelShift;
                            newLetter.isPostLevel = anEvent.letters[0].isPostLevel;
                            newLetter.letterTitle = anEvent.letters[0].letterTitle;
                            foreach (var aSentence in anEvent.letters[0].txtContent)
                            {
                                newLetter.txtContent.Add(aSentence);
                            }

                            int baseAmount = anEvent.letters[0].rewardData.rewards[RewardData.RewardType.Money];
                            float memberRewards = baseAmount * (1 + levelRewardsData.memberCoeff * configData.fullMemberRewardCoeff * 0.125f);
                            float diseaseRewards = baseAmount * (1 + levelRewardsData.diseaseCoeff * configData.diseaseCoeffX10 * 0.1f);
                            float titleRewards = (10) ^ levelRewardsData.titleCoeff;

                            int finalReward = Convert.ToInt32(Math.Truncate(memberRewards + diseaseRewards + titleRewards));

                            var newRewards = new RewardData()
                            {
                                rewards = new Dictionary<RewardData.RewardType, int>
                                {
                                    {RewardData.RewardType.Money, finalReward},
                                }
                            };
                            newLetter.rewardData = newRewards;

                            newEvent.letters.Add(newLetter);
                            eventList.Add(newEvent);
                        }
                    }
                    break;
                case EventConfigData.EventType.DiseaseDiscovered:

                    var diseaseDiscoveredData = creationData as DiseaseDiscovered;
                    foreach (EventData anEvent in configData.allEventData)
                    {
                        if (anEvent.eventType == eventType)
                        {
                            var newEvent = new EventData();
                            newEvent.specificLevel = owner.levelHandler.currentLevel;
                            newEvent.eventType = anEvent.eventType;

                            var newLetter = new EventLetterData();
                            newLetter.effectType = anEvent.letters[0].effectType;
                            newLetter.levelShift = anEvent.letters[0].levelShift;
                            newLetter.isPostLevel = anEvent.letters[0].isPostLevel;
                            newLetter.letterTitle = anEvent.letters[0].letterTitle;
                            foreach (var aSentence in anEvent.letters[0].txtContent)
                            {
                                newLetter.txtContent.Add(aSentence);
                            }
                            newLetter.txtContent.Insert(0, diseaseDiscoveredData.number.ToString());
                            newEvent.letters.Add(newLetter);

                            eventList.Add(newEvent);
                        }
                    }
                    break;
                case EventConfigData.EventType.PatientDied:

                    var patientDiedData = creationData as PatientDied;
                    foreach (EventData anEvent in configData.allEventData)
                    {
                        if (anEvent.eventType == eventType)
                        {
                            var newEvent = new EventData();
                            newEvent.specificLevel = owner.levelHandler.currentLevel;
                            newEvent.eventType = anEvent.eventType;

                            var newLetter = new EventLetterData();
                            newLetter.effectType = anEvent.letters[0].effectType;
                            newLetter.levelShift = anEvent.letters[0].levelShift;
                            newLetter.isPostLevel = anEvent.letters[0].isPostLevel;
                            newLetter.letterTitle = anEvent.letters[0].letterTitle;
                            foreach (var aSentence in anEvent.letters[0].txtContent)
                            {
                                newLetter.txtContent.Add(aSentence);
                            }
                            newLetter.txtContent.Insert(0, patientDiedData.patient.data.theName);
                            newLetter.patientAppearanceId = patientDiedData.patient.appearanceData;
                            newEvent.letters.Add(newLetter);

                            eventList.Add(newEvent);
                        }
                    }
                    break;
            }
        }
        public void SendLetters(bool isPrelevel)
        {
            List<EventLetterData> letterList = new List<EventLetterData>();

            foreach (var anEvent in eventList)
            {
                foreach (var aLetter in anEvent.letters)
                {
                    if (anEvent.specificLevel + aLetter.levelShift == owner.levelHandler.currentLevel
                        && isPrelevel == !aLetter.isPostLevel)
                    {
                        letterList.Add(aLetter);
                    }
                }
            }

            if (onLettersDispatched != null)
                onLettersDispatched(letterList);
        }
    }
}
namespace RefinedGame.Data
{
    public class EventLetterData
    {
        public Guid pageAppearanceId;
        public EventConfigData.EventEffectType effectType;
        public int levelShift;
        public bool isPostLevel;

        //Contents
        public string letterTitle;
        public List<string> txtContent = new List<string>();

        //Options
        public Guid patientAppearanceId = Guid.Empty;
        public RewardData rewardData = null;
    }

    public class EventData
    {
        public int specificLevel = -1;
        public EventConfigData.EventType eventType;
        public List<EventLetterData> letters = new List<EventLetterData>();
    }

    public class EventConfigData
    {
        readonly public int fullMemberRewardCoeff = 2;
        readonly public int diseaseCoeffX10 = 3;
        readonly public int titleCoeff = 2;

        readonly public List<EventData> allEventData = new List<EventData>
        {
            new EventData{
                specificLevel = 1,
                eventType = EventType.Progression,
                letters = new List<EventLetterData>{
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = false,
                        letterTitle = "??????",
                        txtContent = new List<string>{"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????",},
                    },
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = true,
                        letterTitle = "??????",
                        txtContent = new List<string>{"????????????????????????????????????????????????????????? ???????????????????????????????????????????????????",},
                    }
            }},
            new EventData
            {
                specificLevel = 2,
                eventType = EventType.Progression,
                letters = new List<EventLetterData>{
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = -1,
                        isPostLevel = true,
                        letterTitle = "??????",
                        txtContent = new List<string>{"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????" ,}},
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = true,
                        letterTitle = "??????",
                        txtContent = new List<string>{"???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????",}},
            }},
            new EventData
            {
                specificLevel = 2,
                eventType = EventType.Progression,
                letters = new List<EventLetterData>{
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = false,
                        letterTitle = "??????",
                        txtContent = new List<string>{"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????",}},
            }},
            new EventData
            {
                specificLevel = -1,
                eventType = EventType.PatientDied,
                letters = new List<EventLetterData>{
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = true,
                        letterTitle = "??????",
                        txtContent = new List<string>{"    ???????????????????????????????????????????????????????????????????????????????????????",}},
            }},
            new EventData
            {
                specificLevel = -1,
                eventType = EventType.InfectionImmuned,
                letters = new List<EventLetterData>{
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = true,
                        letterTitle = "Good News",
                        txtContent = new List<string>{"is healthy and feels fine today "," "," "}},
            }},
            new EventData
            {
                specificLevel = -1,
                eventType = EventType.DiseaseDiscovered,
                letters = new List<EventLetterData>{
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = true,
                        letterTitle = "??????",
                        txtContent = new List<string>{"    ?????????????????????????????????????????????????????????????????????????????????????????????????????????????????? ??????????????????????????????",}},
            }},
            new EventData
            {
                specificLevel = -1,
                eventType = EventType.LevelRewards,
                letters = new List<EventLetterData>{
                    new EventLetterData{
                        effectType = EventEffectType.NoEffect,
                        levelShift = 0,
                        isPostLevel = true,
                        letterTitle = "??????",
                        txtContent = new List<string>{"???????????????????????????????????????????????????????????????????????????????????????????????????????????? ??? ???????????? ???????????????",},
                        rewardData = new RewardData(){
                            rewards = new Dictionary<RewardData.RewardType, int>{
                                {RewardData.RewardType.Money, 10},
                            }
                        },
                    }
            }},
        };



        public enum EventType
        {
            Progression,
            PatientDied,
            InfectionImmuned,
            DiseaseDiscovered,
            LevelRewards,
        }
        public enum EventEffectType
        {
            NoEffect,
            SpecialPatients,
            ImportantPatients,
            LockBook,
            UnlockBook,
            CurrencyMultipler,
            ContainedComplains,
            OutBreak,
            PatientDied,
            InfectionImmuned,
        }
    }
}