using System.Collections.Generic;

namespace RefinedGame.Data
{
    public class LevelConfigData
    {
        //<Level, <DiseaseLevel, Count>>
        readonly public Dictionary<int, Dictionary<int, int>> levelPatientConfigChart = new Dictionary<int, Dictionary<int, int>>
        {
            {0, new Dictionary<int, int>{{1,2},}},
            {1, new Dictionary<int, int>{}},
            {2, new Dictionary<int, int>{{1,1},}},
            {3, new Dictionary<int, int>{{1,1},}},
            {4, new Dictionary<int, int>{{1,2},{2,1}}},
            {5, new Dictionary<int, int>{}},
            {6, new Dictionary<int, int>{{3,1},{1,2},{2,1}}},
            {7, new Dictionary<int, int>{}},
            {8, new Dictionary<int, int>{{2,1},}},
            {9, new Dictionary<int, int>{{1,1},}},
            {10, new Dictionary<int, int>{{3,1},}},
            {11, new Dictionary<int, int>{{1,2},}},
            {12, new Dictionary<int, int>{{1,3},}},
            {13, new Dictionary<int, int>{{2,1},}},
            {14, new Dictionary<int, int>{{1,1},}},
            {15, new Dictionary<int, int>{{1,1},}},
            {16, new Dictionary<int, int>{{1,1},}},
            {17, new Dictionary<int, int>{{1,1},}},
            {18, new Dictionary<int, int>{{1,1},}},
            {19, new Dictionary<int, int>{{1,1},}},
            {20, new Dictionary<int, int>{{1,1},}},
            {21, new Dictionary<int, int>{{1,1},}},
            {22, new Dictionary<int, int>{{1,1},}},
            {23, new Dictionary<int, int>{{1,1},}},
            {24, new Dictionary<int, int>{{1,1},}},
            {25, new Dictionary<int, int>{{1,1},}},
            {26, new Dictionary<int, int>{{1,1},}},
            {27, new Dictionary<int, int>{{1,1},}},
            {28, new Dictionary<int, int>{{1,1},}},
            {29, new Dictionary<int, int>{{1,1},}},
            {30, new Dictionary<int, int>{{1,1},}},
            {31, new Dictionary<int, int>{{1,1},}},
            {32, new Dictionary<int, int>{{1,1},}},
        };
    }
}
