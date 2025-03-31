using UnityEngine;

public class DetectionTriggerHandler : MonoBehaviour
{
    public Unit body;

    private void OnTriggerEnter(Collider other)
    {
        if(body.DetectionRange.Count < 5) body.DetectionRange.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (body.DetectionRange.Count > 0) body.DetectionRange.Remove(other.gameObject);
    }
}
