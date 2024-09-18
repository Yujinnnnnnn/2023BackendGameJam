using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using System;

public class GuestLogin : LoginBase
{   
    /// <summary>
    /// 로그인 버튼 (상호작용 가능/불가능)
    /// </summary>
    [SerializeField]
    private Button btnLogin;

    [SerializeField]
    private Button btnBGLogin;

    public void Awake()
    {
        // 활성화되지 않은 상태에서도 게임이 계속 진행
        Application.runInBackground = true;

        // 해상도 설정 (9:18.5, 1440x2960)
        //int width = Screen.width;
        //int height = (int)(Screen.width * 18.5f / 9);
        //Screen.SetResolution(width, height, true);

        // 화면이 꺼지지 않도록 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }


    public void OnClickLogin()
    {
        // 로그인 버튼을 연타하지 못하도록 상호작용 비활성화
        btnLogin.interactable = false;
        btnBGLogin.interactable = false;

        // 서버에 로그인을 요청하는 동안 화면에 출력하는 내용 업데이트
        // ex) 로그인 관련 텍스트 출력, 톱니바퀴 아이콘 회전 등
        StartCoroutine(nameof(LoginProcess));

        // 뒤끝 서버 로그인 시도
        ResponseToLogin();
    }

    private void ResponseToLogin()
    {
        Backend.BMember.GuestLogin("GuestLogin", callback =>
        {
            // 로그인 시도가 완료되면 코루틴 중지
            StopCoroutine(nameof(LoginProcess));

            if (callback.IsSuccess())
            {
                SetMessage("게스트 로그인에 성공했습니다");

                // Lobby 씬으로 이동
                Utils.LoadScene(SceneNames.Lobby);
                //Utils.LoadScene("Scenes/Lobby");
            }
            else
            {
                SetMessage("게스트 로그인에 실패했습니다");
            }
        });
    }

    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"로그인 중입니다. . . {time:F1}");

            yield return null;
        }
    }
}
