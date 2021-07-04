using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine.UI;

public class ErrorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI error;
    [SerializeField] private UIPopup errorPopUp;

    public void ShowError(string errorText)
    {
        error.text = errorText;
        errorPopUp.Show();
    }
}
