using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameInputField : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] EditName editName;

    public void ChangeName()
    {
        editName.SetPlayerName(nameInputField.text);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
