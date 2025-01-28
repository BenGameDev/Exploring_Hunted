using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    public GameObject character;
    public float speed = 12f;
    public float dashSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public bool run;

    public Vector3 fowardDirection;

    public Transform groundCheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Animator animator;

    public Vector3 velocity;
    public bool isGrounded;
    private void Awake()
    {
        /*
         * speed = ;
         * dashSpeed = ;
         * gravity = ;
         * jumpHeight = ;
         * ground distance = ;
         * wallDistance = ;
         */
    }
    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
    }

    void Walk()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if ((x > 0 && run == false) || (z > 0 && run == false) || (x < 0 && run == false) || (z < 0 && run == false))
        {
            animator.SetBool("Walking", true);
        }
        else if (x == 0 && z == 0)
        {
            animator.SetBool("Walking", false);
        }

        Vector3 move = transform.right * x + transform.forward * z;
        if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero)
        {
            speed = 12f;
            run = true;
            animator.SetBool("Running", true);
            animator.SetBool("Walking", false);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("Running", false);
            speed = 5f;
            run = false;
        }

        if (move != Vector3.zero)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(move), 0.15F);
        }
        controller.Move(move * speed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (!isGrounded)
        {
            animator.SetBool("Jumping", false);
            speed = 4f;
        }

        if (isGrounded && velocity.y < 0 && run == false)
        {
            speed = 5f;
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("Jumping", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }


        velocity.y += gravity * (Time.deltaTime * 1.5f);
    }
}
