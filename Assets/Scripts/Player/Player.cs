using UnityEngine;

/// <summary>
/// Main script for the Player.
/// Manages health and holds references to movement and spellcasting
/// </summary>
[RequireComponent(typeof(MovementManager))]
[RequireComponent(typeof(SpellCasting))]
public class Player : MonoBehaviour {
    //NOTE: Possible future expansion could be to add a finite-state machine

    [Header("References")]
    [SerializeField] private HealthBar healthBar;

    [Header("Options")]
    [SerializeField] private float maxHealth;

    private MovementManager movementManager;
    private SpellCasting spellCasting;

    // Health -----
    private float _health;

    public float Health {
        get {
            return _health;
        }
        set {
            //set value w/ respect to bounds
            _health = Mathf.Clamp(value, 0, this.maxHealth);

            //update health bar
            if (healthBar != null) healthBar.UpdateBar(_health, this.maxHealth);

            if (_health <= 0) {
                Death();
            }
        }
    }

    private void Awake() {
        this.movementManager = this.GetComponent<MovementManager>();
        this.spellCasting = this.GetComponent<SpellCasting>();
    }

    private void Start() {
        this.Health = this.maxHealth;
    }

    private void Death() {
        //disable movement and spellcasting
        this.movementManager.Active = false;
        this.spellCasting.enabled = false; //TODO: this just disables the component. might not be the greatest solution?

        //TODO: play a dying animation

        //dispatches player death event
        GameEvents.Instance.PlayerDeath();
    }

}