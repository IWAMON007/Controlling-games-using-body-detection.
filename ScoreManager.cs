using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text scoreText;
    private Text totalscoreText;

    public int score = 0;

    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
        scoreText.text = "0";

        totalscoreText = GetComponentInChildren<Text>();
        totalscoreText.text = "0";
    }

    void Update()
    {
        scoreText.text = score.ToString();
        totalscoreText.text = score.ToString();
    }
}