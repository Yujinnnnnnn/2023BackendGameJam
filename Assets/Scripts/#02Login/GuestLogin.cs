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
    /// �α��� ��ư (��ȣ�ۿ� ����/�Ұ���)
    /// </summary>
    [SerializeField]
    private Button btnLogin;

    [SerializeField]
    private Button btnBGLogin;

    public void Awake()
    {
        // Ȱ��ȭ���� ���� ���¿����� ������ ��� ����
        Application.runInBackground = true;

        // �ػ� ���� (9:18.5, 1440x2960)
        //int width = Screen.width;
        //int height = (int)(Screen.width * 18.5f / 9);
        //Screen.SetResolution(width, height, true);

        // ȭ���� ������ �ʵ��� ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }


    public void OnClickLogin()
    {
        // �α��� ��ư�� ��Ÿ���� ���ϵ��� ��ȣ�ۿ� ��Ȱ��ȭ
        btnLogin.interactable = false;
        btnBGLogin.interactable = false;

        // ������ �α����� ��û�ϴ� ���� ȭ�鿡 ����ϴ� ���� ������Ʈ
        // ex) �α��� ���� �ؽ�Ʈ ���, ��Ϲ��� ������ ȸ�� ��
        StartCoroutine(nameof(LoginProcess));

        // �ڳ� ���� �α��� �õ�
        ResponseToLogin();
    }

    private void ResponseToLogin()
    {
        Backend.BMember.GuestLogin("GuestLogin", callback =>
        {
            // �α��� �õ��� �Ϸ�Ǹ� �ڷ�ƾ ����
            StopCoroutine(nameof(LoginProcess));

            if (callback.IsSuccess())
            {
                SetMessage("�Խ�Ʈ �α��ο� �����߽��ϴ�");

                // Lobby ������ �̵�
                Utils.LoadScene(SceneNames.Lobby);
                //Utils.LoadScene("Scenes/Lobby");
            }
            else
            {
                SetMessage("�Խ�Ʈ �α��ο� �����߽��ϴ�");
            }
        });
    }

    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"�α��� ���Դϴ�. . . {time:F1}");

            yield return null;
        }
    }
}
