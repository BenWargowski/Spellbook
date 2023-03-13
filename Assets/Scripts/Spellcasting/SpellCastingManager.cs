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
        [SerializeField] private SegmentedBar manaCostDisplay;
        [SerializeField] private Animator animator;
        
        //spell sounds played when casted
        [SerializeField] protected AudioClip earthquakeClip;
        [SerializeField] protected AudioClip fireClip;
        [SerializeField] protected AudioClip healClip;
        [SerializeField] protected AudioClip lightningClip;
        [SerializeField] protected AudioClip shieldClip;
        [SerializeField] protected AudioClip teleportClip;

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

        //TODO: this is messy
        private bool _capsLockInit;
        private bool _capsLockState;
        public bool CapsLockState => (_capsLockInit && _capsLockState);

        private void Awake() {
                this.SpellString = string.Empty;
                this.spells = new Dictionary<string, SpellData>();
                this.Cooldowns = new Dictionary<SpellData, float>();

                //set up the hashmap for spells
                foreach (SpellData spell in this._spellList) {
                        this.spells.Add(spell.SpellName.ToUpper(), spell);
                }
                this._spellList = null; //free the memory for the list

                _capsLockInit = false;
                _capsLockState = false;
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

                //only update state if we already know it.
                //if it hasn't been initialized -- the state is indeterminate
                if (_capsLockInit && Input.GetKeyDown(KeyCode.CapsLock)) {
                        _capsLockState = !_capsLockState; //toggle state
                }
                animator.SetBool("isSpelling", (Input.GetKey(KeyCode.LeftShift) || CapsLockState));
                
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

                                        if (success) {
                                                PlaySpellSFX(SpellString);
                                        }

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

                //Update mana cost display
                if (this.spells.ContainsKey(this.SpellString)) {
                        this.manaCostDisplay.gameObject.SetActive(true);
                        this.manaCostDisplay.UpdateBar(this.spells[this.SpellString].ManaCost, player.MaxMana);
                }
                else if (this.manaCostDisplay.gameObject.activeInHierarchy) {
                        this.manaCostDisplay.gameObject.SetActive(false);
                }
        }

        private void PlaySpellSFX(string SpellString)
        {
                // plays sound clip of spell being cast. update sound files on player prefab
                if (SpellString == "EARTHQUAKE") { SoundManager.Instance.PlaySound(earthquakeClip); }
                else if (SpellString == "FIRE") { SoundManager.Instance.PlaySound(fireClip); }
                else if (SpellString == "HEAL") { SoundManager.Instance.PlaySound(healClip); }
                else if (SpellString == "LIGHTNING") { SoundManager.Instance.PlaySound(lightningClip); }
                else if (SpellString == "SHIELD") { SoundManager.Instance.PlaySound(shieldClip); }
                else if (SpellString == "TELEPORT") { SoundManager.Instance.PlaySound(teleportClip); }
        }

        private void OnSpellWrite(char c, bool shiftKey) {
                // truth table
                // lowercase | shift held || caps lock enabled | mode
                // F         | F          || F                 | move
                // F         | T          || F                 | spell
                // T         | F          || T                 | spell
                // T         | T          || T                 | spell
                bool isUpper = Char.IsUpper(c);
                if (!shiftKey && !isUpper) return;
                c = Char.ToUpper(c);

                //we now know for certain the caps lock state
                if (!_capsLockInit) _capsLockInit = true;

                //update caps lock state
                if (shiftKey && isUpper) _capsLockState = false;
                else if (shiftKey && !isUpper) _capsLockState = true;
                else _capsLockState = true;

                this.SpellString += c;
        }

        public void PlayerMoved() {
                //player has successfully moved, so we know that the caps lock state must be off
                //otherwise they would have begun casting

                if (!_capsLockInit) _capsLockInit = true;
                _capsLockState = false;
        }
}