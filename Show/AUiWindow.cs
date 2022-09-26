using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefinedGame.Show
{
    public abstract class AUiWindow : MonoBehaviour
    {
        public abstract void Init();
        public virtual void OnUpdate()
        {

        }
    }
}
