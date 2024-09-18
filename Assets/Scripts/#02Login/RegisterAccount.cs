using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class RegisterAccount : LoginBase
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
    /// Confirm PW �ʵ� ���� ����
    /// </summary>
    [SerializeField]
    private Image imageConfirmPW;
    /// <summary>
    /// Confirm PW �ʵ� �ؽ�Ʈ ���� ����
    /// </summary>
    [SerializeField]
    private TMP_InputField inputFieldConfirmPW;
    /// <summary>
    /// E-mail �ʵ� ���� ����
    /// </summary>
    [SerializeField]
    private Image imageEmail;
    /// <summary>
    /// E-mail �ʵ� �ؽ�Ʈ ���� ����
    /// </summary>
    [SerializeField]
    private TMP_InputField inputFieldEmail;

    /// <summary>
    /// "���� ����" ��ư (��ȣ�ۿ� ����/�Ұ���)
    /// </summary>
    [SerializeField]
    private Button btnRegisterAccount;

    /// <summary>
    /// "���� ����" ��ư�� ������ �� ȣ��
    /// </summary>
    public void OnClickRegisterAccount()
    {
        // �Ű������� �Է��� InputField UI�� ����� Message ���� �ʱ�ȭ
        ResetUI(imageID, imagePW, imageConfirmPW, imageEmail);

        // �ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;
        if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "��й�ȣ Ȯ��")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        // ��й�ȣ�� ��й�ȣ Ȯ���� ������ �ٸ� ��
        if (!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
        {
            GuideForIncorrectlyEnteredData(imageConfirmPW, "��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        // ���� ���� �˻�
        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageConfirmPW, "���� ������ �߸��Ǿ����ϴ�. (ex. address@xx.xx)");
            return;
        }

        // ���� ���� ��ư�� ��ȣ�ۿ� ��Ȱ��ȭ
        btnRegisterAccount.interactable = false;
        SetMessage("���� �������Դϴ�. .");

        // �ڳ� ���� ���� ���� �õ�
        CustomSignUp();
    }

    /// <summary>
    /// ���� ���� �õ� �� �����κ��� ���޹��� message�� ������� ���� ó��
    /// </summary>
    private void CustomSignUp()
    {
        Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
        {
            // "���� ����" ��ư ��ȣ�ۿ� Ȱ��ȭ
            btnRegisterAccount.interactable = true;

            // ���� ���� ����
            if (callback.IsSuccess())
            {
                // E-mail ���� ������Ʈ
                Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        SetMessage($"���� ���� ����. {inputFieldID.text}�� ȯ���մϴ�.");

                        // ���� ������ �������� �� �ش� ������ ���� ���� ����
                        BackendGameData.Instance.GameDataInsert();

                        // Lobby ������ �̵�
                        Utils.LoadScene(SceneNames.Lobby);
                    }
                });
            }
            // ���� ���� ����
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 409: // �ߺ��� customId�� �����ϴ� ���
                        message = "�̹� �����ϴ� ���̵��Դϴ�.";
                        break;
                    case 403: // ���ܴ��� ����̽��� ���
                    case 401: // ������Ʈ ���°� '����'�� ���
                    case 400: // ����̽� ������ null�� ���
                    default:
                        message = callback.GetMessage();
                        break;
                }

                if (message.Contains("���̵�"))
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}
