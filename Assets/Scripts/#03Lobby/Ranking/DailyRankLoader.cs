using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;
using UnityEngine.UI;

public class DailyRankLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject rankDataPrefab;      // 랭킹 정보 출력을 위한 UI 프리팹 원본 
    [SerializeField]
    private Scrollbar scrollbar;            // scrollbar의 value 설정 (활성화 될 때 1위가 보이도록 ) 
    [SerializeField]
    private Transform rankDataParent;       // ScrollView의 Content 오브젝트

    [SerializeField]
    private DailyRankData myRankData;           // 내 랭킹 정보를 출력하는 UI 게임 오브젝트 . 

    private List<DailyRankData> rankDataList;

    private void Awake()
    {
        rankDataList = new List<DailyRankData>();

        Backend.Initialize(true);
        Backend.BMember.GuestLogin();
        /*
        // 1 ~ 20위 랭킹 출력을 위한 UI 오브젝트 생성 
        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            rankDataList.Add(clone.GetComponent<DailyRankData>());
        }
        */

        /* 데이터 테이블에 데이터 삽입 & 랭킹 데이터 업데이트
        Param param = new Param();
        param.Add("dailyBestScore", 20);

        Backend.BMember.GuestLogin();

        Backend.GameData.Insert(Constants.USER_DATA_TABLE, param);
        var str = Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where());
        Debug.Log(str);
        var indate = str.GetInDate();


        param = new Param();
        param.Add("dailyBestScore", 120);
        Backend.URank.User.UpdateUserScore(Constants.DAILY_RANK_UUID, Constants.USER_DATA_TABLE, indate, param);
        */
    }

    private void Update()
    {
        if (Backend.IsInitialized && Backend.UseAsyncQueuePoll)
        {
            Backend.AsyncPoll();
        }
    }

    private void OnEnable()
    {
        // 1위 랭킹이 보이도록 scroll 값 설정 
        //scrollbar.value = 1;
        // 1 ~ 100  위의 랭킹 정보 불러오기 
        GetRankList();
        // 내 랭킹 정보 불러오기 .
        GetMyRank();
    }

    private void GetRankList()
    {
        // 1 ~ 100위 랭킹 정보 불러오기 
        Backend.URank.User.GetRankList(Constants.DAILY_RANK_UUID, Constants.MAX_RANK_LIST, callback =>
        {
            if (callback.IsSuccess())
            {
                // JSON 데이터 파싱 성공 
                try
                {
                    Debug.Log($"랭킹 조회에 성공했습니다.  : {callback}");

                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    //받아온 데이터의 개수가 0 이면 데이터가 없는 것 
                    if (rankDataJson.Count <= 0)
                    {
                        // 1 ~ 20위 까지 데이터를 빈 데이터로 설정 
                        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }

                        Debug.LogWarning("데이터가 존재하지 않습니다 . ");
                    }
                    else
                    {
                        int len = 20;

                        // 1 ~ 20위 랭킹 출력을 위한 UI 오브젝트 생성 
                        for (int i = 0; i < len; ++i)
                        {
                            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
                            rankDataList.Add(clone.GetComponent<DailyRankData>());
                        }

                        int rankerCount = rankDataJson.Count;

                        // 랭킹 정보를 불러와 출력할 수 있도록 설정 
                        for (int i = 0; i < rankerCount; ++i)
                        {
                            rankDataList[i].Rank = int.Parse(rankDataJson[i]["rank"].ToString());
                            rankDataList[i].Score = int.Parse(rankDataJson[i]["score"].ToString());

                            Debug.Log("111");

                            // 닉네임을 별도로 설정하지 않은 유저는 내임 대신 gamerId를 출력 
                            // rankDataList[i].Nickname = rankDataJson[i].ContainsKey("nickname") == true ?
                            //                         rankDataJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                        }
                        // 만약 rankerCount 가 100 위 까지 존재하지 않으면 나머지 랭킹에는 빈 데이터를 설정 
                        for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            Debug.Log("222");
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }
                    }
                }
                // JSON ������ �Ľ� ����
                catch (System.Exception e)
                {
                    // try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            else
            {
                // 1 ~ 20������ �����͸� �� �����ͷ� ����
                for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                {
                    SetRankData(rankDataList[i], i + 1, "-", 0);
                }

                Debug.LogError($"��ŷ ��ȸ �� ������ �߻��߽��ϴ� : {callback}");
            }
        });
    }

    private void GetMyRank()
    {
        // �� ��ŷ ���� �ҷ�����
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback =>
        {
            // �г����� ������ gamerId, �г����� ������ nickname ���
            string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

            if (callback.IsSuccess())
            {
                // JSON ������ �Ľ� ����
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                    if (rankDataJson.Count <= 0)
                    {
                        // ["������ ����", "�г���", 0]�� ���� ���
                        SetRankData(myRankData, 1000000000, nickname, 0);

                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
                        myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

                        // �г����� ������ �������� ���� ������ ������ �� �ֱ� ������
                        // �г����� �������� �ʴ� ������ �г��� ��� gamerId�� ���
                        myRankData.Nickname = rankDataJson[0].ContainsKey("nickname") == true ?
                                              rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                    }
                }
                // �ڽ��� ��ŷ ���� JSON ������ �Ľ̿� �������� ��
                catch (System.Exception e)
                {
                    // ["������ ����", "�г���", 0]�� ���� ���
                    SetRankData(myRankData, 1000000000, nickname, 0);

                    // try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            else
            {
                // �ڽ��� ��ŷ ���� �����Ͱ� �������� ���� ��
                if (callback.GetMessage().Contains("userRank"))
                {
                    // ["������ ����", "�г���", 0]�� ���� ���
                    SetRankData(myRankData, 1000000000, nickname, 0);
                }
            }
        });
    }

    private void SetRankData(DailyRankData rankData, int rank, string nickname, int score)
    {
        rankData.Rank = rank;
        rankData.Nickname = nickname;
        rankData.Score = score;
    }
}
