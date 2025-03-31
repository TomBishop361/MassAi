
using System.Collections.Generic;
using Unity.Mathematics;
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


    public void EnableEnemies()
    {
        foreach (Unit unit in unitsInGame)
        {
            unit.gameObject.SetActive(true);
        }
    }
   
        public static Vector3 FindClosestNavMeshPosition(Vector3 position, float maxSearchRadius = 2f)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxSearchRadius, NavMesh.AllAreas))
            {
                return hit.position; 
            }
            return position; 
        }

   

    //Maybe split into jobs or use intermittent thinkingsaw
    private void FixedUpdate()
    {
        if (manager.currentFlowField == null) {return; }
        foreach (Unit unit in unitsInGame) {
            if (!unit.gameObject.activeSelf) continue;
            if (unit.state != 0) continue;
            Cell nodeBelow = manager.currentFlowField.GetCellFromWorldPos(unit.transform.position);
            //if is pushed into obstacle then it will follow direction of neighbour 
            if (nodeBelow.bestDirection == GridDirection.None)
            {
                nodeBelow = manager.currentFlowField.findNearestDirection(nodeBelow);
            }
            Vector3 moveDir = new Vector3(nodeBelow.bestDirection.x, 0, nodeBelow.bestDirection.y);
            unit.agent.Move(moveDir* unit.unitData.moveSpeed);
            
            unit.transform.rotation = quaternion.LookRotation(moveDir, unit.transform.up);
        }
    }

}
