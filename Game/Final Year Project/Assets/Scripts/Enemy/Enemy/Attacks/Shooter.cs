using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform Target;
    [SerializeField] NPC_States npc;
    [SerializeField] private float shootRate;
    [SerializeField] private float projectileMaxMoveSpeed;
    [SerializeField] private float projectileMaxHeight;
    [SerializeField] private AnimationCurve projectileSpeedAnimationCurve;
    private float shootTime;


    //animations
    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    void Start()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    void Update()
    {
        if(npc.isAttacking)
        {
            shootTime -= Time.deltaTime;  //once the shoot time is set to shootrate time.deltatime
                                          //till be subratacted until it reaches 0 which will
                                          //then instantiate the projectile

            if (shootTime <= 0)
            {

                shootTime = shootRate;
                Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>(); // create projectile at the transform position of gameobject connected to
                projectile.InitializeProjectile(Target, projectileMaxMoveSpeed, projectileMaxHeight);
                projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, projectileSpeedAnimationCurve);
            }
        }
        


    }
}
