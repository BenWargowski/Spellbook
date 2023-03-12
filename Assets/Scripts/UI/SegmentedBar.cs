using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private Transform segmentHolder;

    [Header("Options")]
    [SerializeField] private int segmentCount;

    private Image[] segments;

    private void Awake() {
        //Populate segments
        this.segments = new Image[segmentCount];
        for (int i = 0; i < segmentCount; ++i) {
            this.segments[i] = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, segmentHolder).GetComponent<Image>();
            this.segments[i].name = $"BarSegment {i}";
        }
    }


    public void UpdateBar(float value, float maxValue) {
        float percentage = value / maxValue;
        float segmentSize = 1.0f / segmentCount; //represents the amount of percentage a single segment takes up

        //loop through each segment:
        // if the current (percentage - segmentSize) is >= 0 --> fill the entire segment
        // if it's less than 0 but percentage is > 0 --> fill it partially
        // if percentage itself is < 0 --> segment is empty
        // subtract segmentSize from percentage
        for (int i = 0; i < segmentCount; ++i) {
            if (percentage > 0.0f) {
                //full segment
                if (percentage >= segmentSize) {
                    segments[i].fillAmount = 1.0f;
                }
                //partial fill
                else {
                    segments[i].fillAmount = percentage / segmentSize;
                }
                percentage -= segmentSize;
            }
            //already ran out, set rest to 0
            else {
                segments[i].fillAmount = 0.0f;
            }

        }
        
    }
}
