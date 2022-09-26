using System;
using System.Collections;
using System.Collections.Generic;

namespace RefinedGame.Tool.DebugUtil
{
    public abstract class ACheatData
    {
        public CheatType cheatType;

        public ACheatData(CheatType cheatType)
        {
            this.cheatType = cheatType;
        }
    }
    public class None : ACheatData
    {
        public int testNum;
        public None(int testNum) : base(CheatType.None)
        {
            this.testNum = testNum;
        }
    }

    public class DebugEventsController
    {
        private static DebugEventsController instance;
        public static DebugEventsController GetInstance()
        {
            if (instance == null)
                instance = new DebugEventsController();
            return instance;
        }

        public Dictionary<CheatType, List<Action<ACheatData>>> allTypedCallbacks = new Dictionary<CheatType, List<Action<ACheatData>>>();


        public void Send(ACheatData data)
        {
            if (!PlatformConfigs.DebugEnabled || !PlatformConfigs.cheatsEnabled)
                return;

            if (allTypedCallbacks.ContainsKey(data.cheatType))
            {
                foreach (var callbacks in allTypedCallbacks[data.cheatType])
                {
                    callbacks(data);
                }
            }
        }
        public void Send(CheatType cheatType)
        {
            if (!PlatformConfigs.DebugEnabled || !PlatformConfigs.cheatsEnabled)
                return;

            if (allTypedCallbacks.ContainsKey(cheatType))
            {
                foreach (var callbacks in allTypedCallbacks[cheatType])
                {
                    callbacks(null);
                }
            }
        }

        public void Subscribe(CheatType cheatType, Action<ACheatData> callback)
        {
            if (!PlatformConfigs.DebugEnabled)
                return;

            if (!allTypedCallbacks.ContainsKey(cheatType))
            {
                allTypedCallbacks.Add(cheatType, new List<Action<ACheatData>>());
            }
            allTypedCallbacks[cheatType].Add(callback);
        }

        public void UnSubscribe(CheatType cheatType, Action<ACheatData> listening)
        {
            if (!PlatformConfigs.DebugEnabled)
                return;

            if (!allTypedCallbacks.ContainsKey(cheatType))
                return;

            allTypedCallbacks[cheatType].Remove(listening);
        }
    }

    public enum CheatType
    {
        None,
        Game_SwitchPhases,
        Clinic_GenerateCards,
        Clinic_PopCards,
        Clinic_HideCards,
        Data_Add100Money,
        Data_Remove100Money,
        Data_Add10Credit,
        Data_Remove10Credit,
        Debug_TestErrorFire,
        Game_CallAPatient,
        Game_GenerateADisease,
        Book_CloseMedsBook,
        Book_SwitchBookPos,
        Clinic_SwitchNotePos,
        Game_ClearAllPatients,
        Game_SendAllPatientsToIcu,
    }
}