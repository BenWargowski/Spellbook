using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SpellCasting : MonoBehaviour
{
    public List<Spell> list;
    private string input = string.Empty;
    //We use an Action so we can subscribe spell effects as coroutines at will
    public Action OnSpellTyped;


    // Update is called once per frame
    void Update()
    {
        //Check for all uppercase letters typed by the plyer
        foreach(char l in Input.inputString) if (Input.inputString.Any(Char.IsUpper))
        {
            //Since letters are typed per frame, we add letters to the input and check if there is a spell with the same name
            input += l;
            CheckSpellList();
        } else
        {
            //If anything else is input while casting a spell, it will reset
            input = String.Empty;
        }
    }

    void CheckSpellList()
    {
        //Check to see if input matches any of the spell names in the list
        foreach(Spell l in list) if (input == l.name.ToUpper())
        {
            //Subscribe the spell effect to the Action, run it, the unsubscribe from it, resetting the string on a successful cast
            OnSpellTyped += l.castSpell;
            OnSpellTyped.Invoke();
            OnSpellTyped -= l.castSpell;
            input = string.Empty;
        }
    }
}
