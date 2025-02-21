using Unity.Mathematics;
using UnityEngine;

public class FlowFieldManager : MonoBehaviour
{
    public static FlowFieldManager Instance;
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField currentFlowField;
    

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
        //InitializeFlowField();c
        currentFlowField = new FlowField(cellRadius, gridSize);
        
        currentFlowField.CreateMainGrid(new Vector3(100, 0, 100));
        
        
    }

    public void buildingDestroyed()
    {
        currentFlowField.removeSecondaryTarget();
    }

    private void createBuilding() {

        Instantiate(buildingPrefab, new Vector3((int)UnityEngine.Random.Range(0, 199),0, (int)UnityEngine.Random.Range(0, 199)),quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {//change to place building at mouse position
            createBuilding();
        }
        //create 2nd input to select building and remove it 
    }


}
