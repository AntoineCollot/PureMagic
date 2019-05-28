using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationData
{
    static Dictionary<TextFieldLocalization, string> texts;

    static LocalizationData()
    {
        LoadLanguageData("fr");
    }

    static void LoadLanguageData(string language)
    {
        //Load the texts
        string path = "Localization/Texts_" + language;
        Debug.Log("Loading Text from path : "+path);
        TextAsset allTexts = Resources.Load<TextAsset>(path);
        string content = allTexts.text;
        if (content == "")
            content = System.Text.Encoding.UTF8.GetString(allTexts.bytes);

        //Split by lines
        string[] lines = content.Split(new char[] { '\r', '\n' },System.StringSplitOptions.RemoveEmptyEntries);

        texts = new Dictionary<TextFieldLocalization, string>();
        foreach (string line in lines)
        {
            //Comments
            if (line[0] == '#')
                continue;

            //Split by ; since the file is a csv
            string[] fields = line.Split(';');

            //Add the field value to the dictionay
            texts.Add((TextFieldLocalization)System.Enum.Parse(typeof(TextFieldLocalization), fields[0]), fields[1]);
        }
    }

    public static string GetText(TextFieldLocalization field)
    {
        return texts[field];
    }
}

public enum TextFieldLocalization
{
    Button_TryAgain,
    Button_Continue,
    Button_Menu,
    Level_GameOver,
    Level_Cleared,
    Tuto_Grandpa_Main0,
    Tuto_Grandpa_Main1,
    Tuto_Grandpa_Main2,
    Tuto_Grandpa_Main3,
    Tuto_Grandpa_TooSoon0,
    Tuto_Grandpa_TooSoon1,
    Tuto_Grandpa_TooLate0,
    Tuto_Grandpa_TooLate1,
    Button_Beginner,
    Button_Veteran,
    Tuto_Grandpa_Gesture,
    Tuto_Grandpa_QuantityFailed,
    Tuto_Grandpa_QuantityMatters
}