using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RefinedGame.Tool.DebugUtil
{
    public class DebugCanvasTapBtn : MonoBehaviour, IPointerClickHandler
    {
        readonly float maxTapInterval = 0.3f;
        readonly int hitNeeded = 13;

        DebugCanvas owner;
        float timer = 100.0f;
        int hitTimes = 0;


        #region Unity Functions
        private void Update()
        {
            if (PlatformConfigs.DebugEnabled && Input.GetKey(KeyCode.B) && Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.G))
            {
                owner.OpenDebugScreen();
            }

            if (timer < 1.0f && timer > 0.0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 100.0f;
                hitTimes = 0;
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (hitTimes > hitNeeded)
            {
                timer = 100.0f;
                hitTimes = 0;
                owner.OpenDebugScreen();
                return;
            }

            hitTimes++;
            timer = maxTapInterval;
        }
        #endregion

        #region Public Functions
        public void Init(DebugCanvas owner)
        {
            this.owner = owner;
        }
        #endregion
    }
}
