using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class enemystatus_unittest : MonoBehaviour {
    public EnemyStat statusType;
    public float modifier;
        public bool ignore;
        public float duration;
    public bool apply = false;
    public bool applyDmg = false;

    public void Update() {
        if (apply) {
            FindObjectOfType<EnemyStatusManager>().AddStatusEffect(statusType, new Status(modifier, duration));
            apply = false;
        }
        if (applyDmg) {
            FindObjectOfType<EnemyStatusManager>().AddStatusEffect(EnemyStat.OTHER, new EnemyDoTStatus(FindObjectOfType<EnemyHealth>(), SpellElement.FIRE, ignore, modifier, duration));
            applyDmg = false;
        }
    }
}