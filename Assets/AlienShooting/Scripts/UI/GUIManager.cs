using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUIManager : Singleton<GUIManager>
{
    public GameObject mainMenu;
    public GameObject gamePlay;
    public Text bulletsText;
    public Text levelText;
    public Image timerFilled;

    public Dialog gameoverDialog;
    public Dialog waveCompleteDialog;



    public override void Awake()
    {
        MakeSingleton(false);

    }
    public override void Start()
    {
        base.Start();

        UpdateTimerBar(1, 1);

    }

    public void ShowGamePlay(bool isShow)
    {
        if(gamePlay)
        {
            gamePlay.SetActive(isShow);

        }
        if (mainMenu) {
            mainMenu.SetActive(!isShow);
        }
    }

    public void UpdateBulletsText(int bullets)
    {
        if (bulletsText )
            bulletsText.text = "x"+ bullets;

    }
    public void UpdateLevelText(int level)
    {
        if (levelText)
            levelText.text = "LEVEL " + level;

    }
    public void UpdateTimerBar(float cur , float total , bool isReverse = false)
    {
        if (!timerFilled) return;
        if (isReverse )
        {
           timerFilled.fillAmount = 1 - (cur / total) ;
        }else
        {
            timerFilled.fillAmount = cur / total ;
        }

    }



}
