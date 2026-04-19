using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform Target;
    [SerializeField] NPC_States npc;
    [SerializeField] private float baseShootRate;
    private float shootRate;
    [SerializeField] private float baseProjectileMaxMoveSpeed;
    private float projectileMaxMoveSpeed;
    [SerializeField] private float projectileMaxHeight;
    [SerializeField] private AnimationCurve projectileSpeedAnimationCurve;
    private float shootTime;


    //animations
    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;

    private DynamicDifficultyAdjustment DDA;
    void Start()
    {
        shootRate = baseShootRate;
        projectileMaxMoveSpeed = baseProjectileMaxMoveSpeed;
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (DDA == null)
        {
            DDA = GameObject.FindGameObjectWithTag("Player").GetComponent<DynamicDifficultyAdjustment>();
        }
    }
    void Update()
    {
        difficultyAdjustment();
        if (npc.isAttacking)
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
    void difficultyAdjustment()
    {
        if (DDA == null) return;

        if (DDA.playerRank >= DynamicDifficultyAdjustment.Rank.Rank3)
        {
            shootRate = baseShootRate * DDA.difficulty;
            projectileMaxMoveSpeed = baseProjectileMaxMoveSpeed * DDA.difficulty;
        }
    }
}
