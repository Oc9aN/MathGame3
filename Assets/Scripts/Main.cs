using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public int CurrentStage;
    
    public GameObject MianUI;
    public GameObject Name;
    public GameObject PauseButton;
    public GameObject ExitButton_;
    public GameObject StartButton_;
    public GameObject BackButton;
    public List<GameObject> NonMainUI = new List<GameObject>();
    public GameObject PauseUI;
    public GameObject ExitUI;

    public MoneyGame moneygame;
    public TrainGame TrainGame;
    public CakeGame cakeGame;
    public PuzzleGame puzzleGame;

    public List<AudioSource> SFX = new List<AudioSource>();

    public List<Sprite> TextSprite = new List<Sprite>();
    public GameObject TextBox;
    public GameObject TextMessage;

    public bool click = true;

    public void Start()
    {
        Screen.SetResolution(1280, 800, true);
    }

    public void StartAndBackButton(bool check)
    {
        ExitButton_.SetActive(!check);
        StartButton_.SetActive(!check);
        Name.SetActive(!check);
        MianUI.SetActive(check);
        PauseButton.SetActive(check);
        BackButton.SetActive(check);
    }

    public void ExitUIButton(bool check)
    {
        ExitUI.SetActive(check);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void StageSelect(int StageNumber)
    {
        BackButton.SetActive(false);
        CurrentStage = StageNumber;
        switch (CurrentStage)
        {
            case 1:
                moneygame.LevelUI.SetActive(true);
                break;
            case 2:
                TrainGame.GameStart();
                break;
            case 3:
                cakeGame.GameStart();
                break;
            case 4:
                puzzleGame.LevelUI.SetActive(true);
                break;
            default:
                Debug.Log("!");
                break;
        }
        MianUI.SetActive(false);
    }

    public void MoneyGameLevel(int level)
    {
        moneygame.GameStart(level);
    }

    public void PuzzleGameLevel(int level)
    {
        moneygame.GameStart(level);
    }

    public void GamePause()
    {
        click = false;
        Time.timeScale = 0f;
        PauseUI.SetActive(true);

        moneygame.StopSound();
        TrainGame.StopSound();
        cakeGame.StopSound();
        puzzleGame.StopSound();
    }

    public void Resume()
    {
        click = true;
        Time.timeScale = 1.0f;
        PauseUI.SetActive(false);
    }

    public void GoToStage()
    {
        BackButton.SetActive(true);
        click = true;
        Time.timeScale = 1f;

        moneygame.StopAllCoroutines();
        moneygame.StopSound();

        TrainGame.StopCoroutines();
        TrainGame.StopSound();

        cakeGame.StopAllCoroutines();
        cakeGame.StopSound();

        puzzleGame.StopAllCoroutines();
        puzzleGame.StopSound();
        

        MianUI.SetActive(true);
        PauseUI.SetActive(false);

        Destroy(cakeGame.Mainobj);
        Destroy(puzzleGame.MainObj);
        for (int i = 0; i < moneygame.EmptyCoins.Count; i++)
        {
            Destroy(moneygame.EmptyCoins[i]);
        }

        for (int i = 0; i < NonMainUI.Count; i++)
        {
            NonMainUI[i].SetActive(false);
        }
    }

    bool soundscale = true;

    public List<Sprite> btn = new List<Sprite>();
    public void SoundScale(Image obj)
    {
        for (int i = 0; i < SFX.Count; i++)
        {
            if (soundscale)
            {
                SFX[i].volume = 0;
            }
            else
            {
                SFX[i].volume = 1;
                if (i == 0 || i == 1)
                {
                    SFX[i].volume = 0.3f;
                }
            }
        }
        soundscale = !soundscale;
        obj.sprite = btn[(int)SFX[3].volume];
    }

    public void ChangeText(int i)
    {
        if (!TextBox.activeSelf)
            TextBox.SetActive(true);
        TextMessage.GetComponent<Image>().sprite = TextSprite[i];
    }
}
