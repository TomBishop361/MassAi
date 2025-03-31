using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    Quaternion rotation;

    void Update()
    {
        if(transform.rotation != rotation)
        {
            rotation = transform.rotation;
            transform.eulerAngles = new Vector3(0, 50, 0);
        }
    }
}
