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

    Vector2 movement;
    Vector2 targetVelocity;

    // Apply movement to the Rigidbody
    Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        playerScript = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }



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
        movement = new Vector2(moveHorizontal, moveVertical).normalized;
        targetVelocity = movement * playerScript.playerMovement;

        // Apply movement to the Rigidbody
        velocity = rb.velocity;

        velocity.x = targetVelocity.x;
        velocity.y = targetVelocity.y;
        rb.velocity = velocity;

        if (velocity.x > -0.01f)
        {
            // Left
            
        }
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);

        if (velocity != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        



        findPlayerDirection();
        setPlayerDirection();



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
    void findPlayerDirection()
    {

        if (movement.x == 0 && movement.y > 0)
        {

            playerScript.direction = Player.playerDirection.Up;
            Debug.Log(playerScript.direction);
        }
        else if (movement.x == 0 && movement.y < 0)
        {
            playerScript.direction = Player.playerDirection.Down;
            Debug.Log(playerScript.direction);
        }
        else if (movement.y == 0 && movement.x < 0)
        {

            playerScript.direction = Player.playerDirection.Left;
            Debug.Log(playerScript.direction);
        }
        else if (movement.y == 0 && movement.x > 0)
        {

            playerScript.direction = Player.playerDirection.Right;
            Debug.Log(playerScript.direction);
        }

    }
    void setPlayerDirection()
    {
        if (playerScript.direction == Player.playerDirection.Up)
        {

            animator.SetFloat("moveY", 1f);
            animator.SetFloat("moveX", 0f);
        }
        else if (playerScript.direction == Player.playerDirection.Down)
        {
            animator.SetFloat("moveY", -1f);
            animator.SetFloat("moveX", 0f);
        }
        else if (playerScript.direction == Player.playerDirection.Left)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;

            animator.SetFloat("moveX", -1f);
            animator.SetFloat("moveY", 0f);
        }
        else if (playerScript.direction == Player.playerDirection.Right)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;

            animator.SetFloat("moveX", 1f);
            animator.SetFloat("moveY", 0f);
        }
    }
}
