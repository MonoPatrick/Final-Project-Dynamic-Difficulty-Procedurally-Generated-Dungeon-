using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    private Player playerScript;
    [SerializeField] private Cooldown cooldown; // Reference to the Cooldown class for managing attack cooldowns
    private Rigidbody rb;
    //player movement
    private float moveHorizontal;
    private float moveForward;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GetComponent<Player>();
    }

    // Update is called once per frame
    Vector3 movement;


    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

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
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * playerScript.playerMovement;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.velocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.velocity = velocity;
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
