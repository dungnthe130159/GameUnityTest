using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLive = 100;
    [SerializeField] TextMeshProUGUI liveText;
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 00000;

    void Awake()
    {
        int NumGameSession = FindObjectsOfType<GameSession>().Length;
        if(NumGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        liveText.text = playerLive.ToString(); 
        scoreText.text = score.ToString();
    }

    public void AddScoreCoin(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLive > 1) 
        {
            TakeLive();
        }
        else
        {
            ResetTheGame();
        }
    }

    void ResetTheGame()
    {
        FindObjectOfType<ScenePersist>().ResetPersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void TakeLive()
    {
        playerLive--;
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
        liveText.text = playerLive.ToString();
    }

}
