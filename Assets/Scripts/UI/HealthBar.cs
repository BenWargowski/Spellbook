using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple script to update a health bar. Can be used for players or enemies.
/// </summary>
[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour {
    private Image healthBar;

    private void Awake() {
        this.healthBar = this.GetComponent<Image>();
    }

    public void UpdateBar(float health, float maxHealth) {
        // NOTE:
        // Possible expansion/things to do in the future could to make this smoother/animated
        // or even change the bar's color depending on what HP you are at
        this.healthBar.fillAmount = (health / maxHealth);        
    }
}
