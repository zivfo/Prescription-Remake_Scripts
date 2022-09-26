using UnityEngine;

namespace RefinedGame.Tool.DebugUtil
{
    public static class PlatformConfigs
    {
        public static bool cheatsEnabled = false;
        public static bool DebugEnabled { get => debugEnabled; }

        static bool debugEnabled = false;
        static bool hasInit = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            if (hasInit)
                return;

            hasInit = true;

#if DEVELOPMENT_BUILD
            debugEnabled = true;
            cheatsEnabled = true;
            Debug.Log("[DEVELOPMENT]");

#endif

#if UNITY_EDITOR
            cheatsEnabled = true;
            debugEnabled = true;
            Debug.Log("[EDITOR]");

#elif UNITY_STANDALONE
        Debug.Log("[STANDALONE]");

#elif UNITY_IOS || UNITY_ANDROID
        Application.targetFrameRate = 60;
        Debug.Log("[MOBILE]");
        
#elif UNITY_WEBGL
        Application.targetFrameRate = 60;
        Debug.Log("[WEBGL]");

#else
        Application.targetFrameRate = 60;
        Debug.Log("[OTHER]");

#endif
        }
    }
}


