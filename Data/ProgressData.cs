using System.Collections.Generic;

namespace RefinedGame.Data
{
    public class ProgressData
    {
        public enum Type
        {
            PlayerProgress,

            PlayerSpeed,
            PlayerAcceleration,

            WorldSpeed,
            WorldAcceleration,
        }

        public Dictionary<Type, int> allData = new Dictionary<Type, int>()
        {
            {Type.PlayerProgress,-1},

            {Type.PlayerSpeed,-1},
            {Type.PlayerAcceleration,-1},

            {Type.WorldSpeed,-1},
            {Type.WorldAcceleration,-1},
        };
    }
}
