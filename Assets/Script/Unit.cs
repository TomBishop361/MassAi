using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    int _seed =0;
    

    private void Awake()
    {
        if (_seed == 0)
        {
            _seed = Random.Range(0,int.MaxValue);
            Random.InitState(_seed);
        }
    }

    private void Start()
    {
        UnitController.Instance.AddUnitToList(this);
        transform.position = new Vector3(Random.Range(1, 199), 1, Random.Range(1, 199));
    }
}
