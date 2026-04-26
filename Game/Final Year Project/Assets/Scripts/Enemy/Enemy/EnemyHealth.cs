using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;
    void Start()
    {
        curHealth = maxHealth;
    }


    public void ChangeHealth(int amount)
    {
        curHealth += amount;

        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
        else if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
