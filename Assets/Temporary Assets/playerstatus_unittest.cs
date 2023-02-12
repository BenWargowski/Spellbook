using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class playerstatus_unittest : MonoBehaviour {
    public PlayerStat statusType;
    public float modifier;
    public float duration;
    public bool apply = false;
    public bool applyHeal = false;
    public bool applyDmg = false;

    public void Update() {
        if (apply) {
            GetComponent<Player>().AddStatusEffect(statusType, new Status(modifier, duration));
            apply = false;
        }
        if (applyHeal) {
            GetComponent<Player>().AddStatusEffect(PlayerStat.OTHER, new HealStatus(GetComponent<Player>(), modifier, duration));
            applyHeal = false;
        }
        if (applyDmg) {
            GetComponent<Player>().AddStatusEffect(PlayerStat.OTHER, new DoTStatus(GetComponent<Player>(), modifier, duration, false));
            applyDmg = false;
        }
    }
}