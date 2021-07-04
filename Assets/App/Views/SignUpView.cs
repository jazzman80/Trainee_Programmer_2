using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignUpView : MonoBehaviour
{
    [SerializeField] ErrorManager errorManager;

    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    [SerializeField] private TMP_InputField confirm;

    public bool CheckError()
    {
        if (username.text == "")
        {
            errorManager.ShowError("����������, ������� ���");
            return true;
        }
        else if (email.text == "")
        {
            errorManager.ShowError("����������, ������� ����� ����������� �����");
            return true;
        }
        else if (password.text == "")
        {
            errorManager.ShowError("����������, ������� ������");
            return true;
        }
        else if (password.text != confirm.text)
        {
            errorManager.ShowError("����������, ����������� ������");
            return true;
        }
        else
        return false;
    }
}
