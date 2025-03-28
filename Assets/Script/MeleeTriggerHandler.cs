using System.Linq;
using UnityEngine;

public class MeleeTriggerHandler : MonoBehaviour
{
    public Unit body;

    private void OnTriggerEnter(Collider other)
    {
        
        if(body.MeleeRangeCheck.Count <= 5) body.MeleeRangeCheck.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (body.MeleeRangeCheck.Count > 0) body.MeleeRangeCheck.Remove(other.gameObject);
    }
}
