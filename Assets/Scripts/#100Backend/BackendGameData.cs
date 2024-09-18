using UnityEngine;
using BackEnd;
using UnityEngine.Events;
using System.Text;
using System.Collections.Generic;



public class UserData
{
    public int level = 1;
    public float atk = 3.5f;
    public string info = string.Empty;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public List<string> equipment = new List<string>();

    // 데이터를 디버깅하기 위한 함수입니다.(Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"level : {level}");
        result.AppendLine($"atk : {atk}");
        result.AppendLine($"info : {info}");

        result.AppendLine($"inventory");
        foreach (var itemKey in inventory.Keys)
        {
            result.AppendLine($"| {itemKey} : {inventory[itemKey]}개");
        }

        result.AppendLine($"equipment");
        foreach (var equip in equipment)
        {
            result.AppendLine($"| {equip}");
        }

        return result.ToString();
    }
}


public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    private static BackendGameData instance;
    public static BackendGameData Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new BackendGameData();
            }

            return instance; 
        }
    }

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    public static UserData userData;

    private string gameDataRowInDate = string.Empty;


    /// <summary>
    /// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가 
    /// </summary>
    public void GameDataInsert()
    {
        // ���� ������ �ʱⰪ���� ����
        userGameData.Reset();

        //---------------

        userData.level = 1;
        userData.atk = 3.5f;
        userData.info = "ǿ� ������ ��.";

        userData.equipment.Add("ǿ� ������ ��");
        userData.equipment.Add("ǿ� ������ ��");
        userData.equipment.Add(" ������  ��� ");

        userData.inventory.Add(" ������ ", 1);
        userData.inventory.Add(" ������ ", 1);
        userData.inventory.Add(" ������ ", 1);



        ///--------------------
        // ���̺��� �߰��� �����ͷ� ����
        Param param = new Param()
        {
            { "level", userGameData.level },
            { "experience", userGameData.experience },
            { "gold", userGameData.gold },
            { "jewel", userGameData.jewel },
            { "heart", userGameData.heart },
            { "dailyBestScore", userGameData.dailyBestScore },

            { "jipsa_HP_Lv", userGameData.jipsa_HP_Lv},
            { "cat_HP_Lv", userGameData.cat_HP_Lv},
            { "cat_Speed_Lv", userGameData.cat_Speed_Lv},

        };

        // ù ��° �Ű������� �ڳ� �ܼ��� "���� ���� ����" �ǿ� ������ ���̺� �̸�
        Backend.GameData.Insert("USER_DATA", param, callback =>
        {
            // ���� ���� �߰��� �������� ��
            if (callback.IsSuccess())
            {
                // ���� ������ ������
                gameDataRowInDate = callback.GetInDate();

                Debug.Log($"���� ���� ������ ���Կ� �����߽��ϴ�. : {callback}");
            }
            // �������� ��
            else
            {
                Debug.LogError($"���� ���� ������ ���Կ� �����߽��ϴ�. : {callback}");
            }
        });

    }

    public void GameDataGet()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임정보의 고유값입니다.  

                userData = new UserData();

                userData.level = int.Parse(gameDataJson[0]["level"].ToString());
                userData.atk = float.Parse(gameDataJson[0]["atk"].ToString());
                userData.info = gameDataJson[0]["info"].ToString();

                foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                {
                    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                }

                foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                {
                    userData.equipment.Add(equip.ToString());
                }

                Debug.Log(userData.ToString());
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }
    }

    // 뒤끝 콘솔 테이블에서 유저 정보를 불러올 때 호출 
    public void GameDataLoad()
    {
        Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
        {
            // ���� ���� �ҷ����⿡ �������� ��
            if (callback.IsSuccess()) 
            {
                Debug.Log($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : { callback }");

                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    // �޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        // �ҷ��� ���� ������ ������
                        gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        // �ҷ��� ���� ������ userGameData ������ ����
                        userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        userGameData.experience = float.Parse(gameDataJson[0]["experience"].ToString());
                        userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                        userGameData.jewel = int.Parse(gameDataJson[0]["jewel"].ToString());
                        userGameData.heart = int.Parse(gameDataJson[0]["heart"].ToString());
                        userGameData.dailyBestScore = int.Parse(gameDataJson[0]["dailyBestScore"].ToString());

                        userGameData.jipsa_HP_Lv = int.Parse(gameDataJson[0]["jipsa_HP"].ToString());
                        userGameData.cat_HP_Lv = int.Parse(gameDataJson[0]["cat_HP"].ToString());
                        userGameData.cat_Speed_Lv = int.Parse(gameDataJson[0]["cat_Speed"].ToString());


                        onGameDataLoadEvent?.Invoke();
                    }
                }
                // JSON ������ �Ľ� ����
                catch (System.Exception e)
                {
                    // ���� ������ �ʱⰪ���� ����
                    userGameData.Reset();
                    // try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            // �������� ��
            else
            {
                Debug.LogError($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : {callback}");
            }
        });
    }


    //뒤끝 콘솔 테이블에 있는 유저 데이터 갱신 
    public void GameDataUpdate(UnityAction action=null)
    {
        if(userGameData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다" + "insert 혹은 Load를 통해 데이터를 생성해주세요. ");
            return;
        }

        Param param = new Param()
        {
            { "level", userGameData.level },
            { "experience", userGameData.experience },
            { "gold", userGameData.gold },
            { "jewel", userGameData.jewel },
            { "heart", userGameData.heart },
            { "dailyBestScore", userGameData.dailyBestScore },

            { "jipsa_HP_Lv", userGameData.jipsa_HP_Lv},
            { "cat_HP_Lv", userGameData.cat_HP_Lv},
            { "cat_Speed_Lv", userGameData.cat_Speed_Lv},


        };


        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.LogError("유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임 정도 데이터 수정을 요청합니다.");

            Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"게임 정보 데이터 수정에 성공했습니다. . : {callback}");

                    action?.Invoke();
                }
                else
                {
                    Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
                }
            });
        }



    }





}
