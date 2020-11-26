using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour
{
   private Text highscore;

   private void Awake()
   {
      PlayerPrefs.SetInt("HighScore",0);
   }

   private void OnEnable()
   {
      highscore = GetComponent<Text>();
      highscore.text = "Hıgh Score : "+PlayerPrefs.GetInt("HighScore").ToString();
   }
}
