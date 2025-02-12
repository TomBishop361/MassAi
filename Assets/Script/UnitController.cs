
using System.Collections.Generic;
using Unity.Burst;
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

    
    private void FixedUpdate()
    {
        if (manager.currentFlowField == null) {Debug.Log("Null"); return; }
        foreach (Unit unit in unitsInGame) {
            Cell nodeBelow = manager.currentFlowField.GetCellFromWorldPos(unit.transform.position);
            Vector3 moveDir = new Vector3(nodeBelow.bestDirection.Vector.x, 0, nodeBelow.bestDirection.Vector.y);
            unit.agent.Move(moveDir*0.15f);
            
            
        }
    }

}
