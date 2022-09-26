using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RefinedGame.Show
{

    public class EndingWindow : AUiWindow
    {
        [SerializeField] Button returnBtn = null;
        public override void Init()
        {
            returnBtn.onClick.AddListener(() => SceneManager.LoadScene("StartScene"));
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
