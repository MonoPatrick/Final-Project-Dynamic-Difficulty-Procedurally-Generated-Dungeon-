using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVisuals : MonoBehaviour
{
    [SerializeField] private Transform projectileVisual;
    [SerializeField] private Transform projectileShadow;
    [SerializeField] private Projectile projectile;

    private Transform target;
    private Vector3 trajectoryStartPosition;

    private float shadowPositionYDivider = 6f;
    private void Start()
    {
        trajectoryStartPosition = transform.position;

    }
    private void Update()
    {
        UpdateProjectileRotation();
        UpdateShadowPosition();

        float trajectoryProgressMagnitude = trajectoryStartPosition.magnitude;
        float trajectoryMagnitude = (target.position - trajectoryStartPosition).magnitude;

        float trajectoryProgressNormalized = trajectoryProgressMagnitude / trajectoryMagnitude;

        if(trajectoryProgressNormalized < .7f)
        {
            UpdateShadowProjectileRotation();
        }


    }
    private void UpdateShadowPosition()
    {
        Vector3 newPosition = transform.position;
        Vector3 trajectoryRange = target.position - trajectoryStartPosition;

        if(Mathf.Abs(trajectoryRange.normalized.x) < Mathf.Abs(trajectoryRange.normalized.y)) 
        {
            newPosition.x = trajectoryStartPosition.x + projectile.GetNextXTrajectoryPosition() / shadowPositionYDivider + projectile.GetNextPositionXCorrectionAbsolute();

        }
        else
        {
            newPosition.y = trajectoryStartPosition.y + projectile.GetNextYTrajectoryPosition() / shadowPositionYDivider + projectile.GetNextPositionYCorrectionAbsolute();

        }

        projectileShadow.position = newPosition;
    }
    private void UpdateProjectileRotation()
    {
        Vector3 projectileMoveDir = projectile.GetProjectileMoveDir();

        projectileVisual.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);
        projectileShadow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);

    }
    private void UpdateShadowProjectileRotation()
    {
        Vector3 projectileMoveDir = projectile.GetProjectileMoveDir();

        projectileVisual.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);
        projectileShadow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDir.y, projectileMoveDir.x) * Mathf.Rad2Deg);

    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

}
