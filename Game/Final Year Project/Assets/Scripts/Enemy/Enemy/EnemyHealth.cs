using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;
    public GameObject health;
    public GameObject coin;
    public GameObject popUpPrefab;
    void Start()
    {
        curHealth = maxHealth;
    }


    public void ChangeHealth(int amount)
    {
        GameObject popUp = Instantiate(popUpPrefab, transform.position, Quaternion.identity);
        popUp.GetComponentInChildren<TMP_Text>().text = amount.ToString();
        curHealth += amount;

        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
        else if (curHealth <= 0)
        {
            DynamicDifficultyAdjustment.Instance.changeDifficulty(0.05f);  
            FindObjectOfType<ScoreCount>().AddScore(1000);
            int loot = UnityEngine.Random.Range(0, 3);
            if (loot == 0)
            {
                Instantiate(health, transform.position, Quaternion.identity);
            }
            if (loot == 1)
            {
                Instantiate(coin, transform.position, Quaternion.identity);
            }
            else
            {
                // No loot dropped
            }


            Destroy(gameObject);
        }
    }
}
