using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Sprite[] Lives;

    public Image LivesImageDisplay;

    public Image TitleScreenDisplay;

    public Text ScoreTextDisplay;

    public void ShowTitleScreen(bool activeStatus)
    {
        TitleScreenDisplay.gameObject.SetActive(activeStatus);
    }

    public void ShowScoreDisplay(bool activeStatus)
    {
        ScoreTextDisplay.gameObject.SetActive(activeStatus);
    }

    public void ShowLivesDisplay(bool activeStatus)
    {
        LivesImageDisplay.gameObject.SetActive(activeStatus);
    }

    public void UpdateLives(int currentNumLives)
    {
        LivesImageDisplay.sprite = Lives[currentNumLives];
    }

    public void UpdateScore(int score)
    {
        string[] lstOfScoreText = ScoreTextDisplay.text.Split(' ');

        ScoreTextDisplay.text = (lstOfScoreText[0] + " " + score);

    }
}
