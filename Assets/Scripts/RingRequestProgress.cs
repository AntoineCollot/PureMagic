using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingRequestProgress : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool cursorOnRight = Input.mousePosition.x > Screen.width * 0.5f;

        image.fillAmount = RequestsManager.Instance.GetRequestProgress(cursorOnRight);
    }
}
