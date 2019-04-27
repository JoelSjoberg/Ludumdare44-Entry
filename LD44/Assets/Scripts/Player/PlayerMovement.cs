using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterController ch;
    float velocity_x = 0;
    Vector3 movement;
    Vector3 velocity;
    float trajectory = 0;

    [SerializeField]float gravity = -1f;
    [SerializeField]float max_speed = 5;

    [SerializeField]float altitude_decrement = 1f;
    [SerializeField] float jump_force;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;

    
    float current_jump_force = 0;

    bool jumped;

    void accelerate()
    {
        velocity_x += acceleration * Time.deltaTime;
        if (velocity_x > max_speed) velocity_x = max_speed;
        
    }
    void decellerate()
    {
        velocity_x -= decceleration * Time.deltaTime;
        if (velocity_x <= 0) velocity_x = 0;
    }

    void Awake()
    {
        movement = Vector3.zero;
        velocity = Vector3.zero;

        ch = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement on x-axis
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            accelerate();
            trajectory = Input.GetAxisRaw("Horizontal");
        }
        else decellerate();
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

        velocity.x = velocity_x * trajectory;



        
        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && !jumped) jump();
        if (Input.GetKeyUp(KeyCode.Space) && jumped) current_jump_force = current_jump_force * 0.7f;
        if (current_jump_force > 0) current_jump_force -= altitude_decrement * Time.deltaTime;

         
        // movement on y-axis
        velocity.y = (gravity + current_jump_force) * Time.deltaTime;

        /*
        if (velocity.magnitude > max_speed) velocity = velocity.normalized * max_speed;

        velocity.y += (current_jump_force) * Time.deltaTime;


        if (current_jump_force > 0) current_jump_force -= altitude_decrement * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && !jumped) jump();
        */
    }


    private void FixedUpdate()
    {
        ch.Move(velocity);
    }

    private void jump()
    {
        current_jump_force = jump_force;
        jumped = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Ground") jumped = false;
    }
}
