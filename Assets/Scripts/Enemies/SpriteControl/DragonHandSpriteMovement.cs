using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHandSpriteMovement : MonoBehaviour
{
    [SerializeField] float xMovement;
    [SerializeField] float yMovement;

    private Animator animator;
    private BehaviorStateManager behaviorManager;
    private EnemyStatusManager statusManager;
    private EnemyHealth health;
    private bool isActive;
    private Vector3 originalPosition;
    private float time;

    void Awake()
    {
        animator = GetComponent<Animator>();
        behaviorManager = GetComponentInParent<BehaviorStateManager>();
        statusManager = GetComponentInParent<EnemyStatusManager>();
        health = GetComponentInParent<EnemyHealth>();
        isActive = true;
        originalPosition = transform.position;
        time = Random.Range(0, 2 * (1 + xMovement + yMovement));

        if (statusManager != null)
        {
            statusManager.onStunned += StopMovement;
            statusManager.onNotStunned += StartMovement;
        }
    }

    void Start()
    {
        GameEvents.Instance.playerVictory += StopMovement;
    }

    void Update()
    {
        if (isActive)
        {
            time += Time.deltaTime;

            transform.position =
                new Vector3(
                    originalPosition.x + (Mathf.Sin(time) * xMovement),
                    originalPosition.y + (Mathf.Cos(time) * yMovement),
                    originalPosition.z);
        }
    }

    private void StopMovement()
    {
        isActive = false;

        animator.speed = health.Health <= 0 ? 0f : .25f;
    }

    private void StartMovement()
    {
        if (health.Health <= 0) return;

        isActive = true;

        animator.speed = 1f;
    }
}
