using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public string numberFormat;
    public PlayerController player;
    public GameObject logo;
    public GameObject shadow;
    public GameObject ingameUI;
    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI scoreCounter;
    public GameObject endScreen;
    public TextMeshProUGUI endScreenFlowerCount;
    public TextMeshProUGUI endScreenAnimalCount;
    public TextMeshProUGUI endScreenTrashCount;
    public TextMeshProUGUI endScreenScore;
    public TextMeshProUGUI endScreenHighscore;
    public GameObject retryButton;
    public GameObject settingsButton;
    public GameObject tutorialButton;
    public GameObject tutorialImage;
    public GameObject settings;

    public GameObject musicToggle;
    public GameObject controlsToggle;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ammoCounter.text = player.currentAmmo.ToString() + " / " + player.maxAmmo.ToString();
        scoreCounter.text = string.Format(numberFormat, GameController.Instance.score);

        if(musicToggle.transform.localScale.x > 0 && !AudioListener.pause)
        {
            musicToggle.transform.localScale = new Vector3(-1, 1, 1);
        } else if (musicToggle.transform.localScale.x < 0 && AudioListener.pause) {
            musicToggle.transform.localScale = new Vector3(1, 1, 1);
        }


        if (controlsToggle.transform.localScale.x > 0 && GameController.Instance.player.invertedAiming)
        {
            controlsToggle.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (controlsToggle.transform.localScale.x < 0 && !GameController.Instance.player.invertedAiming)
        {
            controlsToggle.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SetEndScreen(bool condition)
    {
        endScreen.SetActive(condition);
        shadow.SetActive(condition);
        retryButton.SetActive(condition);
        settingsButton.SetActive(condition);
        tutorialButton.SetActive(condition);
        if (condition)
        {
            SoundManager.Instance.Play("result_screen");
            PopulateEndScreen();        
        }
    }

    public void SetStartScreen(bool condition)
    {
        logo.SetActive(condition);
        shadow.SetActive(condition);
        retryButton.SetActive(condition);
        settingsButton.SetActive(condition);
        tutorialButton.SetActive(condition);
        if (condition)
        {
            SoundManager.Instance.PlayLoop("mainmenu_bg");
            PopulateEndScreen();
        }
    }

    public void SetTutorial(bool condition)
    {
        if (GameController.Instance.gameState == GameController.GameState.StartScreen)
        {
            SetStartScreenTutorial(condition);
        }
        else if (GameController.Instance.gameState == GameController.GameState.EndScreen)
        {
            SetEndScreenTutorial(condition);
        }
    }

    public void SetEndScreenTutorial(bool condition)
    {
        tutorialImage.SetActive(condition);
        settingsButton.SetActive(!condition);
        tutorialButton.SetActive(!condition);
        retryButton.SetActive(!condition);
        endScreen.SetActive(!condition);
    }

    public void SetStartScreenTutorial(bool condition)
    {
        tutorialImage.SetActive(condition);
        settingsButton.SetActive(!condition);
        tutorialButton.SetActive(!condition);
        retryButton.SetActive(!condition);
    }

    public void SetSettings(bool condition)
    {
        if (GameController.Instance.gameState == GameController.GameState.StartScreen)
        {
            SetStartScreenSettings(condition);
        } else if(GameController.Instance.gameState == GameController.GameState.EndScreen)
        {
            SetEndScreenSettings(condition);
        }
    }

    public void SetEndScreenSettings(bool condition)
    {
        settings.SetActive(condition);
        settingsButton.SetActive(!condition);
        tutorialButton.SetActive(!condition);
        retryButton.SetActive(!condition);
        endScreen.SetActive(!condition);
    }

    public void SetStartScreenSettings(bool condition)
    {
        settings.SetActive(condition);
        settingsButton.SetActive(!condition);
        tutorialButton.SetActive(!condition);
        retryButton.SetActive(!condition);
    }

    public void SetIngameUI(bool condition)
    {
        ingameUI.SetActive(condition);
    }

    public void PopulateEndScreen()
    {
        endScreenFlowerCount.text = string.Format(numberFormat, GameController.Instance.flowerCount);
        endScreenAnimalCount.text = string.Format(numberFormat, GameController.Instance.animalCount);
        endScreenTrashCount.text = string.Format(numberFormat, GameController.Instance.trashCount);
        endScreenScore.text = string.Format(numberFormat, GameController.Instance.score);
        endScreenHighscore.text = string.Format(numberFormat, GameController.Instance.highscore);
    }
}
