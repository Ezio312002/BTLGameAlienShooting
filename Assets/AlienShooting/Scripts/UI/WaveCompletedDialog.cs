using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCompletedDialog : Dialog
{
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if (AudioController.Ins)
        {
            AudioController.Ins.PlaySound(AudioController.Ins.winSound);
        }
    }
    public void BackToMenu ()
    {
        if (SceneController.Ins)
        {
            SceneController.Ins.LoadCurrentScene();

        }
        
    }
    public void Replay()
    {
        Close();
        if (GameManager.Ins)
        {
            GameManager.Ins.StartGame();
        }
    }
    public void NextLevel ()
    {
        Close ();
        if (GameManager.Ins)
        {
            GameManager.Ins.NextLevel();
        }
    }
}
