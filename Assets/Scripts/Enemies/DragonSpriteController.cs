using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSpriteController : EnemySpriteController
{
    [SerializeField] private float rotationSpeed;

    private Vector3 currentLookDirection;
    private bool isFacingTarget;

    protected override void Start()
    {
        base.Start();

        currentLookDirection = transform.rotation.eulerAngles;
    }

    void LateUpdate()
    {
        if (!isActive) return;

        Vector2 targetPosition = behaviorManager.GetTargetPosition();
        float lookAngle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;

        currentLookDirection = Vector3.Lerp(currentLookDirection, new Vector3(0, 0, lookAngle), rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentLookDirection);

        isFacingTarget = Mathf.Abs(currentLookDirection.z - lookAngle) < 2.5;
    }

    public override bool GetIsFacingTarget()
    {
        return isFacingTarget;
    }
}
