using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Player player;

    private char tile;
    private int count;
    [SerializeField]
    private int totalCount;
    private int mode; // 0 for walk, 1 for casting
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject dummy;

    // Start is called before the first frame update
    void Start()
    {
        tile = '0';
        count = 0;
        button.gameObject.SetActive(false);
        mode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0)
        {
            dummy.gameObject.SetActive(false);
            walkMode();
        }
        else
        {
            dummy.gameObject.SetActive(true);
            castingMode();
        }
    }

    void walkMode()
    {
        if (tile == '0')
        {
            tile = (char)UnityEngine.Random.Range(65, 90);
            highlightKey(tile);
        }
        else if (mode == 0)
        {
            highlightKey(tile);
        }

        if (player.TargetKey == tile)
        {
            count++;
            tile = '0';
        }

        if (count == totalCount)
        {
            button.gameObject.SetActive(true);
        }
    }

    void castingMode()
    {
        
    }

    public void toggleMode()
    {
        mode = 1 - mode;
    }

    void highlightKey(char c)
    {
        GameObject tile = StageLayout.Instance.Tiles[c];
        Tile tileData = null;
        tile.TryGetComponent<Tile>(out tileData);
        Animator animator = null;
        tileData.Sprite.TryGetComponent<Animator>(out animator);
        animator.SetTrigger("Blink");
    }
}
