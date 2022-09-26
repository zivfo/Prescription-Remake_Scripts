using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RefinedGame.Tool.DebugUtil
{
    [RequireComponent(typeof(Button))]
    public class DebugEventBtn : MonoBehaviour
    {
        public void Init(CheatType cheatType)
        {
            GetComponent<Button>().onClick.AddListener(() => DebugEventsController.GetInstance().Send(cheatType));
            GetComponentInChildren<TMP_Text>().text = $"{cheatType}".Substring($"{cheatType}".IndexOf("_"));
        }
    }
}
