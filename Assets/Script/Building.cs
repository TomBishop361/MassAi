using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(BuiltBuilding))]
public class Building : MonoBehaviour
{
    // References to other components or managers
    public FlowFieldManager manager;
    public HealthBar healthBar;
    public BoxCollider boxCollider;
    public MeshRenderer _Renderer;
    BuiltBuilding BuiltBuilding;
    // Building-specific data
    public BuildingData _BuildingData;
    public LayerMask TargetResource;
    public LayerMask ObstacleMask;

    // State tracking variables
    public bool preplacedBuilding;
    public bool IsValidLocation = true;
    bool isBuilt = false;
    List<GameObject> ResourceTargets = new List<GameObject>();

    // Health-related variables
    public Vector3 PositionLastFrame;

    // FlowField settings
    public int influence = 5;




    private void Start()
    {
        BuiltBuilding = GetComponent<BuiltBuilding>();
        if (_BuildingData.Produce != BuildingData.Produces.None)
        {
            ObstacleMask = LayerMask.GetMask("Mountain");
            TargetResource = LayerMask.GetMask($"{_BuildingData.Produce}");
            
        }
        if (preplacedBuilding)
        {
            ResourceAreaCheck();
            buildComplete();
        }

    }

    void ResourceAreaCheck() 
    {
        ResourceTargets.Clear();
        if (PositionLastFrame != transform.position)
        {
            PositionLastFrame = transform.position;
            
            Debug.Log("Here");
            
            Collider[] targetsInView = Physics.OverlapSphere(transform.position, _BuildingData.ResourceRange, TargetResource);
            foreach (Collider target in targetsInView)
            {
                if (!Physics.Raycast(transform.position, target.transform.position, Vector3.Distance(transform.position, target.transform.position), ObstacleMask))
                {
                    ResourceTargets.Add(target.gameObject);
                }
            }
        }
    }

    void placeOnFlowField()
    {
        manager = FlowFieldManager.Instance;
        manager.AddSecondaryTarget(this.transform, influence);
    }
   

    private void FixedUpdate()
    {
        if (transform.position != PositionLastFrame) {
            ResourceAreaCheck();
        }
        if (!isBuilt)
        { 
            SpawnValidation();
        }
    }


    public void SpawnValidation()
    {
         
            int count = 0;
            Collider[] hit = Physics.OverlapBox(transform.position, boxCollider.size * 0.5f,transform.rotation);
            if (hit.Length > 0)
            {
                foreach (Collider c in hit)
                {
                    if (c.gameObject.CompareTag("Building") || c.gameObject.CompareTag("Resource"))
                    {
                        if (c != boxCollider && IsValidLocation)
                        {

                            IsValidLocation = false;
                            _Renderer.materials[1].SetColor("_OutlineColour", new Color(1, 0, 0));
                        }
                        count++;
                    }                  
                    
                }
                if (count == 1 && !IsValidLocation)
                {
                    _Renderer.materials[1].SetColor("_OutlineColour", new Color(0, 1, 0));
                    IsValidLocation = true;
                }
            }
            
        
    }

    public void buildComplete()
    {
        _Renderer.materials[1].SetFloat("_IsBuilt", 1);
        isBuilt = true;
        healthBar.hideHealthBar();
        placeOnFlowField();
        BuiltBuilding.construct(manager,healthBar,_BuildingData,ResourceTargets, this);
    }

    
}
