using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginToDatabase : MonoBehaviour
{
    public InputField emailField, passwordField;

    public AutoLogin autologin;

    public Image colorTester;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => Login());
    }

    private void Start()
    {
        emailField.text = autologin.email;
    }

    void Login()
    {
        if (emailField.text == "")
            return;

        emailField.text.ToLower();

        if (passwordField.text == "")
            return;


        StartCoroutine(LoggingIn());
    }


    IEnumerator LoggingIn()
    {

        WWWForm form = new WWWForm();

        form.AddField("email", emailField.text);
        form.AddField("password", passwordField.text);

        WWW www = new WWW("http://localhost/sqlconnect/Login.php", form);

        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("Logged in");
            LoadAccountInfo(www.text);

        }
        else
            Debug.Log(www.text);

        // Then need to load all account racers into game from the database
    }

    public void LoadAccountInfo(string info)
    {
        DBManager.acountId = int.Parse(info.Split('\t')[1]);
        DBManager.email = info.Split('\t')[2];
        autologin.email = DBManager.email;

        StartCoroutine(LoadingRacers());
    }

    IEnumerator LoadingRacers()
    {
        WWWForm form = new WWWForm();

        form.AddField("id", DBManager.acountId);

        WWW www = new WWW("http://localhost/sqlconnect/ReturnRacers.php", form);

        yield return www;

        if (www.text[0] == '0')
        {
            Debug.Log("Racers Loaded");


            for (int r = 0; r < www.text.Split('\n').Length - 1; r++)
            {
                string row = www.text.Split('\n')[r];

                RacerInfo racer = new RacerInfo
                {
                    racerId = int.Parse(row.Split('\t')[1]),

                    _name = row.Split('\t')[2],

                    skin_Color = GetColorFromString(row.Split('\t')[3]),
                };

                colorTester.color = GetColorFromString(row.Split('\t')[3]);

                DBManager.racers.Add(racer);

            }

        }
        else
            Debug.Log(www.text);
    }

    private int HexToDec(string hex)
    {
        int dec = System.Convert.ToInt32(hex, 16);
        return dec;
    }

    private string DecToHex(int value)
    {
        return value.ToString("X2");
    }

    private string FloatNormalizedTOHex(float value)
    {
        return DecToHex(Mathf.RoundToInt(value * 255f));
    }

    private float HexToFloatNormalized(string hex)
    {
        return HexToDec(hex) / 255f;
    }

    private Color GetColorFromString(string hexString)
    {
        float red = HexToFloatNormalized(hexString.Substring(0, 2));
        float green = HexToFloatNormalized(hexString.Substring(2, 2));
        float blue = HexToFloatNormalized(hexString.Substring(4, 2));

        float alphas = 1f;

        if (hexString.Length >= 8)
        {
            alphas = HexToFloatNormalized(hexString.Substring(6, 2));
        }

        return new Color(red, green, blue);

    }

    private string GetStringFromColor(Color color, bool useAlpha = false)
    {
        string red = FloatNormalizedTOHex(color.r);
        string green = FloatNormalizedTOHex(color.g);
        string blue = FloatNormalizedTOHex(color.b);

        if (!useAlpha)
            return red + green + blue;
        else
        {
            string alpha = FloatNormalizedTOHex(color.a);
            return red + green + blue + alpha;
        }
    }
}
