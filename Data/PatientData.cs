using System.Collections.Generic;


namespace RefinedGame.Data
{
    public class PatientData
    {
        public System.Guid id;
        public string theName;
        public Dictionary<PatientConfigData.SpeechType, List<string>> sentences = new Dictionary<PatientConfigData.SpeechType, List<string>>();
        public string occupation;
        public PatientConfigData.Category category;
        public int characteristic;
        public bool isMale;
        public int age;
        public bool sos;
    }
}


