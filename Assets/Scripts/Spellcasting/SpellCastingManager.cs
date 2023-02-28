using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles the writing and casting of spells
/// </summary>
public class SpellCastingManager : MonoBehaviour {

        [Header("References")]

        [SerializeField] private Player player;
        [SerializeField] private SpellCastingBox textBox;

        [Header("Data")]

        //this is information to be loaded in from the editor. should not be used after the fact
        [SerializeField] private SpellData[] _spellList; 

        //spell hashmap for O(1) spell lookup
        private Dictionary<string, SpellData> spells;
        public Dictionary<SpellData, float> Cooldowns { get; private set; }

        private string _spellString;
        public string SpellString {
                get {
                        return _spellString;
                }
                private set {
                        _spellString = value;

                        //update UI
                        if (textBox != null) textBox.SpellText(value);
                }
        }

        private void Awake() {
                this.SpellString = string.Empty;
                this.spells = new Dictionary<string, SpellData>();
                this.Cooldowns = new Dictionary<SpellData, float>();

                //set up the hashmap for spells
                foreach (SpellData spell in this._spellList) {
                        this.spells.Add(spell.SpellName.ToUpper(), spell);
                }
                this._spellList = null; //free the memory for the list
        }

        private void Start() {
                //Subscribed to keyboard input event
                GameEvents.Instance.alphabetKeyPressed += OnSpellWrite;
        }

        private void Update() {
                //decrement cooldowns
                //hacky workaround :(
                foreach (SpellData data in this.Cooldowns.Keys.ToList()) {
                        //subtract cooldown time
                        if (this.Cooldowns[data] > 0.0f) {
                                this.Cooldowns[data] -= Time.deltaTime;
                        }
                }


                //check for enter key
                if (Input.GetKeyDown(KeyCode.Return)) {
                        //match spell name
                        if (this.spells.ContainsKey(this.SpellString)) {
                                SpellData data = this.spells[this.SpellString];

                                //make sure it's not on cooldown
                                if (!this.Cooldowns.ContainsKey(data) || this.Cooldowns[data] <= 0.0f) {
                                        //set cooldown and cast
                                        this.Cooldowns[data] = data.Cooldown;
                                        bool success = data.CastSpell(this.player);

                                        if (!success && this.textBox != null) {
                                                this.textBox.FailedSpell();
                                        }
                                }
                        }

                        //clear the spell string
                        this.SpellString = string.Empty;
                }

                //backspace functionality
                if (Input.GetKeyDown(KeyCode.Backspace)) {
                        //TODO: clean this up, seems quite messy/inefficient
                        if (this.SpellString.Length > 0) {
                                this.SpellString = this.SpellString.Substring(0, this.SpellString.Length - 1);
                        }
                }
        }

        private void OnSpellWrite(char c, bool shiftKey) {
                // truth table
                // lowercase | shift held || caps lock enabled | mode
                // F         | F          || F                 | move
                // F         | T          || F                 | spell
                // T         | F          || T                 | spell
                // T         | T          || T                 | spell
                if (!shiftKey && !Char.IsUpper(c)) return;
                c = Char.ToUpper(c);

                this.SpellString += c;
        }

}