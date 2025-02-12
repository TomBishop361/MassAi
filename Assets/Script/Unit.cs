using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;

    

    private void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        UnitController.Instance.AddUnitToList(this);
        transform.position = new Vector3(Random.Range(0, 199), 1, Random.Range(0, 199));
    }
}
