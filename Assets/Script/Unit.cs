using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour
{

    public NavMeshAgent agent;
    public UnitData unitData;
    public GameObject Target;
    public List<GameObject> MeleeRangeCheck;
    public List<GameObject> DetectionRange;    
    public bool Attacking = false;
    [SerializeField] Animator animator;
    public Health health;
    public int state = 0; // 0 = walking 1 = chasing 2 = attacking 3 = Dead
    

    public GameObject _Target { get { return _Target; } set { Target = value; if (value != null) { state = 2; Attack(); } } }
    delegate void AnimFinished();

    #region Attack Procedure

    //Initiate Attack procedure
    void Attack()
    {
        if (Target != null)
        {
            Attacking = true;
            animator.SetTrigger("Attack");
        }
    }

    //Attack Cooldown
    IEnumerator AttackTimer()
    {
        animator.SetTrigger("AttackEnd");
        yield return new WaitForSeconds(unitData.attackSpeed);
        Attacking = false;
        Attack();
    }

    //Called by Animation Event
    void ApplyDamage()
    {
        if (Target == null) { state = 0; return; }
        Target.GetComponent<Health>().AdjustHealth((int)-unitData.damage);
        StartCoroutine(AttackTimer());

    }

    #endregion

    public void Perish()
    {
        UnitController.Instance.RemoveUnitFromList(this);
        Destroy(gameObject);
    }


    //TO DO write logic to detect near enemies and target them
    // maybe only target player/ enemies if attacked
    //attack building if in melee range. only attack player/player ally in melee range if is targeted first (targeted due to being attacked by )
    public void AddMeleeTarget(GameObject targetObject)
    {
        if (Target == null)
        {
            _Target = targetObject;


        }
        MeleeRangeCheck.Add(targetObject);
    }


    public void RemoveMeleeTarget(GameObject targetObject)
    {
        if (Target == targetObject)
        {
            StopCoroutine(AttackTimer());
            _Target = null;
        }
        MeleeRangeCheck.Remove(targetObject);
        CheckForNextTarget();
    }

    public void CheckForNextTarget()
    {
        if (MeleeRangeCheck.Count > 0)
        {
            _Target = MeleeRangeCheck[0];
        }
        else
        {
            state = 0;
        }
    }

    private void Update()
    {

    }

    private void OnEnable()
    {
        health.healthDepleted += Perish;
    }
    private void OnDisable()
    {
        health.healthDepleted -= Perish;
        
    }

    //write detection lost countdown (after target leaves detection area start countdown before forgetting about it)

    private void Start()
    {

        UnitController.Instance.AddUnitToList(this);
        gameObject.SetActive(false);
        health._maxHealth = unitData.Health;

        

        MeleeRangeCheck = new List<GameObject>();
        DetectionRange = new List<GameObject>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
     health = GetComponent<Health>();   
        agent = GetComponent<NavMeshAgent>();   
    }
#endif  
}
