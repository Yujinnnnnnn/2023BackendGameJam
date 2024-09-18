using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UserDataManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI moneyText;


    public void Update()
    {
        moneyText.text = BackendGameData.Instance.UserGameData.gold.ToString();
    }



}
