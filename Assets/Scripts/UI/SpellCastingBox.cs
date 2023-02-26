using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellCastingBox : MonoBehaviour
{
    public TextMeshProUGUI spellCastingText;
    public float pulseDuration = 2f;
    public float pulseIntensity = 1f;
    public float stopDuration = 3f;


    void Start()
    {
        
    }

    IEnumerator GlowRed()
    {
        Color originalColor = spellCastingText.color;
        float startTime = Time.time;
        float stopTime = startTime + stopDuration;

        while (Time.time < stopTime)
        {
            float elapsedTime = Time.time - startTime;
            float pulse = Mathf.Sin(elapsedTime * Mathf.PI / pulseDuration);
            float glow = pulse * pulseIntensity;

            spellCastingText.color = Color.Lerp(originalColor, Color.red, pulse);
            spellCastingText.fontSharedMaterial.SetFloat("_GlowPower", glow);

            yield return null;
        }

        spellCastingText.color = originalColor;
        spellCastingText.fontSharedMaterial.SetFloat("_GlowPower", 0f);
    }
    
    public void SpellText(string spellText)
    {
        spellCastingText.text = spellText;
    }

    public void FailedSpell()
    {
        StartCoroutine(GlowRed());
        spellCastingText.text = "Spell Failed";
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
