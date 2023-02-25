using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Main script for the Player.
/// Manages health, death, effects
/// </summary>
public class Player : MonoBehaviour, IDamageable, IHealable{
    //NOTE: Possible future expansion could be to add a finite-state machine

    [Header("References")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private MovementManager movementManager;
    [SerializeField] private SpellCastingManager spellCasting;
    [SerializeField] private new SpriteRenderer renderer;

    [Header("Stats")]
    [SerializeField] private float baseMaxHealth;
    [SerializeField] private float baseMaxMana;
    [SerializeField] private float baseManaRegenRate; //per second
    [SerializeField] private float baseMovementSpeed;
    [SerializeField] private float baseSpellDamageMultiplier;
    [SerializeField] private float baseArmor;

    [Header("Other Options")]
    //the minimum/maximum seconds the player can get invulnerability for when damaged
    //the duration is scaled to the damage, and the max is only reached if the damage = the player's base max health
    [SerializeField] private Vector2 invulnBounds; 

    //stats internal variables (use the Properties when modifying)
    private float _health;
    private float _mana;
    private float invulerabilityTime;
    private Color originalColor;

    private Dictionary<PlayerStat, HashSet<Status>> statusEffects;

    private void Awake() {
        this.statusEffects = new Dictionary<PlayerStat, HashSet<Status>>();
        this.invulerabilityTime = 0.0f;
        this.originalColor = this.renderer.color;
    }

    private void Start() {
        this.Health = this.baseMaxHealth;
        this.Mana = this.baseMaxMana;
    }

    private void Update() {
        //Update Status Effects
        foreach (HashSet<Status> sublist in statusEffects.Values) {
            foreach (Status status in sublist) {
                status.UpdateStatus();
            }
            //Remove expired effects
            sublist.RemoveWhere((x) => !x.IsValid());
        }

        //Mana Regeneration
        ManaRegen();

        //count down invuln time
        if (invulerabilityTime > 0.0f) {
            invulerabilityTime = Mathf.Max(0, invulerabilityTime - Time.deltaTime);
        }
    }

    /// <summary>
    /// Add a new status effect to the player
    /// </summary>
    /// <param name="statAffected">The player stat that the effect will affect. If using a custom effect, use PlayerStat.OTHER</param>
    /// <param name="statusEffect">Effect to apply to the stat</param>
    public void AddStatusEffect(PlayerStat statAffected, Status statusEffect) {
        //create set for stat if it doesn't already exist
        if (!this.statusEffects.ContainsKey(statAffected)) {
            this.statusEffects[statAffected] = new HashSet<Status>();
        }

        this.statusEffects[statAffected].Add(statusEffect);

        //Manual update of HP bar if MAX HEALTH was changed
        if (statAffected == PlayerStat.MAX_HEALTH && healthBar != null) healthBar.UpdateBar(_health, this.MaxHealth);
    }

    /// <summary>
    /// Remove status effects of a certain type
    /// </summary>
    /// <param name="stat">Type/Stat to remove</param>
    /// <param name="rule">Remove All? Only Buffs? Only Debuffs?</param>
    public void RemoveStatusEffects(PlayerStat stat, StatRemoveRule rule) {
        if (!this.statusEffects.ContainsKey(stat)) return;

        if (rule == StatRemoveRule.ALL) {
            this.statusEffects[stat].Clear();
            return;
        }

        foreach (Status status in this.statusEffects[stat]) {
            if ((rule == StatRemoveRule.NEGATIVE_ONLY && status.modifier < 0) ||
                (rule == StatRemoveRule.POSITIVE_ONLY && status.modifier > 0)) {
                    this.statusEffects[stat].Remove(status);
            }
        }
    }

    private float ModifierSum(PlayerStat stat) {
        if (!this.statusEffects.ContainsKey(stat)) return 0.0f;

        float sum = 0.0f;
        foreach (Status status in this.statusEffects[stat]) {
            sum += status.modifier;
        }

        return sum;
    }

    private void Death() {
        //disable movement and spellcasting
        this.movementManager.Active = false;
        this.spellCasting.enabled = false; //TODO: this just disables the component. might not be the greatest solution?

        //TODO: play a dying animation

        //dispatches player death event
        GameEvents.Instance.PlayerDeath();
    }

    public void Damage(float damage, bool triggerInvuln, bool ignoreArmor) {
        //if invulnerable, return
        if (invulerabilityTime > 0.0f) return;

        //Apply armor damage reduction
        if (!ignoreArmor) {
            damage *= (1 - (this.Armor / 100));
        }

        //Final Damage cannot be non-positive
        if (damage <= 0) return;
        
        this.Health -= damage;

        if (triggerInvuln)
        {
            //Add invulnerability based on damage taken -- more damage, more invulnerability
            float min = invulnBounds.x;
            float max = invulnBounds.y;
            this.invulerabilityTime = Mathf.Clamp((((max - min) / this.baseMaxHealth) * damage) + min, min, max);
        }

        //Damage vfx/sfx
        StopAllCoroutines();
        StartCoroutine(ColorFlash(Color.red, 5.0f, 2.5f));
    }

    public void Heal(float hp) {
        //Healing canot be non-positive
        if (hp <= 0) return;
        this.Health += hp;
        
        //TODO: Heal effects
    }

    private void ManaRegen() {
        this.Mana += this.ManaRegenRate * Time.deltaTime;
    }
    
    private IEnumerator ColorFlash(Color color, float fadeInSpeed, float fadeOutSpeed) {
        Color startColor = renderer.color;
        float t = 0.0f;

        //lerp from current color to target color
        while (t <= 1.0f) {
            renderer.color = Color.Lerp(startColor, color, t);
            t += fadeInSpeed * Time.deltaTime;
            yield return null;
        }

        //lerp from current color to default/original color
        t = 0.0f;
        while (t <= 1.0f) {
            renderer.color = Color.Lerp(color, this.originalColor, t);
            t += fadeOutSpeed * Time.deltaTime;
            yield return null;
        }

        //just in case -- set color back to original
        renderer.color = this.originalColor;
    }

    //GETTERS AND SETTERS ---------------------
    public char TargetKey => this.movementManager.TargetKey;

    public float MaxHealth => (this.baseMaxHealth + ModifierSum(PlayerStat.MAX_HEALTH));
    public float MaxMana => (this.baseMaxMana + ModifierSum(PlayerStat.MAX_MANA));
    public float ManaRegenRate => (this.baseManaRegenRate + ModifierSum(PlayerStat.MANA_REGEN_RATE));
    public float MovementSpeed => (this.baseMovementSpeed + ModifierSum(PlayerStat.MOVEMENT_SPEED));
    public float SpellDamageMultiplier => (this.baseSpellDamageMultiplier + ModifierSum(PlayerStat.SPELL_DAMAGE));
    public float Armor => (this.baseArmor + ModifierSum(PlayerStat.ARMOR));
    
    public float Health {
        get {
            return _health;
        }
        private set {
            //set value w/ respect to bounds
            _health = Mathf.Clamp(value, 0, this.MaxHealth);

            //update health bar
            if (healthBar != null) healthBar.UpdateBar(_health, this.MaxHealth);

            if (_health <= 0) {
                Death();
            }
        }
    }

    public float Mana {
        get {
            return _mana;
        }
        set {
            //set value w/ respect to bounds
            _mana = Mathf.Clamp(value, 0, this.MaxMana);
        }
    }


}