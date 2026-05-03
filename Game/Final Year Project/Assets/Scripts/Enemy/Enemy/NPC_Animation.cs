using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Animation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NPC_States npcStates;
    [SerializeField] private NPC_Controller enemyController;

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 movement = transform.position - lastPosition;
        bool isMoving = movement.magnitude > 0.01f;

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isAttacking", npcStates.isAttacking);


        if (npcStates.isAttacking && enemyController.player != null)
        {
            // Face player when attacking
            Vector3 attackDir = enemyController.player.position - transform.position;
            SetNPCDirection(attackDir);
        }
        else if (isMoving)
        {
            // Face movement direction when moving
            SetNPCDirection(movement);
        }

        lastPosition = transform.position;
        lastPosition = transform.position;

    }
    void SetNPCDirection(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                transform.localScale = scale;

                animator.SetFloat("moveX", -1f);
                animator.SetFloat("moveY", 0f);
            }
            else
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);
                transform.localScale = scale;

                animator.SetFloat("moveX", 1f);
                animator.SetFloat("moveY", 0f);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetFloat("moveX", 0f);
                animator.SetFloat("moveY", 1f);
            }
            else
            {
                animator.SetFloat("moveX", 0f);
                animator.SetFloat("moveY", -1f);
            }
        }
    }
}
