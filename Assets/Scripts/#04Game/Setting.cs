using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project9
{
    public class Setting : MonoBehaviour
    {
        void OnEnable()
        {
            GameManager.instance.Stop();
        }

        public void CloseSetting()
        {
            GameManager.instance.Resume();
            gameObject.SetActive(false);
        }

        public void LoadSceneLobby()
        {
            Utils.LoadScene("Lobby");
        }
    }
}