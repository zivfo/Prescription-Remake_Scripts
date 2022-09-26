namespace RefinedGame.Data
{
    public class TalentData
    {
        public string content;
        public int level;
        public int chanceX10;
        public TalentType type;


        public enum TalentType
        {
            Immunity,
            LessCost,
            ExtraGrowth,
            None,
        }
    }
}