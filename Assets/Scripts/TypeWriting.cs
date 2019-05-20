using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriting : MonoBehaviour
{
    TextMeshProUGUI text;

    public float timePerCharacter;

    public float endDelay;

    public GameObject followUpObject;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        StartCoroutine(TypeWritingText());
    }

    IEnumerator TypeWritingText()
    {
        int characterCount = text.textInfo.characterCount;

        for (int i = 0; i <= characterCount; i++)
        {
            text.maxVisibleCharacters = i;

            yield return new WaitForSeconds(timePerCharacter);
        }

        text.maxVisibleCharacters = 100;

        if (followUpObject!=null)
        {
            yield return new WaitForSeconds(endDelay);

            followUpObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
