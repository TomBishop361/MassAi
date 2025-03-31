using System.Collections;
using Unity.Burst;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    Vector2 DirectionInput;
    public float speed;
    Camera cam;
    public CharacterController controller;
    public Animator animator;
    bool walking = false;
    public Vector3[] AttackArchPos = new Vector3[4];
    public LayerMask AttackMask;
    void OnMove(InputValue value)
    {
        DirectionInput = value.Get<Vector2>();
    }
    void OnAttack(InputValue value)
    {
        animator.SetTrigger("AttackEnd");
        animator.SetTrigger("Attack");
        StartCoroutine(AttackAnim());
    }

    void OnRun()
    {

    }

    IEnumerator AttackAnim()
    {   
        while(animator.GetCurrentAnimatorStateInfo(1).IsName("cav_shield_04_attack"))
        {
            yield return null;
            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.25f && !animator.IsInTransition(0))
            {
                //Damage();
            }
            if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animator.SetTrigger("AttackEnd");                
            }
        }
    }

    IEnumerator AttackArch()
    {
        yield return new WaitForSeconds(0.15f);
        Vector3 ArchPosA = (transform.position + (Vector3)DirFromAngle(-45, false) * 1.7f);
        Vector3 ArchPosB = (transform.position + (Vector3)DirFromAngle(45, false) * 1.7f);
        Vector3 ArchPosC = (transform.position + (Vector3)DirFromAngle(-20, false) * 1.7f);
        Vector3 ArchPosD = (transform.position + (Vector3)DirFromAngle(20, false) * 1.7f);
        Collider[] HitEnemies = new Collider[10];
        Physics.OverlapBoxNonAlloc(ArchPosA, Vector3.one * 0.85f, HitEnemies);
        Physics.OverlapBoxNonAlloc(ArchPosB, Vector3.one * 0.85f, HitEnemies);
        Physics.OverlapBoxNonAlloc(ArchPosC, Vector3.one * 0.85f, HitEnemies);
        Physics.OverlapBoxNonAlloc(ArchPosD, Vector3.one * 0.85f, HitEnemies);
        
        foreach(Collider c in HitEnemies)
        {
            if (c != null) Debug.Log(c.gameObject.name);
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
