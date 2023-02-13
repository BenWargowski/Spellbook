using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public float speed;
    public float damage;

    public void init(float s, float d)
    {
        speed = s;
        damage = d;
    }

    void Start()
    {
        transform.LookAt(FindObjectOfType<EnemyHealth>().GetComponent<Transform>());
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyHealth>().Health = collision.GetComponent<EnemyHealth>().Health - damage;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


    }

}
