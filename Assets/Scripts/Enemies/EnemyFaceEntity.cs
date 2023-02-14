using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFaceEntity : MonoBehaviour
{
    [SerializeField] private BehaviorStateManager manager;
    private SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (manager.GetIsFacingRight())
        {
            rend.flipX = false;
        }
        else
        {
            rend.flipX = true;
        }
    }
}
