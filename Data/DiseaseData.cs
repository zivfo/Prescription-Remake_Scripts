using System.Collections.Generic;

namespace RefinedGame.Data
{
    public class DiseaseData
    {
        public System.Guid id;
        public string theName;
        public int level;
        public bool isKnown = false;
        public DiseaseConfigData.Systems diseaseSystem;
        public string adj;
        public string organ;
        public List<string> symptoms = new List<string>();
        public bool examResult;
        public System.Guid preCondition;
        public bool havePair = false;
    }
}

