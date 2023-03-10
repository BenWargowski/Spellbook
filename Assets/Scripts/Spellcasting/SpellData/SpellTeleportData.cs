using UnityEngine;
using System.Collections;

/// <summary>
/// Spell that makes the next move(s) instant for a duration
/// </summary>
[CreateAssetMenu(fileName = "New SpellTeleport", menuName = "Spells/SpellTeleport")]
public class SpellTeleportData : SpellData {

    //TODO: refactor
    private static int RefCount = 0;

    [Header("Teleport Settings")]

    [Tooltip("How long the Teleporting State should last")]
    [SerializeField] private float duration;
    
    [Tooltip("How many Teleports the Player can do. Set to -1 for Unlimited.")]
    [SerializeField] private int limit;

    public override bool CastSpell(Player player) {
        //already teleporting
        if (RefCount > 0 || player.MovementManager.Teleport) return false;

        //Handle mana
        if (!base.CastSpell(player)) return false;

        player.StartCoroutine(TeleportMechanism(player));
        return true;
    }

    public IEnumerator TeleportMechanism(Player player) {
        //initialize allow next move to be a teleport
        player.MovementManager.Teleport = true;
        player.TeleportIcon.SetActive(true);
        RefCount++;
        
        int count = 0;
        //my signature cursed for loop
        for (float elasped = 0.0f; elasped < this.duration && (count < limit || limit == -1); elasped += Time.deltaTime) {
            if (!player.MovementManager.Teleport) {
                ++count;
                player.MovementManager.Teleport = true;
            }

            yield return null;
        }

        RefCount--;
        if (RefCount <= 0) player.TeleportIcon.SetActive(false);
        player.MovementManager.Teleport = false;
    }

}