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

            DynamicDifficultyAdjustment.Instance.changeDifficulty(0.07f);  
            FindObjectOfType<ScoreCount>().AddScore(1000);
            FindObjectOfType<ScoreCount>().AddEnemyKilled(1);

            int loot = UnityEngine.Random.Range(0, 3);
            if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank3 ||
               DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank4 ||
               DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
            {
                loot = UnityEngine.Random.Range(0, 7);
            }


            if (loot == 0 || loot == 4 || loot == 2)
            {
                Instantiate(health, transform.position, Quaternion.identity);
            }

            else if (loot == 1 || loot == 5 || loot == 6 || loot == 3)
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
