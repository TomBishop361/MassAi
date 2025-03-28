using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fov))]
public class FovEditor : Editor
{

    private void OnSceneGUI()
    {
        Fov fov = (Fov)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle*0.5f, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle * 0.5f, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);
    }
}
