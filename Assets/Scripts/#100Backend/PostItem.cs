using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class PostItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI moneyText;



    public string title;
    public string content;
    public int money;

    public void Init(Post post)
    {
        title = post.title;
        content = post.content;
        money = post.postReward.ContainsKey("Gold") ? post.postReward["Gold"] : 0;

        titleText.text = title;
        contentText.text = content;
        moneyText.text = "x" + money;
    }



    public void Receive()
    {
       
        // 저장된 우편의 위치를 읽어 우편을 수령합니다. 
        BackendPost.Instance.PostReceive(PostType.Admin, 0);

        BackendGameData.Instance.UserGameData.gold += money;

        Debug.Log(money);
        Destroy(gameObject);


    }


}
