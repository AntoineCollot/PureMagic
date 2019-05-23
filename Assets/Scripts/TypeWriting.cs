using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriting : MonoBehaviour
{
    TextMeshProUGUI text;

    public float timePerCharacter;
    public float startDelay;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        StartCoroutine(TypeWritingText());
    }

    IEnumerator TypeWritingText()
    {
        int characterCount = text.textInfo.characterCount;
        text.maxVisibleCharacters = 0;

        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i <= characterCount; i++)
        {
            text.maxVisibleCharacters = i;

            yield return new WaitForSeconds(timePerCharacter);
        }

        text.maxVisibleCharacters = 100;
    }
}
