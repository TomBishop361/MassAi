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

    void OnMove(InputValue value)
    {
        DirectionInput = value.Get<Vector2>();
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
