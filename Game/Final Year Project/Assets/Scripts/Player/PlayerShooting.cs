using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public Player playerScript;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); // get the mouse position in world space

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        if (playerScript.direction == Player.playerDirection.Left)
        {
            Vector3 scale = transform.localScale; // if the player is facing left, set the scale to negative to ensure the gun is facing the correct direction
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (playerScript.direction == Player.playerDirection.Right)
        {

            Vector3 scale = transform.localScale;// if the player is facing right, set the scale to positive to ensure the gun is facing the correct direction
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenFiring)
            {
                canFire = true; // reset the timer and allow firing again
            }
        }

        if (Input.GetMouseButton(1) && canFire) // right click to shoot
        {
            Instantiate(bullet, bulletTransform.position, transform.rotation);// spawn the bullet at the position of the gun and with the same rotation as the player
            canFire = false; // reset the timer and prevent firing until the cooldown is over
            timer = 0;
        }
        


    }
}
