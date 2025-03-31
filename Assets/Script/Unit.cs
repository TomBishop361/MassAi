using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;    
    public GameObject demoTarget;
    public UnitData unitData;
    GameObject Target;
    public List<GameObject> MeleeRangeCheck;
    public List<GameObject> DetectionRange;
    public HealthBar healthBar;

    public int state = 0; // 0 = walking 1 = chasing 2 = attacking
    float currentHealth;

    public void AdjustHealh(int healthChange)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthChange, 0, unitData.Health);
        healthBar.setHealth((float)currentHealth / (float)unitData.Health);
        if (currentHealth < unitData.Health) healthBar.displayHealthBar();
        if (currentHealth == unitData.Health) healthBar.hideHealthBar();
    }

    //TO DO write logic to detect near enemies and target them
    // maybe only target player/ enemies if attacked
    //attack building if in melee range. only attack player/player ally in melee range if is targeted first (targeted due to being attacked by )

    private void Update()
    {
     if(MeleeRangeCheck.Count > 0)
        {
            state = 2;

        }   
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(unitData.attackSpeed);
    }

    //write detection lost countdown (after target leaves detection area start countdown before forgetting about it)

    private void Start()
    {
        UnitController.Instance.AddUnitToList(this);    
        gameObject.SetActive(false);
        currentHealth = unitData.Health;
        healthBar.gameObject.SetActive(false);

        MeleeRangeCheck = new List<GameObject>();
         DetectionRange = new List<GameObject>();
    }   

    
    

   
}
