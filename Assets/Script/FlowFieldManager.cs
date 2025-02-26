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

    private void createBuilding(Vector3 pos) {

        Instantiate(buildingPrefab, pos,quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {//change to place building at mouse position         

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, float.MaxValue);
            Debug.DrawRay(ray.origin, hit.point * 100);
            createBuilding(hit.point);   
            
        }
        //create 2nd input to select building and remove it 
    }


}
