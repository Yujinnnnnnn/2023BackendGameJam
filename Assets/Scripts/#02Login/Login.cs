using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using System;

public class Login : LoginBase
{
    /// <summary>
    /// ID �ʵ� ���� ����
    /// </summary>
    [SerializeField]
    private Image imageID;            

    /// <summary>
    /// ID �ʵ� �ؽ�Ʈ ���� ����
    /// </summary>
    [SerializeField]
    private TMP_InputField inputFieldID;

    /// <summary>
    /// PW �ʵ� ���� ����
    /// </summary>
    [SerializeField]
    private Image imagePW;

    /// <summary>
    /// PW �ʵ� �ؽ�Ʈ ���� ����
    /// </summary>
    [SerializeField]
    private TMP_InputField inputFieldPW;

    /// <summary>
    /// �α��� ��ư (��ȣ�ۿ� ����/�Ұ���)
    /// </summary>
    [SerializeField]
    private Button btnLogin;

    /// <summary>
    /// "�α���" ��ư�� ������ �� ȣ��
    /// </summary>
    public void OnClickLogin()
    {
        // �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
        ResetUI(imageID, imagePW);

        // �ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;

        // �α��� ��ư�� ��Ÿ���� ���ϵ��� ��ȣ�ۿ� ��Ȱ��ȭ
        btnLogin.interactable = false;

        // ������ �α����� ��û�ϴ� ���� ȭ�鿡 ����ϴ� ���� ������Ʈ
        // ex) �α��� ���� �ؽ�Ʈ ���, ��Ϲ��� ������ ȸ�� ��
        StartCoroutine(nameof(LoginProcess));

        // �ڳ� ���� �α��� �õ�
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    /// <summary>
    /// �α��� �õ� �� �����κ��� ���޹��� message�� ������� ���� ó��
    /// </summary>
    private void ResponseToLogin(string ID, string PW)
    {
        // ������ �α��� ��û
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            // �α��� �õ��� �Ϸ�Ǹ� �ڷ�ƾ ����
            StopCoroutine(nameof(LoginProcess));

            // �α��� ����
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}�� ȯ���մϴ�.");

                // Lobby ������ �̵�
                Utils.LoadScene(SceneNames.Lobby);
            }
            // �α��� ����
            else 
            {
                // �α��ο� �������� ���� �ٽ� �α����� �ؾ��ϱ� ������ "�α���" ��ư ��ȣ�ۿ� Ȱ��ȭ
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401: // �������� �ʴ� ���̵�, �߸��� ��й�ȣ
                        message = callback.GetMessage().Contains("customId") ? "�������� �ʴ� ���̵��Դϴ�." : "�߸��� ��й�ȣ �Դϴ�.";
                        break;
                    case 403: // ���� or ����̽� ����
                        message = callback.GetMessage().Contains("user") ? "���ܴ��� �����Դϴ�." : "���ܴ��� ����̽� �Դϴ�.";
                        break;
                    case 410: // Ż�� ������
                        message = "Ż�� �������� �����Դϴ�.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                // StatusCode 401���� "�߸��� ��й�ȣ �Դϴ�." �� ��
                if (message.Contains("��й�ȣ"))
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
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
