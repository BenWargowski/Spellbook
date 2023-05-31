using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeScore : MonoBehaviour
{
    public const string highScore = "High Score";
    public string highScorePlayerPref => highScore + SceneManager.GetActiveScene().buildIndex.ToString();
    [SerializeField] private TextMeshProUGUI highScoreTimer;
    [SerializeField] private TextMeshProUGUI scoreTimer;
    [SerializeField] private TextMeshProUGUI progressTimer;
    private float currentTime = 0;
    private bool isActive = true;

    private void Start()
    {
        GameEvents.Instance.playerDeath += Defeat;
        GameEvents.Instance.playerVictory += Victory;

        highScoreTimer?.gameObject.SetActive(false);
        scoreTimer?.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isActive) return;

        currentTime += Time.deltaTime * 100;

        if (progressTimer != null) progressTimer.text = TimeToString(currentTime);
    }

    public string TimeToString(float time)
    {
        return string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt(time / 6000), Mathf.FloorToInt(time / 100 % 60), Mathf.FloorToInt(time%100));
    }

    public void Victory()
    {
        isActive = false;
        progressTimer?.gameObject.SetActive(isActive);

        float currentHighScore = PlayerPrefs.GetFloat(highScorePlayerPref, Mathf.Infinity);
        bool isNewHighScore = currentTime < currentHighScore;
        PlayerPrefs.SetFloat(highScorePlayerPref, Mathf.Min(currentTime, currentHighScore));

        if (highScoreTimer != null) highScoreTimer.text = (isNewHighScore ? "New " : string.Empty) + string.Format("Best Time: {0}", TimeToString(PlayerPrefs.GetFloat(highScorePlayerPref, currentTime)));
        highScoreTimer?.gameObject.SetActive(true);

        if (!isNewHighScore)
        {
            if (scoreTimer != null) scoreTimer.text = string.Format("Time: {0}", TimeToString(currentTime));
            scoreTimer?.gameObject.SetActive(true);
        }
    }

    public void Defeat()
    {
        isActive = false;
        progressTimer?.gameObject.SetActive(isActive);
    }
}
