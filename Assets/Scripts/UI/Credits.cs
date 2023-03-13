using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    public GameObject playMenu;

    void OnEnable()
    {
        playMenu.SetActive(false);
    }
    private void OnDisable()
    {
        playMenu.SetActive(true);
    }
}