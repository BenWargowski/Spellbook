using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpellType{
    FIRE, LIGHTNING, ROCK, 
}

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    public new string name; 
    public float damage;
    public float speed;
    public int manaCost;
    public SpellType type;
    public GameObject attackPrefab;
    private SpellFactory spellFactory = new SpellFactory();
    

    public void castSpell()
    {
        ISpell castedspell = spellFactory.CreateSpell(type);
        castedspell.init(speed, damage);
        castedspell.spellEffect(attackPrefab);
    }
}
