using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchLanguageButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        UpdateText();
    }

    public void OnClick()
    {
        LocalizationData.SwitchLanguage();
        UpdateText();
    }

    public void UpdateText()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = LocalizationData.OtherLanguageName;
    }
}
