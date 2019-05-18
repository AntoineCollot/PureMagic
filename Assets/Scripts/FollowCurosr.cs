using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurosr : MonoBehaviour
{
    Camera cam;
    public float depth;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = depth;
        transform.position = cam.ScreenToWorldPoint(pos);
    }
}
