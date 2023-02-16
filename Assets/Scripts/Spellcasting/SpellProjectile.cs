using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public SpellType type;

    public void init(float s, float d, SpellType t)
    {
        speed = s;
        damage = d;
        type = t;
    }

    void Start()
    {
        transform.LookAt(FindObjectOfType<EnemyHealth>().GetComponent<Transform>());
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = null;
        if (collision.TryGetComponent<EnemyHealth>(out enemyHealth)) {
            enemyHealth.Damage(damage, type, false);
            Destroy(gameObject);    
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


    }

}
