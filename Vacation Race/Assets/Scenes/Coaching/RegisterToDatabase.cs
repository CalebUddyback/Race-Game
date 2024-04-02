using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterToDatabase : MonoBehaviour
{
    public InputField nameField, emailField, passwordField, confirmPassword;

    public Dropdown gender;



    Coroutine registeringCoroutine = null;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => Register());
    }

    void Register()
    {
        if (nameField.text == "")
            return;

        nameField.text[0].ToString().ToUpper();

        if (gender.value == 0)
            return;

        if (emailField.text == "")
            return;

        emailField.text.ToLower();

        if (passwordField.text == "")
            return;

        if (passwordField.text != confirmPassword.text)
            return;

        registeringCoroutine = StartCoroutine(Registering());
    }

    IEnumerator Registering()
    {

        WWWForm form = new WWWForm();

        form.AddField("name",       nameField.text);
        form.AddField("gender",     gender.options[gender.value].text);
        form.AddField("email",      emailField.text);
        form.AddField("password",   passwordField.text);

        WWW www = new WWW("http://localhost/sqlconnect/SignUp.php", form);

        yield return www;

        if (www.text == "0")
            Debug.Log("Racer Registered");
        else
            Debug.Log(www.text);
    }
}
