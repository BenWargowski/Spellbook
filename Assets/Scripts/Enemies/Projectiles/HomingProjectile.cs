using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : BasicProjectile
{
    private float rotationSpeed;

    public void UpdateDirection(Vector3 newMoveDirection)
    {
        moveDirection = Vector3.Lerp(moveDirection, newMoveDirection, rotationSpeed * Time.deltaTime);
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
