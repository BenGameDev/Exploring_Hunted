using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float dashSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Vector3 fowardDirection;

    public Transform groundCheck;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Animator animator;

    public Vector3 velocity;
    public bool isGrounded;
    public bool usedDash;
    public bool dashCruveDone;
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
        Dash();      
    }

    void Walk()
    { 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if(x > 0 || z > 0 || x < 0 || z < 0)
        {
            animator.SetBool("Walking", true);
        }
        else if(x == 0 && z == 0)
        {
            animator.SetBool("Walking", false);
        }

        Vector3 move = transform.right * x + transform.forward * z;

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

        if (isGrounded && velocity.y < 0)
        {
            speed = 10f;
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("Jumping", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        
        velocity.y += gravity * (Time.deltaTime * 1.5f);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && usedDash == false)
        {
            fowardDirection = transform.forward;
            usedDash = true;
            if (usedDash == true)
            {
                velocity = fowardDirection * dashSpeed;
                StartCoroutine(DashTime());
            }
        }
    }

    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(0.1f);
        velocity = fowardDirection * (dashSpeed / 2);
        yield return new WaitForSeconds(0.15f);
        usedDash = false;
        velocity.x = 0;
        velocity.z = 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (usedDash == false)
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }
    }


}
