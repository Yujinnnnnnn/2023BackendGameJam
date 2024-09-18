using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{ 
    public class CutScene : MonoBehaviour
    {
        bool isEndCutScene = false;

        void OnEnable()
        {
            Time.timeScale = 1;
            isEndCutScene = false;
        }

        public void EndCutScene()
        {
            isEndCutScene = true;
        }

        public void CloseCutScene(bool isStartGame)
        {
            gameObject.SetActive(false);
            if (isStartGame) GameManager.instance.GameStart(0);
        }
    }
}
