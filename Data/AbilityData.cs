namespace RefinedGame.Data
{
    public class AbilityData
    {
        public int level;
        public int skill;
        public int lvlUpCost;
        public TalentData talentData = new TalentData();
        public int growth;
        public int immunityX10;
        public Status status = Status.Onduty;

        public enum Status
        {
            Onduty,
            Offduty,
            InICU,
        }
    }
}