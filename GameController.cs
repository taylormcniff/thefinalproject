using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject playerExplosion;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text ScoreText;
    public Text RestartText;
    public Text GameOverText;
    public Text whoText;
    public Text highText;
    public Text winText;

    public AudioClip Loss;
    public AudioClip PersonalBest;
    public AudioSource musicSource;

    private bool gameOver;
    private bool restart;
    private bool win;
    private int score;
    public int count;
    public int highScore;

    void Start()
    {
        gameOver = false;
        restart = false;
        win = false;
        GameOverText.text = "";
        whoText.text = "";
        winText.text = "";
        highText.text = "High Score: " + highScore;
        score = 0;
        count = 0; 
        highScore = PlayerPrefs.GetInt("High Score", score);
        UpdateScore();
        StartCoroutine(SpawnWaves());
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene("HardMode");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("NormalMode");
        }
        if (restart)
        {
            if (Input.GetKey("return"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        {
            if (Input.GetKey("escape"))
                Application.Quit();
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[UnityEngine.Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            count = count + 1;

            if (gameOver)
            {
                GameOverText.text = "Game Over!";
                RestartText.text = "Press 'Enter' to Restart";
                whoText.text = "Created By Taylor McNiff";
                musicSource.clip = Loss;
                musicSource.Play();
                musicSource.loop = false;
                restart = true;
                win = false;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = "Points: " + score;
        highText.text = "High Score: " + highScore;
        if (score >= highScore)
        {
            highText.text = "High Score: " + score;
            PlayerPrefs.SetInt("High Score", score);
        }
    }

    public void GameOver()
    {
        gameOver = true;
    }
}