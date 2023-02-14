using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] private List<Spell> list;
    private string input = string.Empty;
    //We use an Action so we can subscribe spell effects as coroutines at will
    public Action OnSpellTyped;


    // Update is called once per frame
    void Update()
    {
        //Check for all uppercase letters typed by the plyer
        foreach(char l in Input.inputString) if (Input.inputString.Any(Char.IsUpper) || Input.inputString == " ")
        {
            //Since letters are typed per frame, we add letters to the input and check if there is a spell with the same name
            input += l;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckSpellList();
        }
    }

    void CheckSpellList()
    {
        //Check to see if input matches any of the spell names in the list
        foreach(Spell l in list) if (input == l.name.ToUpper())
        {
            //Subscribe the spell effect to the Action, run it, the unsubscribe from it, resetting the string on a successful cast
            l.castSpell();
        }
        ResetSpellcasting();
    }

    
    public void ResetSpellcasting()
    {
        //Resets the string for spellcasting, you can call this whenever you want
        input = string.Empty;
    }
}
