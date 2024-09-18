using UnityEngine;
using BackEnd;

public class DailyRankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        //UpdateMyRankData(newScore); // 가장 최근 점수를 업데이트 함
        UpdateMyBestRankData(newScore); // 가장 높은 점수를 업데이트. 
    }

    private void UpdateMyRankData(int newScore)
    {
        string rowInDate = string.Empty;

        //랭킹 데이터를 업데이트 하려면 게임 데이터에서 사용하는 데이터의 inDate 값이 필.
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"데이터 조회 중 문제가 발생했습니. : {callback}");
                return;
            }

            Debug.Log($"데이터 조회 성공했습니다. : {callback}");
            if (callback.FlattenRows().Count > 0)
            {
                rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("데이터가 존재하지 않습니다.");
                return;
            }

            Param param = new Param()
            {
                { "dailyBestScore", newScore }
            };

            Backend.URank.User.UpdateUserScore(Constants.DAILY_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
            {

                if(callback.IsSuccess())
                {
                    Debug.Log($"랭킹 등록에 성공했습니다 . : {callback}");
                }else
                {
                    Debug.LogError($"랭킹 등록에 실패했습니. :{callback}");
                }






            });
        });

    }
    private void UpdateMyBestRankData(int newScore)
    {
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback =>
        {
            if(callback.IsSuccess())
            {
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if(rankDataJson.Count <=0)
                    {

                        Debug.LogWarning("데이터가 존재하지 않습니다. ");

                    }
                    else
                    {
                        int bestScore = int.Parse(rankDataJson[0]["score"].ToString());
                        if (newScore > bestScore) // 현재 점수가 최고 점수보다 높으면  
                        {
                            // ���� ������ ���ο� �ְ� ������ �����ϰ�, ��ŷ�� ���
                            UpdateMyRankData(newScore); // 현재 점수를 최고 점수로 설정하고 , 랭킹에 등록  

                            Debug.Log($"�ְ� ���� ���� {bestScore} -> {newScore}");
                        }

                    }

                }
                catch (System.Exception e) // json 데이터 파싱 실패ㅣ 
                {
                    // try-catch 에러 출력 
                    Debug.LogError(e);
                }

            }
            else
            {
                // 자신의 랭킹 정보가 존재하지 않을 때는 현재 점수를 새로운 랭킹으로 등록 
                if (callback.GetMessage().Contains("userRank"))
                {
                    UpdateMyRankData(newScore);

                    Debug.Log($"새로운 랭킹 데이터 생성 및 등록: {callback}");
                }



            }


        });






    }


}
