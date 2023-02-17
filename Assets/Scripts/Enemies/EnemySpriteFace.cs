using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteFace : MonoBehaviour
{
    [SerializeField] private BehaviorStateManager manager;
    private SpriteRenderer rend;
    private bool isActive = true;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        GameEvents.Instance.playerVictory += EnemyDeath;
    }

    void LateUpdate()
    {
        if (!isActive) return;

        if (manager.GetIsFacingRight())
        {
            rend.flipX = false;
        }
        else
        {
            rend.flipX = true;
        }
    }

    private void EnemyDeath()
    {
        isActive = false;
    }
}
