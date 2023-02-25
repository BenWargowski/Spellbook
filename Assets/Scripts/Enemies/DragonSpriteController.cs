using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSpriteController : EnemySpriteController
{
    void LateUpdate()
    {
        if (!isActive) return;

        Vector2 targetPosition = behaviorManager.GetTargetPosition();
        float lookAngle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, lookAngle));
    }
}
