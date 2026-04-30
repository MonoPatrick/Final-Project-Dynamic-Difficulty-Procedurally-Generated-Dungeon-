using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    object OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerInventory>().keys > 0)
            {
                collision.gameObject.GetComponent<PlayerInventory>().keys--;
                Destroy(gameObject);
            }
        }
        return null;
    }
}
