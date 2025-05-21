using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    FlowFieldManager manager;
    bool isBuilding;
    public GameObject[] BuildingPrefabs;

    GameObject SelectBuilding;

    GameObject CurrentBuildGO;
    Building CurrentBuilding;

    public GraphicRaycaster GRayCast;
    public EventSystem _eventSystem;
    PointerEventData m_PointerEventData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FlowFieldManager.Instance;
    }

    public void Build(int buildingIndx)
    {
        ClearCurrentBuilding();
        SelectBuilding = BuildingPrefabs[buildingIndx];
        CurrentBuildGO = Instantiate(SelectBuilding);
        CurrentBuilding = CurrentBuildGO.GetComponent<Building>();
        isBuilding = true;
    }

    private void createBuilding(Vector3 pos)
    {
        //GameObject Building =  Instantiate(SelectBuilding, pos, Quaternion.identity);
        CurrentBuildGO.transform.position = pos;
        CurrentBuilding.buildComplete();
        CurrentBuildGO = null;
        CurrentBuilding = null;
    }

    void ClearCurrentBuilding()
    {
        isBuilding = false;
        if (CurrentBuildGO != null)
        {
            Destroy(CurrentBuildGO);
            CurrentBuilding = null;
        }
    }

    private void Update()
    {

        if (isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentBuildGO.transform.eulerAngles += Vector3.up * 90;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            m_PointerEventData = new PointerEventData(_eventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            GRayCast.Raycast(m_PointerEventData, results);
            if (results.Count > 0) CurrentBuilding.IsValidLocation = false;

            Physics.Raycast(ray, out hit, float.MaxValue);
            Vector3 CellPos = manager.currentFlowField.GetCellFromWorldPos(hit.point).worldPos;

            CurrentBuildGO.transform.position = CellPos;

            if (Input.GetMouseButtonDown(0) && CurrentBuilding.IsValidLocation)
            {//change to place building at mouse position                                   
                createBuilding(CellPos);
                isBuilding = false;
            }


            //create 2nd input to select building and remove it 
            if (Input.GetMouseButtonDown(1))
            {
                ClearCurrentBuilding();
            }
        }
    }
}

