using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public Image healthBar;
    private DynamicDifficultyAdjustment DDA;
    public bool debug;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = currentHealth;
        if (DDA == null)
        {
            DDA = GameObject.FindGameObjectWithTag("Difficulty").GetComponent<DynamicDifficultyAdjustment>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Projectile")
        {
            ChangeHealth(-12 * DDA.difficulty);
            //Debug.Log("Collided with an enemy!");
            // Perform action here
        }

    }
    public void ChangeHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            if (debug)
            {

            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
