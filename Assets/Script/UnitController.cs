
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public FlowFieldManager manager;
    public static UnitController Instance;
    public List<Unit> unitsInGame;


    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);

        unitsInGame = new List<Unit>();
    }

    public void AddUnitToList(Unit Unit) { 
        unitsInGame.Add(Unit);
    }

   
        public static Vector3 FindClosestNavMeshPosition(Vector3 position, float maxSearchRadius = 2f)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxSearchRadius, NavMesh.AllAreas))
            {
                return hit.position; 
            }
            return position; 
        }


    [BurstCompile]
    private void FixedUpdate()
    {
        if (manager.currentFlowField == null) {Debug.Log("Null"); return; }
        foreach (Unit unit in unitsInGame) {
            Cell nodeBelow = manager.currentFlowField.GetCellFromWorldPos(unit.transform.position);
            //if is pushed into obstacle then it will follow direction of neighbour 
            if (nodeBelow.bestDirection == GridDirection.None)
            {
                nodeBelow = manager.currentFlowField.findNearestDirection(nodeBelow);
            }
            Vector3 moveDir = new Vector3(nodeBelow.bestDirection.x, 0, nodeBelow.bestDirection.y);
            unit.agent.Move(moveDir*0.15f);

           
            
          
        }
    }

}
