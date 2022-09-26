using System.Collections.Generic;

namespace RefinedGame.Logic
{
    using System;
    using System.Text;
    using RefinedGame.Data;

    public class DiseaseHandler
    {
        List<Disease> allDiseases = new List<Disease>();
        DiseaseConfigData configData = new DiseaseConfigData();
        Action<Disease> onDiseaseUpdated;

        public DiseaseHandler(GameRunner gameRunner)
        {
            this.owner = gameRunner;
        }

        public List<Disease> AllDiseases
        {
            get => allDiseases;
        }
        private GameRunner owner;

        #region Public Functions
        public void RegisterToDiseaseUpdates(Action<Disease> callBack)
        {
            onDiseaseUpdated += callBack;
        }
        public void UnRegisterToDiseaseUpdates(Action<Disease> callBack)
        {
            onDiseaseUpdated -= callBack;
        }
        public int GetKnownDiseaseCount()
        {
            int count = 0;
            foreach (var disease in allDiseases)
            {
                if (disease.data.isKnown)
                    count++;
            }
            return count;
        }
        public Disease GetDiseaseFromId(Guid id)
        {
            return allDiseases.Find(x => x.data.id == id);
        }
        public void MakeKnown(Disease disease)
        {
            disease.data.isKnown = true;

            Player.UnknownDiseaseCount--;
            Player.KnownDiseaseCount++;

            if (onDiseaseUpdated != null)
                onDiseaseUpdated(disease);
        }
        public void GenerateSomeUnknownDiseases(LevelData levelData, bool forceKnown = false)
        {
            int count = 0;
            foreach (var diseaseGenData in levelData.forcedNewDiseases)
            {
                for (int i = 0; i < diseaseGenData.Value; i++)
                {
                    var newDisease = GenerateNewUnknownDisease(diseaseGenData.Key);
                    count++;
                    if (newDisease == null)
                    {
                        //TODO: handle disease extinction error case
                    }
                    else if (forceKnown)
                    {
                        MakeKnown(newDisease);
                    }
                }
            }
            if (count > 0)
                owner.eventHandler.CreateEvent(Data.EventConfigData.EventType.DiseaseDiscovered, new DiseaseDiscovered(count));
        }

        public Disease GenerateNewUnknownDisease(int level)
        {
            var newDisease = new Disease(new DiseaseData());
            newDisease.data.id = System.Guid.NewGuid();

            int attempt = 0;
            switch (level)
            {
                case 1:
                    do
                    {
                        attempt++;

                        newDisease.data.level = 1;
                        newDisease.data.examResult = false;
                        newDisease.data.diseaseSystem = (DiseaseConfigData.Systems)MyRandom.Range(5);

                        List<string> theSymptoms = new List<string>();
                        newDisease.data.symptoms.Clear();
                        if (configData.symptoms.TryGetValue(newDisease.data.diseaseSystem, out theSymptoms))
                        {
                            string symptom = theSymptoms[MyRandom.Range(theSymptoms.Count)];
                            newDisease.data.symptoms.Add(new LocalizableString(symptom).ToString());
                        }

                        string adj = configData.adj[MyRandom.Range(configData.adj.Count)];
                        newDisease.data.adj = adj;

                        List<string> theOrgans = new List<string>();
                        if (configData.organs.TryGetValue(newDisease.data.diseaseSystem, out theOrgans))
                        {
                            string organ = theOrgans[MyRandom.Range(theOrgans.Count)];
                            newDisease.data.organ = organ;
                        }

                    } while (attempt <= 100 && !CheckDuplicate(newDisease)); break;
                case 2:
                    do
                    {
                        attempt++;

                        newDisease.data.level = 2;
                        newDisease.data.examResult = false;
                        newDisease.data.diseaseSystem = (DiseaseConfigData.Systems)MyRandom.Range(5);

                        List<string> theSymptoms = new List<string>();
                        newDisease.data.symptoms.Clear();
                        if (configData.symptoms.TryGetValue(newDisease.data.diseaseSystem, out theSymptoms))
                        {
                            string symptom = theSymptoms[MyRandom.Range(theSymptoms.Count)];
                            newDisease.data.symptoms.Add(new LocalizableString(symptom).ToString());
                        }

                        string generalSymptom = configData.generalSymptoms[MyRandom.Range(configData.generalSymptoms.Count)];
                        newDisease.data.symptoms.Add(new LocalizableString(generalSymptom).ToString());

                        string adj = configData.adj[MyRandom.Range(configData.adj.Count)];
                        newDisease.data.adj = adj;

                        List<string> theOrgans = new List<string>();
                        if (configData.organs.TryGetValue(newDisease.data.diseaseSystem, out theOrgans))
                        {
                            string organ = theOrgans[MyRandom.Range(theOrgans.Count)];
                            newDisease.data.organ = organ;
                        }

                    } while (attempt <= 100 && !CheckDuplicate(newDisease)); break;
                case 3:
                    do
                    {
                        attempt++;

                        newDisease.data.level = 3;
                        newDisease.data.examResult = false;
                        newDisease.data.diseaseSystem = (DiseaseConfigData.Systems)MyRandom.Range(5);

                        List<string> theSymptoms = new List<string>();
                        newDisease.data.symptoms.Clear();
                        if (configData.symptoms.TryGetValue(newDisease.data.diseaseSystem, out theSymptoms))
                        {
                            string symptom = theSymptoms[MyRandom.Range(theSymptoms.Count)];
                            newDisease.data.symptoms.Add(new LocalizableString(symptom).ToString());
                        }

                        string generalSymptom = configData.generalSymptoms[MyRandom.Range(configData.generalSymptoms.Count)];
                        newDisease.data.symptoms.Add(new LocalizableString(generalSymptom).ToString());

                        List<string> theOtherSymptoms = new List<string>();
                        int otherSystem = System.Math.Abs((int)newDisease.data.diseaseSystem - MyRandom.Range(1, 5));
                        if (configData.symptoms.TryGetValue((DiseaseConfigData.Systems)otherSystem, out theOtherSymptoms))
                        {
                            string otherSymptom = theOtherSymptoms[MyRandom.Range(theOtherSymptoms.Count)];
                            newDisease.data.symptoms.Add(new LocalizableString(otherSymptom).ToString());
                        }

                        string adj = configData.adj[MyRandom.Range(configData.adj.Count)];
                        newDisease.data.adj = adj;

                        List<string> theOrgans = new List<string>();
                        if (configData.organs.TryGetValue(newDisease.data.diseaseSystem, out theOrgans))
                        {
                            string organ = theOrgans[MyRandom.Range(theOrgans.Count)];
                            newDisease.data.organ = organ;
                        }

                    } while (attempt <= 100 && !CheckDuplicate(newDisease)); break;

                    #region Old design
                    // case 4:
                    //     do
                    //     {
                    //         attempt++;

                    //         newDisease.data.level = 4;
                    //         newDisease.data.examResult = true;

                    //         if (knownDiseases.Count == 0)
                    //             attempt = 101;

                    //         bool found = false;
                    //         foreach (var disease in knownDiseases)
                    //         {
                    //             if (disease.data.level == 1 && !disease.data.havePair)
                    //             {
                    //                 newDisease.data.diseaseSystem = disease.data.diseaseSystem;
                    //                 newDisease.data.symptoms = disease.data.symptoms;
                    //                 newDisease.data.organ = disease.data.organ;
                    //                 newDisease.data.havePair = true;
                    //                 disease.data.havePair = true;
                    //                 found = true;
                    //             }
                    //         }
                    //         if (!found) attempt = 101;

                    //         string adj = configData.adj[MyRandom.Range(configData.adj.Count)];
                    //         newDisease.data.adj = adj;

                    //     } while (attempt <= 100 && !CheckDuplicate(newDisease)); break;
                    // case 5:
                    //     do
                    //     {
                    //         attempt++;

                    //         newDisease.data.level = 5;
                    //         newDisease.data.examResult = true;

                    //         if (knownDiseases.Count == 0)
                    //             attempt = 101;

                    //         bool found = false;
                    //         foreach (var disease in knownDiseases)
                    //         {
                    //             if (disease.data.level == 2 && !disease.data.havePair)
                    //             {
                    //                 newDisease.data.diseaseSystem = disease.data.diseaseSystem;
                    //                 newDisease.data.symptoms = disease.data.symptoms;
                    //                 newDisease.data.organ = disease.data.organ;
                    //                 newDisease.data.havePair = true;
                    //                 disease.data.havePair = true;
                    //                 found = true;
                    //             }
                    //         }
                    //         if (!found) attempt = 101;

                    //         string adj = configData.adj[MyRandom.Range(configData.adj.Count)];
                    //         newDisease.data.adj = adj;

                    //     } while (attempt <= 100 && !CheckDuplicate(newDisease)); break;
                    // case 6:
                    //     do
                    //     {
                    //         attempt++;

                    //         newDisease.data.level = 6;
                    //         newDisease.data.examResult = true;

                    //         if (knownDiseases.Count == 0)
                    //             attempt = 101;

                    //         bool found = false;
                    //         foreach (var disease in knownDiseases)
                    //         {
                    //             if (disease.data.level == 3 && !disease.data.havePair)
                    //             {
                    //                 newDisease.data.diseaseSystem = disease.data.diseaseSystem;
                    //                 newDisease.data.symptoms = disease.data.symptoms;
                    //                 newDisease.data.organ = disease.data.organ;
                    //                 newDisease.data.havePair = true;
                    //                 disease.data.havePair = true;
                    //                 found = true;
                    //             }
                    //         }
                    //         if (!found) attempt = 101;

                    //         string adj = configData.adj[MyRandom.Range(configData.adj.Count)];
                    //         newDisease.data.adj = adj;

                    //     } while (attempt <= 100 && !CheckDuplicate(newDisease)); break;
                    #endregion
            }

            if (attempt > 100)
                return null;
            else
            {
                newDisease.FormName();
                allDiseases.Add(newDisease);
                Player.UnknownDiseaseCount++;
                if (onDiseaseUpdated != null)
                    onDiseaseUpdated(newDisease);

                return newDisease;
            }

        }
        public Disease Debug_GetARandomDisease()
        {
            if (allDiseases.Count == 0)
                return null;
            return allDiseases[MyRandom.Range(allDiseases.Count)];
        }
        public string Debug_DisplayDiseaseInText(Disease disease)
        {
            string resultTxt = "";
            if (disease == null)
                resultTxt = "null";
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var symptom in disease.data.symptoms)
                {
                    sb.Append("[" + symptom + "]");
                }
                sb.Append(" : " + disease.data.examResult);
                resultTxt = $"{disease.data.theName} : {sb}";
            }

            return resultTxt;
        }
        #endregion

        #region Private Functions
        private bool CheckDuplicate(Disease disease)
        {
            bool noDuplicates = true;
            foreach (var aDisease in allDiseases)
            {
                //check name
                if (aDisease.data.adj == disease.data.adj && aDisease.data.organ == disease.data.organ)
                    noDuplicates = false;

                //check symptom
                if (aDisease.data.examResult == disease.data.examResult && aDisease.data.symptoms.Count == disease.data.symptoms.Count)
                {
                    bool same = true;
                    foreach (var aSymptom in aDisease.data.symptoms)
                    {
                        if (disease.data.symptoms.Contains(aSymptom))
                            continue;
                        else
                        {
                            same = false;
                            break;
                        }
                    }
                    noDuplicates = same ? false : noDuplicates;
                }
            }

            return noDuplicates;
        }
        #endregion
    }
}

