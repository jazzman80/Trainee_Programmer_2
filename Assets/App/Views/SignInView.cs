using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignInView : MonoBehaviour
{
    [SerializeField] private ErrorManager errorManager;

    public TMP_InputField username;
    public TMP_InputField password;

    public bool CheckError()
    {
        if (username.text == "")
        {
            errorManager.ShowError("����������, ������� ���");
            return true;
        }
        else if (password.text == "")
        {
            errorManager.ShowError("����������, ������� ������");
            return true;
        }
        else
        return false;
    }
}
