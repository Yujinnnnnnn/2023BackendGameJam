using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] private UnityEvent OnGameOver; // 게임오버 되었을 때 호출할 메소드 등록 및 실행
    [SerializeField] private DailyRankRegister dailyRank;


    //private int score = 0;
    public bool IsGameOver { set; get; } = false;

    public void GameOver(int score)
    {
   
        if (IsGameOver == true) return; // 중복처리 되지 않도록 bool 변수 

        IsGameOver = true;

        OnGameOver.Invoke(); // 게임오버 되었을때 호출할 메소드들을 실행

        dailyRank.Process(score); // 현재 점수 정보를 바탕으로 랭킹 데이터 갱신 



        // 경험치 증가 및 레벨업 여부 검사

        BackendGameData.Instance.UserGameData.experience += score; // # 점수 추가
        if (BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            //BackendGameData.Instance.UserGameData.level++;
            //일단 레벨 사용안하니까 주석 처리해둠 
        }

        // ���� ���� ������Ʈ
        BackendGameData.Instance.GameDataUpdate(AfterGameOver);
    }

    public void AfterGameOver()
    {
        //로비 씬으로 이동 
        //Utils.LoadScene(SceneNames.Lobby);
    }

}
