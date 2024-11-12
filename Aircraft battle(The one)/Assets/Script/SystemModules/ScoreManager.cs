using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region SCORE DISPLAY
    public int Score=>score;
    int score;
    int currentscore;

    Vector3 scoreTextScale = new Vector3(1.4f, 1.4f, 1f);
    public void ResetScore()
    {
        score = 0;
        currentscore = 0;
        ScoreDisplay.UpdateText(score);
    }
    
    public void AddScore(int scorePoint)
    {
        currentscore += scorePoint;
       
        StartCoroutine(nameof(AddScoreCoroutine));
    }

    //¶¯Ì¬¼Ó·ÖUI
    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while(score < currentscore)
        {
            score += 1;
            ScoreDisplay.UpdateText(score);
            yield return null;  
        }
        ScoreDisplay.ScaleText(Vector3.one);    
    }
    #endregion
    #region HIGHT SCORE SYSTEM
    [System.Serializable]class PlayerScore
    {
        public int Score;
        public string PlayerName;
        public PlayerScore(int score, string playerName)
        {
            Score = score;
            PlayerName = playerName;
        }
    }
    [System.Serializable]class PlayerScoreData
    {

        public List<PlayerScore> List=new List<PlayerScore>();
    }
    //PlayerScoreData LoadPlayerScoreData()
    //{
    //    var playerScoreData=new PlayerScoreData();
    //    playerScoreData=SaveSystem.Load()
    //}
    #endregion  
}
