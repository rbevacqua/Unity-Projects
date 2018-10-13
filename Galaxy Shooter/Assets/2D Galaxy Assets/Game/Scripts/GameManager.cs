using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{

    private UIManager _uiManager;

    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _playerPrefab;

    private bool gameStarted;

    private TimeSpan gameStartTime;

    private bool isNormalDifficulty;

    private bool isHardDifficulty;

    private bool isExtremeDifficulty;

    public int Score { get; private set; }

    // player hitpoints/lives
    public int Lives { get; private set; }



    public void Start()
    {
        gameStarted = false;
        isNormalDifficulty = false;
        isHardDifficulty = false;
        isExtremeDifficulty = false;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

    }

    void Update()
    {
    
        if (!gameStarted)
        {
            gameStartTime = DateTime.Now.TimeOfDay;
            
        }

        if (gameStarted)
        {
            var timeDiff = (DateTime.Now.TimeOfDay - gameStartTime);

            if (timeDiff.TotalSeconds > TimeSpan.FromSeconds(60 * 1).TotalSeconds && !isNormalDifficulty)
            {
                isNormalDifficulty = true;
                _spawnManager.StartSpawningEnemies(1);
            }

            if (timeDiff.TotalSeconds > TimeSpan.FromSeconds(60 * 2).TotalSeconds && !isHardDifficulty)
            {
                isHardDifficulty = true;
                _spawnManager.StartSpawningEnemies(2);
            }

            if (timeDiff.TotalSeconds > TimeSpan.FromSeconds((60 * 3)).TotalSeconds && !isExtremeDifficulty)
            {
                isExtremeDifficulty = true;
                _spawnManager.StartSpawningEnemies(2);
            }
        }
        
    }

    public void StartGame()
    {
        gameStarted = true;

        // disable title screen image
        _uiManager.ShowTitleScreen(false);

        // initialize game score and player lives
        Score = 0;
        Lives = 3;

        _uiManager.UpdateScore(Score);
        _uiManager.UpdateLives(Lives);

        // enable score and lives UI
        _uiManager.ShowLivesDisplay(true);
        _uiManager.ShowScoreDisplay(true);

        // enable spawning of enemies and other things
        _spawnManager.StartAllSpawning();

        // instatiate player prefab
        Instantiate(_playerPrefab, new Vector3(0, -3, 0), Quaternion.identity);
    }

    public void EndGame()
    {

        StartCoroutine(EndScreenCoolDown());

        _spawnManager.StopAllSpawning();
        

    }

    public void AddUpdateScoreUI(int num)
    {
        Score += num;
        _uiManager.UpdateScore(Score);
    }

    public void UpdateLiveUI(int numLivesToAdd)
    {
        Lives += numLivesToAdd;
        _uiManager.UpdateLives(Lives);
    }

    public void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    private IEnumerator EndScreenCoolDown()
    {
        yield return new WaitForSeconds(5);

        // destroy any remaining enemies in the game
        var enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemiesRemaining)
        {
            Destroy(enemy.gameObject);
        }

        // can start new game after 5 seconds from losing all lives
        gameStarted = false;

        isNormalDifficulty = false;
        isHardDifficulty = false;
        isExtremeDifficulty = false;

        _uiManager.ShowTitleScreen(true);
        _uiManager.ShowLivesDisplay(false);
        _uiManager.ShowScoreDisplay(false);
    }
}
