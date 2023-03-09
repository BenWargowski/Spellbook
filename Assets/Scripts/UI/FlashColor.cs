using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashColor : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private float speed;

    private float t;
    private bool flipped;

    private void Start() {
        t = 0.0f;
        flipped = false;
    }

    // Update is called once per frame
    private void Update() {
        if (flipped) {
            image.color = Color.Lerp(color2, color1, t);
        }
        else {
            image.color = Color.Lerp(color1, color2, t);
        }

        t += this.speed * Time.deltaTime; 

        if (t >= 1.0f) {
            flipped = !flipped;
            t = 0.0f;
        }        
    }
}
