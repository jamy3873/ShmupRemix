using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper S;

    public Text scoreText;
    public int score;
    void Start()
    {
        scoreText = GetComponent<Text>();
        score = 0;
        scoreText.text = score.ToString();
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  UpdateScore(int value_)
    {
        score += value_;
        scoreText.text = score.ToString();
    }
}
