using System.Collections.Generic;

namespace RefinedGame.Data
{
    public class TalentConfigData
    {
        readonly public int minRateInclusive = 3;
        readonly public int maxRateExclusive = 7;

        readonly public List<TalentData> allTalents = new List<TalentData>
        {
            new TalentData
            {
                content = "Super Immunity",
                level = 3,
                type = TalentData.TalentType.Immunity,
            },
            new TalentData
            {
                content = "Fast Learner",
                level = 2,
                type = TalentData.TalentType.LessCost,
            },
            new TalentData
            {
                content = "Had Some Training",
                level = 1,
                type = TalentData.TalentType.ExtraGrowth,
            },
            new TalentData
            {
                content = "-",
                level = 0,
                type = TalentData.TalentType.None,
            },
        };
    }
}