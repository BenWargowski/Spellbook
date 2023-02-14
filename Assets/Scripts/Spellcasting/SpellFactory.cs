using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFactory
{
    
    public ISpell CreateSpell(SpellType type)
    {
        switch(type)
        {
            case SpellType.FIRE: return ScriptableObject.CreateInstance<FireSpell>();
            case SpellType.LIGHTNING: return ScriptableObject.CreateInstance<FireSpell>();
            case SpellType.ROCK: return ScriptableObject.CreateInstance<FireSpell>();
            default: return ScriptableObject.CreateInstance<FireSpell>();
        }
    }

}
