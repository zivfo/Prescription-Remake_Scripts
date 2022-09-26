using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Tool.DebugUtil
{
    public class DebugCanvas : MonoBehaviour
    {
        [SerializeField] TMP_Text logTxt = null;
        [SerializeField] GameObject debugCanvas = null;
        [SerializeField] CanvasGroup canvasScrollGroup = null;
        [SerializeField] DebugCanvasTapBtn tapAreaBtn = null;
        [SerializeField] Button enableCheatBtn = null;
        [SerializeField] Button closeScreenBtn = null;
        [SerializeField] Button clearScreenBtn = null;
        [SerializeField] Toggle scrollToggle = null;
        [SerializeField] Transform cheatBtnParent = null;
        [SerializeField] DebugEventBtn btnTemplate = null;

        private static string debugLog = "";
        private List<DebugEventBtn> currentBtns = new List<DebugEventBtn>();
        private string outputLine;


        #region Unity Functions
        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMsgToUi;
        }
        private void Awake()
        {
            Application.logMessageReceived += LogMsgToUi;

            InitSelf();
        }
        private void Start()
        {
            InitCheatBtns();
        }
        #endregion

        #region Public Functions
        public void OpenDebugScreen()
        {
            debugCanvas.gameObject.SetActive(true);
        }
        #endregion

        #region Private Functions
        private void InitCheatBtns()
        {
            foreach (CheatType cheatType in Enum.GetValues(typeof(CheatType)))
            {
                if (cheatType == CheatType.None)
                    continue;

                var newCheatBtn = Instantiate(btnTemplate, cheatBtnParent, false);
                newCheatBtn.Init(cheatType);

                newCheatBtn.gameObject.SetActive(true);
                currentBtns.Add(newCheatBtn);
            }
        }
        private void InitSelf()
        {
            debugCanvas.gameObject.SetActive(false);
            tapAreaBtn.Init(this);
            btnTemplate.gameObject.SetActive(false);
            enableCheatBtn.GetComponentInChildren<TMP_Text>().text = PlatformConfigs.cheatsEnabled ? "CheatsOn" : "CheatsOff";
            enableCheatBtn.GetComponent<Image>().color = PlatformConfigs.cheatsEnabled ? Color.green : Color.white;

            scrollToggle.isOn = canvasScrollGroup.blocksRaycasts;
            scrollToggle.onValueChanged.AddListener((bool isOn) =>
            {
                canvasScrollGroup.blocksRaycasts = isOn;
            });
            clearScreenBtn.onClick.AddListener(() =>
            {
                debugLog = $"<color=white>{System.DateTime.Now.ToString("u")}</color>  " + "---- Cleared ----";
                logTxt.text = debugLog;
            });
            closeScreenBtn.onClick.AddListener(() => debugCanvas.gameObject.SetActive(false));
            enableCheatBtn.onClick.AddListener(() =>
            {
                PlatformConfigs.cheatsEnabled = !PlatformConfigs.cheatsEnabled;
                enableCheatBtn.GetComponentInChildren<TMP_Text>().text = PlatformConfigs.cheatsEnabled ? "CheatsOn" : "CheatsOff";
                enableCheatBtn.GetComponent<Image>().color = PlatformConfigs.cheatsEnabled ? Color.green : Color.white;
            });
        }
        private void LogMsgToUi(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Log && type != LogType.Warning && PlatformConfigs.DebugEnabled)
                debugCanvas.gameObject.SetActive(true);

            string timeStamp = (type == LogType.Error || type == LogType.Assert || type == LogType.Exception) ?
                $"<color=red>{System.DateTime.Now.ToString("u")}</color>  " : $"<color=white>{System.DateTime.Now.ToString("u")}</color>  ";

            if (type == LogType.Warning)
                timeStamp = $"<color=yellow>{System.DateTime.Now.ToString("u")}</color>  ";

            outputLine = timeStamp + logString;
            debugLog = outputLine + "\n" + debugLog;
            if (debugLog.Length > 10000)
            {
                debugLog = debugLog.Substring(0, 10000);
            }
            logTxt.text = debugLog;
        }
        #endregion
    }
}


