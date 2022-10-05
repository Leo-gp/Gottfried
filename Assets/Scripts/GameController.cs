using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController playerPrefab;
    public GameObject gameOverScreen;
    public GameObject levelCompletedScreen;
    public Text lifeText;
    public GameObject[] disableOnGameOverOrCompleted;

    public int CurrentLives { get; set; }
    public DoorController LevelDoor { get; set; }

    private bool gameIsOver;
    private bool levelIsCompleted;

    public static GameController instance;

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Time.timeScale = 1;

        LevelDoor = FindObjectOfType<DoorController>();
    }

    void Start ()
    {
        CurrentLives = LevelController.instance.totalLives;
        SpawnNewPlayer();
    }

    void Update ()
    {
		if (CurrentLives <= 0 && !gameIsOver)
        {
            GameOver();
        }

        lifeText.text = "x" + (CurrentLives);

        if (Input.GetKeyDown(KeyCode.R) && !levelIsCompleted)
            SceneController.instance.RestartLevel();

        if (Input.GetKeyDown(KeyCode.Space) && levelIsCompleted)
            SceneController.instance.NextLevel();
    }

    public PlayerController SpawnNewPlayer ()
    {
        if (CurrentLives <= 0)
            return null;
        return Instantiate(playerPrefab, LevelController.instance.playerStartPoint, Quaternion.identity);
    }

    public void GameOver ()
    {
        Time.timeScale = 0;
        gameIsOver = true;
        gameOverScreen.SetActive(true);
        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            player.enabled = false;
        }

        foreach (var item in disableOnGameOverOrCompleted)
        {
            item.SetActive(false);
        }

        AudioController.instance.Play("GameOver");
    }

    public void LevelCompleted ()
    {
        if (LevelController.instance.lastLevel)
        {
            SceneController.instance.LoadScene("Ending");
        }
        else
        {
            Time.timeScale = 0;
            levelIsCompleted = true;
            foreach (var player in FindObjectsOfType<PlayerController>())
            {
                player.enabled = false;
            } 
            levelCompletedScreen.SetActive(true);
            foreach (var item in disableOnGameOverOrCompleted)
            {
                item.SetActive(false);
            }

            AudioController.instance.Play("Victory");
        }
    }
}
