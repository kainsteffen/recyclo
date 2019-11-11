using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public UIController uiController;
    public LevelController levelController;
    public TimeController timeController;
    public TouchController touchController;
    public PlayerController player;
    public int targetFrameRate;
    public int score = 0;
    public int highscore;
    public int flowerScoreValue;
    public int trashScoreValue;
    public int animalScoreValue;
    public int flowerCount;
    public int trashCount;
    public int animalCount;

    public enum GameState
    {
        Ingame,
        EndScreen,
        StartScreen,
    }

    public GameState gameState;

    private void Awake()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = targetFrameRate;
        Screen.orientation = ScreenOrientation.Portrait;
#endif

#if UNITY_IOS
        Application.targetFrameRate = targetFrameRate;
        Screen.orientation = ScreenOrientation.Portrait;
#endif

        highscore = PlayerPrefs.GetInt("highscore", 0);

        gameState = GameState.StartScreen;

        if (Instance != null)
        {
            DestroyImmediate(Instance);
        }
        else
        {
            Instance = this;
        }
        gameState = GameState.StartScreen;
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.Play("mainmenu_bg");
        uiController.SetStartScreen(true);
        player.stateMachine.ChangeState(new IdleState(player));
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.EndScreen:
                break;
        }
    }

    public void FlowerPlanted()
    {
        flowerCount++;
        score += flowerScoreValue;
    }

    public void TrashCollected()
    {
        trashCount++;
        score += trashScoreValue;
    }

    public void AnimalSaved()
    {
        animalCount++;
        score += animalScoreValue;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public void CheckHighscore()
    {
        highscore = Mathf.Max(score, highscore);
        PlayerPrefs.SetInt("highscore", highscore);
    }

    public void PlayButtonInput()
    {
        if(gameState == GameState.EndScreen)
        {
            RestartGame();
        } else if(gameState == GameState.StartScreen)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        gameState = GameState.Ingame; 
        SoundManager.Instance.Stop("mainmenu_bg");
        SoundManager.Instance.Play("gameplay_Bgm", 1, 1);
        uiController.SetStartScreen(false);
        uiController.SetIngameUI(true);
        touchController.gameObject.SetActive(true);
        player.stateMachine.ChangeState(new GroundedState(player));
    }

    public void RestartGame()
    {
        Reset();
        gameState = GameState.Ingame;

        SoundManager.Instance.Play("gameplay_Bgm", 1, 1);

        uiController.SetEndScreen(false);
        uiController.SetIngameUI(true);
        timeController.StopSlowMotion();
        touchController.enabled = true;
        levelController.ResetLevel();
        player.gameObject.SetActive(true);
        player.Reset();
        player.stateMachine.ChangeState(new GroundedState(player));
    }

    public void ToggleAudio()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void ToggleControls()
    {
        player.invertedAiming = !player.invertedAiming;
    }

    void Reset()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("AmmoPickup"))
        {
            Destroy(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Flower"))
        {
            Destroy(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Flower"))
        {
            Destroy(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyProjectile"))
        {
            Destroy(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerProjectile"))
        {
            Destroy(go);
        }

        score = 0;
        flowerCount = 0;
        animalCount = 0;   
    }

    public void EndGame()
    {
        CheckHighscore();
        gameState = GameState.EndScreen;
        player.gameObject.SetActive(false);
        uiController.SetEndScreen(true);
        uiController.SetIngameUI(false);
    }
}
