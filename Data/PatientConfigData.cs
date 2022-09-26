using System.Collections.Generic;


namespace RefinedGame.Data
{
    public class PatientConfigData
    {
        readonly public List<string> manNames = new List<string> { "MName_Liam", "MName_Noah", "MName_Oliver", "MName_Elijah", "MName_William", "MName_James", "MName_Benjamin", "MName_Lucas", "MName_Henry", "MName_Alexander", "MName_Mason", "MName_Michael", "MName_Ethan", "MName_Daniel", "MName_Jacob", "MName_Logan", "MName_Jackson", "MName_Levi", "MName_Sebastian", "MName_Mateo", "MName_Jack", "MName_Owen", "MName_Theodore", "MName_Aiden", "MName_Samuel", "MName_Joseph", "MName_John", "MName_David", "MName_Wyatt", "MName_Matthew", "MName_Luke", "MName_Asher", "MName_Carter", "MName_Julian", "MName_Grayson", "MName_Leo", "MName_Jayden", "MName_Gabriel", "MName_Isaac", "MName_Lincoln", "MName_Anthony", "MName_Hudson", "MName_Dylan", "MName_Ezra", "MName_Thomas", "MName_Charles", "MName_Christopher", "MName_Jaxon", "MName_Maverick", "MName_Josiah", "MName_Isaiah", "MName_Andrew", "MName_Elias", "MName_Joshua", "MName_Nathan", "MName_Caleb", "MName_Ryan", "MName_Adrian", "MName_Miles", "MName_Eli", "MName_Nolan", "MName_Christian", "MName_Aaron", "MName_Cameron", "MName_Ezekiel", "MName_Colton", "MName_Luca", "MName_Landon", "MName_Hunter", "MName_Jonathan", "MName_Santiago", "MName_Axel", "MName_Easton", "MName_Cooper", "MName_Jeremiah", "MName_Angel", "MName_Roman", "MName_Connor", "MName_Jameson", "MName_Robert", "MName_Greyson", "MName_Jordan", "MName_Ian", "MName_Carson", "MName_Jaxson", "MName_Leonardo", "MName_Nicholas", "MName_Dominic", "MName_Austin", "MName_Everett", "MName_Brooks", "MName_Xavier", "MName_Kai", "MName_Jose", "MName_Parker", "MName_Adam", "MName_Jace", "MName_Wesley", "MName_Kayden", "MName_Silas", };
        readonly public List<string> womanNames = new List<string> { "WName_Olivia", "WName_Emma", "WName_Charlotte", "WName_Amelia", "WName_Ava", "WName_Sophia", "WName_Isabella", "WName_Mia", "WName_Evelyn", "WName_Harper", "WName_Luna", "WName_Camila", "WName_Gianna", "WName_Elizabeth", "WName_Eleanor", "WName_Ella", "WName_Abigail", "WName_Sofia", "WName_Avery", "WName_Scarlett", "WName_Emily", "WName_Aria", "WName_Penelope", "WName_Chloe", "WName_Layla", "WName_Mila", "WName_Nora", "WName_Hazel", "WName_Madison", "WName_Ellie", "WName_Lily", "WName_Nova", "WName_Isla", "WName_Grace", "WName_Violet", "WName_Aurora", "WName_Riley", "WName_Zoey", "WName_Willow", "WName_Emilia", "WName_Stella", "WName_Zoe", "WName_Victoria", "WName_Hannah", "WName_Addison", "WName_Leah", "WName_Lucy", "WName_Eliana", "WName_Ivy", "WName_Everly", "WName_Lillian", "WName_Paisley", "WName_Elena", "WName_Naomi", "WName_Maya", "WName_Natalie", "WName_Kinsley", "WName_Delilah", "WName_Claire", "WName_Audrey", "WName_Aaliyah", "WName_Ruby", "WName_Brooklyn", "WName_Alice", "WName_Aubrey", "WName_Autumn", "WName_Leilani", "WName_Savannah", "WName_Valentina", "WName_Kennedy", "WName_Madelyn", "WName_Josephine", "WName_Bella", "WName_Skylar", "WName_Genesis", "WName_Sophie", "WName_Hailey", "WName_Sadie", "WName_Natalia", "WName_Quinn", "WName_Caroline", "WName_Allison", "WName_Gabriella", "WName_Anna", "WName_Serenity", "WName_Nevaeh", "WName_Cora", "WName_Ariana", "WName_Emery", "WName_Lydia", "WName_Jade", "WName_Sarah", "WName_Eva", "WName_Adeline", "WName_Madeline", "WName_Piper", "WName_Rylee", "WName_Athena", "WName_Peyton", "WName_Everleigh", };

        public enum DetailedSpeechType
        {
            niceGreeting, badGreeting, niceRejoin, badRejoin, symptomDetail, niceTone, badTone, neutralTone, niceReply, badReply,
        }
        public enum SpeechType
        {
            Greeting, Rejoin, Detail, Tone, Reply,
        }


        readonly public Dictionary<DetailedSpeechType, List<string>> speeches = new Dictionary<DetailedSpeechType, List<string>>
        {
            { DetailedSpeechType.niceGreeting, new List<string>() { "Speech_niceGreeting1", "Speech_niceGreeting2", "Speech_niceGreeting3", "Speech_niceGreeting4", "Speech_niceGreeting5",} },
            { DetailedSpeechType.badGreeting, new List<string>() { "Speech_badGreeting1", "Speech_badGreeting2", "Speech_badGreeting3", "Speech_badGreeting4", "Speech_badGreeting5", "Speech_badGreeting6",} },
            { DetailedSpeechType.niceRejoin, new List<string>() { "Speech_niceRejoin1", "Speech_niceRejoin2", "Speech_niceRejoin3", "Speech_niceRejoin4", "Speech_niceRejoin5", } },
            { DetailedSpeechType.badRejoin, new List<string>() { "Speech_badRejoin1", "Speech_badRejoin2", "Speech_badRejoin3", "Speech_badRejoin4", "Speech_badRejoin5", } },
            { DetailedSpeechType.symptomDetail, new List<string>() { "Speech_symptomDetail1", "Speech_symptomDetail2", "Speech_symptomDetail3", "Speech_symptomDetail4",
            "Speech_symptomDetail5",} },
            { DetailedSpeechType.niceTone, new List<string>() { "Speech_niceTone1", "Speech_niceTone2", "Speech_niceTone3", "Speech_niceTone4", "Speech_niceTone5", } },
            { DetailedSpeechType.badTone, new List<string>() { "Speech_badTone1", "Speech_badTone2", "Speech_badTone3", "Speech_badTone4", "Speech_badTone5", }},
            { DetailedSpeechType.neutralTone, new List<string>() { "Speech_neutralTone1", "Speech_neutralTone2", "Speech_neutralTone3", "Speech_neutralTone4", "Speech_neutralTone5", }},
            { DetailedSpeechType.niceReply, new List<string>() { "Speech_niceReply1", "Speech_niceReply2", "Speech_niceReply3", "Speech_niceReply4", "Speech_niceReply5", }},
            { DetailedSpeechType.badReply, new List<string>() { "Speech_badReply1", "Speech_badReply2", "Speech_badReply3", "Speech_badReply4", "Speech_badReply5", }},
        };

        public enum Category
        {
            begger, rogue, noble, commoner,
        }
        readonly public Dictionary<Category, List<string>> occupations = new Dictionary<Category, List<string>>
        {
            { Category.begger, new List<string>() {"Occupation_beggar","Occupation_refugee","Occupation_farmer","Occupation_miner","Occupation_poorman","Occupation_idiot", } },
            { Category.rogue, new List<string>() {"Occupation_robber","Occupation_Thief","Occupation_gangster","Occupation_rogue","Occupation_thug","Occupation_murderer",} },
            { Category.noble, new List<string>() {"Occupation_knight","Occupation_richman","Occupation_professor","Occupation_official","Occupation_baron","Occupation_bishop", } },
            { Category.commoner, new List<string>() {"Occupation_worker","Occupation_clerk","Occupation_soldier","Occupation_vendor","Occupation_teacher","Occupation_police", }},
        };
    }
}


