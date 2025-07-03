using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordUIMap1 : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInput; 
    [SerializeField] private Button submitButton; 
    [SerializeField] private TextMeshProUGUI errorText; 
    public string correctPassword; 
    public bool isPasswordCorrect = false;

    private void Start()
    {
        errorText.gameObject.SetActive(false);

        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        if (passwordInput.text == correctPassword)
        {
            errorText.gameObject.SetActive(false);
            isPasswordCorrect = true;
        }
        else
        {
            errorText.gameObject.SetActive(true);
            errorText.color = Color.red;
            errorText.text = "WRONG PASSWORD, TRY AGAIN!";
            isPasswordCorrect = false;
        }
    }

}
