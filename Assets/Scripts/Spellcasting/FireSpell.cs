using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : ScriptableObject, ISpell
{
    public float spellDamage;
    public float spellSpeed;
    public Player player;
    public SpellType type = SpellType.FIRE;

    public float getEffectValue()
    {
        return spellDamage;
    }

    public void spellEffect(GameObject prefab)
    {
        EnemyHealth enemy = FindObjectOfType<EnemyHealth>();
        Vector2 direction = enemy.transform.position - player.transform.position;
        prefab.GetComponent<SpellProjectile>().init(spellSpeed, spellDamage, type);
        Instantiate(prefab, player.transform.position + new Vector3(0, 0, 0), Quaternion.LookRotation(direction, Vector2.up));
    }

    public void init(float speed, float damage)
    {
        //Initializes the spell with damage parametres
        player = FindObjectOfType<Player>();
        spellDamage = damage * player.SpellDamageMultiplier;
        spellSpeed = speed;
    }
}
