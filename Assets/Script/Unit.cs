using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;    
    public GameObject demoTarget;

    GameObject Target;
    public List<GameObject> MeleeRangeCheck;
    public List<GameObject> DetectionRange;

    
    //TO DO write logic to detect near enemies and target them
    // maybe only target player/ enemies if attacked
    //attack building if in melee range. only attack player/player ally in melee range if is targeted first (targeted due to being attacked by )

    private void Update()
    {
     if(MeleeRangeCheck.Count > 0)
        {

        }   
    }

    private void Start()
    {
        UnitController.Instance.AddUnitToList(this);    
        gameObject.SetActive(false);

        MeleeRangeCheck = new List<GameObject>();
         DetectionRange = new List<GameObject>();
    }   

    
    

   
}
