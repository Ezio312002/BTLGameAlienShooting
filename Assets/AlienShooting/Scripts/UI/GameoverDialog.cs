using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverDialog : Dialog
{
    public Text totalScoreTxt;
    public Text bestScoreTxt;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if (AudioController.Ins)
        {
            AudioController.Ins.StopPlayMusic();
            AudioController.Ins.PlaySound(AudioController.Ins.loseSound);

        }
        if (totalScoreTxt && GameManager.Ins)
        {
            totalScoreTxt.text = GameManager.Ins.Score.ToString();

        }
        if (bestScoreTxt && GameManager.Ins)
        {
           bestScoreTxt.text = Pref.bestScore.ToString();
        }
        if (AudioController.Ins)
        {
            AudioController.Ins.PlaySound(AudioController.Ins.loseSound);
        }


    }
    public void BackToMenu ()
    {
        if (SceneController.Ins)
        {
            SceneController.Ins.LoadCurrentScene();

        }
    }
    public void Replay ()
    {
        Close ();
        if (GameManager.Ins)
        {
            GameManager.Ins.StartGame();
        }
    }
}
