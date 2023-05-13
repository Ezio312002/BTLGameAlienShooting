using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{   

    public GameState State;
    public Camera cam;
    public bool isOnMobile;
    public Enemy[] enemyPbs;
    public int wavePerLevel;
    public int enemyPerLevel;
    public int enemyUpPerLevel;
    public int bulletExtra;
    public float timeDownPerLevel;

    private List<Enemy> m_enemySpaweds;
    private int m_killed;
    private int m_level = 1;
    private int m_waveCouting = 1;
    private int m_score;

    private bool m_isSlowed;
    private bool m_isBeginSlow;

    public int Killed { get => m_killed; set => m_killed = value; }
    public bool IsSlowed { get => m_isSlowed; set => m_isSlowed = value; }
    public int Score { get => m_score;  }

    public override void Awake()
    {
        MakeSingleton(false);
        m_enemySpaweds = new List<Enemy>();
    }

    public override void Start()
    {
        State = GameState.Starting;
        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }
    private void Update()
    {
        if (State != GameState.Playing) return;
       
        if(CanSlow() && !m_isBeginSlow)
        {
            m_isBeginSlow=true;
            float delay = Random.Range(0.01f, 0.05f);
            Timer.Schedule(this, delay, () =>
            {
                SlowController.Ins.DoSlowmotion();
            }, true);
        }

        if (Time.timeScale <  1 && m_killed >= m_enemySpaweds.Count && State != GameState.WaveCompleted)
        {
            if (m_waveCouting % wavePerLevel == 0 )
            {
                State = GameState.WaveCompleted;
                enemyPerLevel += enemyUpPerLevel;
                m_level++;
                if (GUIManager.Ins)
                {
                    GUIManager.Ins.UpdateLevelText(m_level);
                    if (GUIManager.Ins.waveCompleteDialog)
                    {
                        GUIManager.Ins.waveCompleteDialog.Show(true);
                    }
                }
                Debug.Log("You Won!.");
            } else
            {
                ResetData();
                Spawn();
                m_waveCouting++;
                State = GameState.Playing;
            }
            

        }
        if (Time.timeScale >= 0.9f && !CanSlow() && m_killed < m_enemySpaweds.Count && m_isSlowed &&
                State != GameState.GameOver)
        {

            State = GameState.GameOver;
            
            Debug.Log("GameOver!");
            if (GUIManager.Ins && GUIManager.Ins.gameoverDialog != null)
            {
                GUIManager.Ins.gameoverDialog.Show(true);
            }
            m_score = 0;
        }
    }
    public void Spawn()
    {   
        if(!SlowController.Ins || !Player.Ins) return;

        SlowController.Ins.slowdownLength = (enemyUpPerLevel / 2 + 2f) - timeDownPerLevel * m_waveCouting;

        Player.Ins.Bullet = enemyPerLevel + enemyUpPerLevel + bulletExtra;
        if (enemyPbs == null || enemyPbs.Length <= 0) return;
        for (int i = 0;i < enemyPerLevel;i++)
        {
            int ranIdx = Random.Range(0, enemyPbs.Length);

            var enemyPb = enemyPbs[ranIdx];

            float spawPosX = Random.Range(-8, 8);
            float spawPosY = Random.Range(7f, 8.5f);
            Vector3 spawnPos = new Vector3(spawPosX, spawPosY, 0f);

            if (enemyPb)
            {
                var enemyClone = Instantiate(enemyPb, spawnPos, Quaternion.identity);
                m_enemySpaweds.Add(enemyClone);
            }
        }
        if (GUIManager.Ins)
        {
            GUIManager.Ins.UpdateBulletsText(Player.Ins.Bullet);
        }
    }

    public void ResetData()
    {
        m_isSlowed = false;
        m_isBeginSlow = false;
        m_killed = 0;

        if(State == GameState.GameOver)
        {
            m_waveCouting = 1;
            m_level = 1;

        }
        State = GameState.Starting;

        if (m_enemySpaweds == null || m_enemySpaweds.Count <= 0) return;

        for (int i = 0; i < m_enemySpaweds.Count; i++)
        {
            var enemy = m_enemySpaweds[i];
            if (enemy)
            {
                Destroy(enemy.gameObject);
            }
        }
        m_enemySpaweds.Clear();

    }

    public void NextLevel()
    {
        if (State == GameState.WaveCompleted)
        {
            Timer.Schedule(this, 1f, () =>
            {
                m_waveCouting = 1;
                ResetData();
                Spawn();
                State = GameState.Playing;
            });
        }
        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }

    public void StartGame()
    {
        ResetData();
        Timer.Schedule(this, 1f, () =>
        {
            Spawn();
            State = GameState.Playing;
        });

        if (GUIManager.Ins)
        {
            GUIManager.Ins.UpdateLevelText(m_level);
            GUIManager.Ins.UpdateBulletsText(Player.Ins.Bullet);
            GUIManager.Ins.ShowGamePlay(true);

        }
        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }
    public void AddScore()
    {
        m_score++;
        Pref.bestScore = m_score;
    }


    private bool CanSlow()
    {
        if (m_enemySpaweds == null || m_enemySpaweds.Count <= 0 ) return false;

        int check = 0;

        for (int i = 0;i <  m_enemySpaweds.Count;i++)
        {
            var enemy = m_enemySpaweds[i];
            if (enemy && enemy.CanSlow)
            {
                check++;
            }
        }
        if (check == m_enemySpaweds.Count)
        {
            return true;

        }
        return false;
    }
}
