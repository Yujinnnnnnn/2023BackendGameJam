using UnityEngine;
using TMPro;

public class DailyRankData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textRank;
    [SerializeField]
    private TextMeshProUGUI textNickName;
    [SerializeField]
    private TextMeshProUGUI textScore;
    [SerializeField]
    private TextMeshProUGUI textCatName;


    private int rank;
    private string nickname;
    private int score;
   

    public int Rank
    {
        set
        {
            if (value <= Constants.MAX_RANK_LIST)
            {
                rank = value;
                print(rank);
                textRank.text = rank.ToString();
            }
            else
            {
                textRank.text =  "순위에 없음 . ";
            }
        }
        get => rank;
    }

    public string Nickname
    {
        set
        {
            nickname = value;
            textNickName.text = nickname;
        }
        get => nickname;
    }

    public int Score
    {
        set
        {
            score = value;
            textScore.text = score.ToString();
        }
        get => score;
    }
}

