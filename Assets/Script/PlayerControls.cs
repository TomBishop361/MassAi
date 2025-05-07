using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[BurstCompile]
public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    Vector2 DirectionInput;
    public float speed;
    Camera cam;
    public CharacterController controller;
    public Animator animator;
    bool walking = false;
    bool attacking = false;
    public LayerMask AttackMask;

    void OnMove(InputValue value)
    {
        DirectionInput = value.Get<Vector2>();
    }
    void OnAttack(InputValue value)
    {
        if (!attacking)
        {
            animator.SetTrigger("AttackEnd");
            animator.SetTrigger("Attack");
            StartCoroutine(AttackAnim());
        }
    }

    void OnRun()
    {

    }

  
    IEnumerator AttackAnim()
    {
        attacking = true;
        AttackArch();
        while(animator.GetCurrentAnimatorStateInfo(1).IsName("cav_shield_04_attack"))
        {
            
            yield return null;
            
            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime == 1 && !animator.IsInTransition(0))
            {
                animator.SetTrigger("AttackEnd");       
                
            }
        }
        attacking = false;
    }

#if UNITY_EDITOR
    public bool DisplayArchDebug = false;
    private void OnDrawGizmos()
    {
        if (DisplayArchDebug)
        {
            Vector3 ArchPosA = (transform.position + (Vector3)DirFromAngle(-45, false) * 1.7f);
            Vector3 ArchPosB = (transform.position + (Vector3)DirFromAngle(45, false) * 1.7f);
            Vector3 ArchPosC = (transform.position + (Vector3)DirFromAngle(-20, false) * 1.7f);
            Vector3 ArchPosD = (transform.position + (Vector3)DirFromAngle(20, false) * 1.7f);

            Gizmos.DrawSphere(ArchPosA, 0.7f);
            Gizmos.DrawSphere(ArchPosB, 0.7f);
            Gizmos.DrawSphere(ArchPosC, 0.7f);
            Gizmos.DrawSphere(ArchPosD, 0.7f);
        }
    }
#endif

    public void AttackArch()
    {        
        Vector3 ArchPosA = (transform.position + (Vector3)DirFromAngle(-45, false) * 1.7f);
        Vector3 ArchPosB = (transform.position + (Vector3)DirFromAngle(45, false) * 1.7f);
        Vector3 ArchPosC = (transform.position + (Vector3)DirFromAngle(-20, false) * 1.7f);
        Vector3 ArchPosD = (transform.position + (Vector3)DirFromAngle(20, false) * 1.7f);
        List<GameObject> enemiesHit = new List<GameObject>();
        Collider[] HitEnemies = new Collider[5];
        
        Physics.OverlapSphereNonAlloc(ArchPosA, 1.7f, HitEnemies,AttackMask);
        enemiesHit.AddAllCollidersToList(HitEnemies); 
        Physics.OverlapSphereNonAlloc(ArchPosB, 1.7f, HitEnemies, AttackMask);
        enemiesHit.AddAllCollidersToList(HitEnemies);
        Physics.OverlapSphereNonAlloc(ArchPosC, 1.7f, HitEnemies, AttackMask);
        enemiesHit.AddAllCollidersToList(HitEnemies);
        Physics.OverlapSphereNonAlloc(ArchPosD, 1.7f, HitEnemies, AttackMask);
        enemiesHit.AddAllCollidersToList(HitEnemies);
        
        ApplyDamage(enemiesHit);
    }  

    public void ApplyDamage(List<GameObject> HitTargets)
    {
        
        foreach (GameObject Target in HitTargets)
        {
           // Debug.Log(Target);
            Target.SendMessage("AdjustHealth", -2,SendMessageOptions.DontRequireReceiver);
        }
    }

    [BurstCompile]
    public float3 DirFromAngle(float angleInDeg, bool isGlobalAngle)
    {
        if (!isGlobalAngle) angleInDeg += transform.eulerAngles.y;

        return new float3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if ((DirectionInput != Vector2.zero && !walking))
        {
            animator.SetTrigger("Walking");
            walking = true;
        }else if ((DirectionInput == Vector2.zero && walking))
        {
            animator.SetTrigger("Idle");
            walking = false;
        }
        Move();
       
    }

    

    private void Move()
    {
        Vector3 move = (cam.transform.right * DirectionInput.x) + (cam.transform.forward * DirectionInput.y);
        move.y = 0f;
        controller.Move(move * speed * Time.deltaTime);

       if(move != Vector3.zero) rotate(move);
    }

    void rotate(Vector3 move)
    {
        transform.rotation = Quaternion.LookRotation(move, transform.up);
        
    }
}

