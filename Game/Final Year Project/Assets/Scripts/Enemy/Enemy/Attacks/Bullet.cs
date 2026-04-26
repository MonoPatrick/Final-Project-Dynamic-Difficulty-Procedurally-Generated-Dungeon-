using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); // get the mouse position in world space
        Vector3 direction = mousePos - transform.position; // calculate the direction from the bullet to the mouse position
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force; // set the velocity of the bullet in the direction of the mouse position
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg; // calculate the rotation of the bullet based on the direction to the mouse position
        transform.rotation = Quaternion.Euler(0f, 0f, rot + 90); // set the rotation of the bullet
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // if the bullet collides with an enemy, destroy the bullet and apply damage to the enemy
        {
            other.GetComponent<EnemyHealth>().ChangeHealth(-1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacles")) // if the bullet collides with an obstacle, destroy the bullet
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Walls")) // if the bullet collides with an obstacle, destroy the bullet
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
