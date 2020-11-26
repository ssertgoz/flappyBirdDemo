using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentScoreText : MonoBehaviour
{
    public Text ScoreText;


    private void OnEnable()
    {
        Text CurrentText = GetComponent<Text>();
        CurrentText.text = "Current Score : " + ScoreText.text;
    }
}
