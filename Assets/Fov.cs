using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

public class Fov : MonoBehaviour
{
    public float viewRadius;
    [Range(0f,360f)]
    public float viewAngle;

    [BurstCompile]
    public float3 DirFromAngle(float angleInDeg, bool isGlobalAngle)
    {
        if (!isGlobalAngle) angleInDeg += transform.eulerAngles.y;

        return new float3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad),0,Mathf.Cos(angleInDeg* Mathf.Deg2Rad));
    }

   
}
