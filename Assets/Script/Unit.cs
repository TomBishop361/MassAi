using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    int _seed =0;
    public GameObject demoTarget;

    private void Awake()
    {
        //if (_seed == 0)
        //{
        //    _seed = Random.Range(0,int.MaxValue);
        //    Random.InitState(_seed);
        //}
    }
    

    private void Start()
    {
        UnitController.Instance.AddUnitToList(this);
      // transform.position = new Vector3(Random.Range(1, 50), 1, Random.Range(1, 50));
      gameObject.SetActive(false);
    }

    
    private void FixedUpdate()
    {
       transform.LookAt(demoTarget.transform.position);

        
    }

   
}
