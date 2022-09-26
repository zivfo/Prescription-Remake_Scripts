using System.Collections.Generic;

namespace RefinedGame.Data
{
    public class RewardData
    {
        public Dictionary<RewardType, int> rewards;

        public enum RewardType
        {
            None,
            Credit,
            Money,
        }
    }
}