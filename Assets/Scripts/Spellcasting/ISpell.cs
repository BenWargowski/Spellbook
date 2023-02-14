using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
    void spellEffect(GameObject prefab);
    float getEffectValue();
    void init(float speed, float damage);
}
