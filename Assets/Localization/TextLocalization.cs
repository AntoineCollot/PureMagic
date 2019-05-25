using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLocalization : MonoBehaviour
{

    public TextFieldLocalization field;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = LocalizationData.GetText(field);
    }
}
