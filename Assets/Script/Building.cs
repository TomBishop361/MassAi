using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BuiltBuilding))]
public class Building : MonoBehaviour
{
    // References to other components or managers
    public FlowFieldManager manager;
    
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
            
            
            Collider[] targetsInView = new Collider[20];
            int hitcount = Physics.OverlapSphereNonAlloc(transform.position, _BuildingData.ResourceRange,targetsInView, TargetResource);            
            
                foreach (Collider target in targetsInView)
                {
                    if (target != null)
                    {
                        if (!Physics.Raycast(transform.position, target.transform.position, Vector3.Distance(transform.position, target.transform.position), ObstacleMask))
                        {
                            ResourceTargets.Add(target.gameObject);
                          
                        }
                    }
                }
            
        }
    }

    void placeOnFlowField()
    {
        manager = FlowFieldManager.Instance;
        manager.AddSecondaryTarget(this.transform, _BuildingData.influence);
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
        Collider[] hit = new Collider[10];
        Physics.OverlapBoxNonAlloc(transform.position, boxCollider.size * 0.5f, hit, transform.rotation);
        if (hit.Length > 0)
        {
            foreach (Collider c in hit)
            {
                if (c != null)
                {


                    if (c.gameObject.CompareTag("Building") || c.gameObject.CompareTag("Resource"))
                    {
                        if (c != boxCollider && IsValidLocation)
                        {

                            IsValidLocation = false;
                            List<Material> materials = new List<Material>();
                            _Renderer.GetMaterials(materials);
                            materials[1].SetColor("_OutlineColour", new Color(1, 0, 0));
                        }
                        count++;
                    }

                }
                if (count == 1 && !IsValidLocation)
                {
                    List<Material> materials = new List<Material>();
                    _Renderer.GetMaterials(materials);
                    materials[1].SetColor("_OutlineColour", new Color(0, 1, 0));
                    IsValidLocation = true;
                }
            }
        }


    }

    public void buildComplete()
    {
        List<Material> materials = new List<Material>();
        _Renderer.GetMaterials(materials);
        materials[1].SetFloat("_IsBuilt", 1);
        isBuilt = true;
        
        placeOnFlowField();
        BuiltBuilding.construct(manager,_BuildingData,ResourceTargets, this);
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        boxCollider = GetComponent<BoxCollider>();  
        _Renderer = GetComponent<MeshRenderer>();
        
    }
#endif

}
