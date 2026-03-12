using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    private Player playerScript;
    public GameObject gameObject;
    [SerializeField] private Cooldown cooldown; // Reference to the Cooldown class for managing attack cooldowns
    private Rigidbody2D rb;
    //player movement
    private float moveHorizontal;
    private float moveVertical;
    private Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        playerScript = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    Vector3 movement;


    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.F))
        {

            attack();

        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    

    void MovePlayer()
    {
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        Vector2 targetVelocity = movement * playerScript.playerMovement;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.velocity;

        velocity.x = targetVelocity.x;
        velocity.y = targetVelocity.y;
        rb.velocity = velocity;
        if (velocity.x > 0.01f)
        {
            // Right
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;

            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetFloat("Movement", 1f);
        }
        else if (velocity.x < -0.01f)
        {
            // Left
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;

            animator.SetBool("Left", true);
            animator.SetBool("Right", true);
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
            animator.SetFloat("Movement", 1f);
        }
        else if (velocity.y > 0.01f)
        {
            // Up
            animator.SetBool("Front", true);
            animator.SetBool("Back", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            animator.SetFloat("Movement", 1f);
        }
        else if (velocity.y < -0.01f)
        {
            // Down
            animator.SetBool("Back", true);
            animator.SetBool("Front", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            animator.SetFloat("Movement", 1f);
        }

        if(velocity.x == 0 && velocity.y == 0 )
        {
            
            animator.SetFloat("Movement", 0f);
        }
        Debug.Log(velocity.x);

            
        
        

    }
    void attack()
    {
        if(cooldown.isOnCooldown)
        {

            Debug.Log("Attack is on cooldown!");
            return;
            
        }
        Debug.Log("Player Attacked!");
        cooldown.StartCooldown();
    }
}
