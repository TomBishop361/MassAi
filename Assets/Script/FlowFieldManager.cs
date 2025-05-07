using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class FlowFieldManager : MonoBehaviour
{
    public static FlowFieldManager Instance;
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField currentFlowField;
    public bool isBuilding;
    

    public GameObject buildingPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    

   
    private void Start()
    {
        LoadFlowField();

    }
    public void AddSecondaryTarget(Transform pos, int influence)
    {
        currentFlowField.addSecondaryTarget((float3)pos.position, influence);
    }

    public void LoadFlowField()
    {        
        currentFlowField = new FlowField(cellRadius, gridSize);        
        currentFlowField.CreateMainGrid(new Vector3(74.17072f, 0, 78.00873f));
    }

    public void buildingDestroyed(Transform pos, int influence)
    {        
        currentFlowField.removeSecondaryTarget(pos.position,influence);
        
    }

   


   


}
