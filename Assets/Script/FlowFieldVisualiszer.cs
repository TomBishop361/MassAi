using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FlowFieldVisualizer : MonoBehaviour
{
    public FlowFieldManager flowFieldManager;
    public FlowField flowField;
    public float arrowLength = 0.3f; // Length of the direction arrows
    public bool displayDirectionArrows = false;
   

    private void OnDrawGizmos()
    {
        if(!displayDirectionArrows) return;
        if (flowFieldManager.currentFlowField == null || flowFieldManager.currentFlowField.CurrentGrid == null) return;
        
        foreach (Cell cell in flowFieldManager.currentFlowField.CurrentGrid)
        {
            if (cell.bestDirection == GridDirection.None) continue;

            Vector3 startPos = cell.worldPos + Vector3.up * 0.1f; // Slightly above ground to avoid z-fighting
            Vector3 endPos = startPos + new Vector3(cell.bestDirection.x, 0, cell.bestDirection.y) * arrowLength;

            Debug.DrawLine(startPos, endPos, Color.green);

            // Draw arrowhead
            Vector3 right = Quaternion.Euler(0, 30, 0) * (startPos - endPos).normalized * 0.1f;
            Vector3 left = Quaternion.Euler(0, -30, 0) * (startPos - endPos).normalized * 0.1f;
            Debug.DrawLine(endPos, endPos + right, Color.green);
            Debug.DrawLine(endPos, endPos + left, Color.green);
        }
    }
}
