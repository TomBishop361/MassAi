using Unity.Entities;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BuildingManager : MonoBehaviour
{
    FlowFieldManager manager;
    bool isBuilding;
    public GameObject[] BuildingPrefabs;

    GameObject SelectBuilding;

    GameObject CurrentBuildGO;
    Building CurrentBuilding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FlowFieldManager.Instance;
    }

    public void Build( int buildingIndx)
    {
        SelectBuilding = BuildingPrefabs[buildingIndx];
        CurrentBuildGO = Instantiate(SelectBuilding, Vector3.zero, Quaternion.identity);
        CurrentBuildGO.transform.eulerAngles += new Vector3(-90, 0, 0);
        CurrentBuilding = CurrentBuildGO.GetComponent<Building>();
        isBuilding = true;
    }

    private void createBuilding(Vector3 pos)
    {

        //GameObject Building =  Instantiate(SelectBuilding, pos, Quaternion.identity);
        CurrentBuildGO.transform.position = pos;
        CurrentBuilding.buildComplete();
    }


    private void Update()
    {

        if (isBuilding)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            Physics.Raycast(ray, out hit, float.MaxValue);
            Debug.DrawRay(ray.origin, hit.point * 100);
            Vector3 CellPos = manager.currentFlowField.GetCellFromWorldPos(hit.point).worldPos;
            CurrentBuildGO.transform.position = CellPos;            

            if (Input.GetMouseButtonDown(0) && CurrentBuilding.IsValidLocation)
            {//change to place building at mouse position                    

                createBuilding(hit.point);
                isBuilding = false;
            }
            
        }
        //create 2nd input to select building and remove it 
        if (Input.GetMouseButtonDown(1))
        {
            isBuilding = false ;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //Physics.Raycast(ray, out hit, float.MaxValue);
            //Debug.DrawRay(ray.origin, hit.point * 100);
            //if (hit.transform.gameObject.tag == "Building")
            //{
            //    buildingDestroyed(transform, hit.transform.gameObject.GetComponent<Building>().influence);
            //}

        }
    }
}
