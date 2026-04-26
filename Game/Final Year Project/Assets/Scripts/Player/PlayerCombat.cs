using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform[] attackPoint;
    public float weaponRange;
    public float knockbackForce = 50;
    public LayerMask enemylayer;
    public int damage = 1;
    public float knockbackTime = .15f;
    public float stunTime = 1.3f; 

    public void playerAttackUp()
    {


        Collider2D[] enemies= Physics2D.OverlapCircleAll(attackPoint[0].position, weaponRange, enemylayer);
        Debug.Log("Player Attacked Up");
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage);
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
        }
    }
    public void playerAttackDown()
    {
        Collider2D[]  enemies = Physics2D.OverlapCircleAll(attackPoint[1].position, weaponRange, enemylayer);
        Debug.Log("Player Attacked Down");
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage);
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
        }
    }
    public void playerAttackLeft()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint[2].position, weaponRange, enemylayer);
        Debug.Log("Player Attacked Left");
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage);
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
        }
    }
    public void playerAttackRight()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint[3].position, weaponRange, enemylayer);
        Debug.Log("Player Attacked Right");
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage);
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint[0].position, weaponRange);
        Gizmos.DrawWireSphere(attackPoint[1].position, weaponRange);
        Gizmos.DrawWireSphere(attackPoint[2].position, weaponRange);
        Gizmos.DrawWireSphere(attackPoint[3].position, weaponRange);
    }
}
