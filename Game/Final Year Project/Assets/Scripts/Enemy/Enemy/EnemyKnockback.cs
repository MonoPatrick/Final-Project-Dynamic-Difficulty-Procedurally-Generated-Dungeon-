using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private NPC_Controller enemyMovement;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<NPC_Controller>();
    }


    public void Knockback(Transform playerTransform, float knockbackforce, float knockbackTime, float stunTime)
    {
        enemyMovement.currentState = NPC_Controller.StateMachine.Knockback;
        StartCoroutine(StunTimer(knockbackTime ,stunTime));
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackforce;
        Debug.Log("Knockback applied");
    }
    IEnumerator StunTimer(float knockback, float stunTime)
    {
        yield return new WaitForSeconds(knockback);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemyMovement.currentState = NPC_Controller.StateMachine.Patrol;

    }
}
