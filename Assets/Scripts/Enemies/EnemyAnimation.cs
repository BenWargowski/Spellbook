using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation
{
    public string name { get; private set; }
    public AnimParamType type { get; private set; }
    public int state { get; private set; }

    public EnemyAnimation(string n, AnimParamType t, int s = 0)
    {
        name = n;
        type = t;
        state = s;
    }
}

public enum AnimParamType
{
    INT,
    TRIG
}