using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAlmanacManager : MonoBehaviour
{
    [SerializeField] private AlmanacPage[] pages;
    [SerializeField] private SegmentedBar manaBar;

    private int _index;
    private int Index {
        get {
            return _index;
        }
        set {
            //guard clause -- must be in range
            if (value < 0 || value >= this.pages.Length) return;

            //disable all pages
            foreach (AlmanacPage page in this.pages) {
                if (page.gameObject.activeInHierarchy) {
                    page.gameObject.SetActive(false);
                }
            }

            //enable the page
            this.pages[value].gameObject.SetActive(true);

            //update the mana bar
            this.manaBar.UpdateBar(this.pages[value].Spell.ManaCost, 100.0f);

            _index = value;
        }
    }

    public void ChangePageRelative(int i) {
        this.Index += i;
    }

    public void Initialize() {
        this.Index = 0;
    }

}
