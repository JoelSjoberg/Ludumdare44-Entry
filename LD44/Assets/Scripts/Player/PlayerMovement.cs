using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterController ch;
    float velocity_x = 0;
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



    [SerializeField] float lowerBound_jumpForce = 0;

    [SerializeField] float attack_force = 1;
    bool invulnerable = false;
   

    [SerializeField]float attack_speed = 1;

    Health health;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator anim;

    void accelerate(float acceleration, float max_speed)
    {
        velocity_x += acceleration * Time.deltaTime;
        if (velocity_x > max_speed) velocity_x = max_speed;
        
    }
    void decellerate(float decceleration)
    {
        velocity_x -= decceleration * Time.deltaTime;
        if (velocity_x <= 0) velocity_x = 0;
    }

    IEnumerator attack()
    {
        health.dangerous = true;
        trajectory = Input.GetAxisRaw("Horizontal");
        // Speed up
        while (Mathf.Abs(velocity.x) < attack_speed)
        {
            accelerate(acceleration * 11f, attack_speed);
            velocity = new Vector3(velocity_x * trajectory, 0, 0);
            yield return null;
        }

        // Slow down
        while (Mathf.Abs(velocity.x) > 0.1f)
        {
            decellerate(decceleration * 13);
            velocity = new Vector3(velocity_x * trajectory, 0, 0);
            
            yield return null;
        }
        velocity = Vector3.zero;
        health.dangerous = false;

        yield return 0;
    }

    // slow down for a moment when hit
    public void stun()
    {
        startShake(this.transform);
    }

    [SerializeField] float time = 0.1f, intensity = 0.08f;

    public void startShake(Transform t)
    {
        StartCoroutine(shake(time, intensity, t));
    }
    IEnumerator shake(float t, float intensity, Transform transform, float timer = 0)
    {

        Vector3 origin = transform.position;

        while (timer < t)
        {
            transform.position = origin;
            timer += Time.deltaTime;
            transform.position += (Vector3)Random.insideUnitCircle * intensity;
            yield return null;
        }
        transform.position = origin;
    }

    void Awake()
    {
        velocity = Vector3.zero;

        ch = GetComponent<CharacterController>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity.x < 0) sr.flipX = true;
        else sr.flipX = false;

            // Attacking, should take priority over movement
            if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0 && !health.dangerous && !health.dead)
            {
            audioManager.playSound("attack");
                anim.SetTrigger("attack");
                StartCoroutine(attack());
            }
            else if (!health.dangerous && !health.dead)
            {
                // Movement on x-axis
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    accelerate(acceleration, max_speed);
                    trajectory = Input.GetAxisRaw("Horizontal");
                }
                else decellerate(decceleration);

                velocity.x = velocity_x * trajectory;

                // Jumping
                if (Input.GetKeyDown(KeyCode.Space) && !jumped) jump();
                if (Input.GetKeyUp(KeyCode.Space) && jumped) current_jump_force = current_jump_force * 0.7f;
                if (current_jump_force > lowerBound_jumpForce) current_jump_force -= altitude_decrement * Time.deltaTime;

                // movement on y-axis
                velocity.y = (gravity + current_jump_force) * Time.deltaTime;
            }
    }

    private void FixedUpdate()
    {
        if (!health.dead)
        {
            ch.Move(velocity);
        }
        else anim.SetTrigger("dead");

    }

    private void jump()
    {
        audioManager.playSound("jump");
        current_jump_force = jump_force;
        jumped = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Ground" && jumped)
        {
            jumped = false;
            audioManager.playSound("land");
        }
    }

    // It is too hard to attack enemies right now, use this trigger to deal damage to them
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && health.dangerous)
        {
            other.GetComponent<Health>().takeDamage();
            health.heal();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && health.dangerous)
        {
            other.GetComponent<Health>().takeDamage();
            health.heal();
        }
    }
}
