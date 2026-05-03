using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int keys;
    public GameObject health;
    public GameObject coin;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Keys")
        {
            keys++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Chest")
        {
            int loot = UnityEngine.Random.Range(0, 2);
            if (loot == 0)
            {
                Instantiate(health, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            }

            else if (loot == 1)
            {
                Instantiate(coin, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
            }
            
            Destroy(collision.gameObject);
        }
    }
}
