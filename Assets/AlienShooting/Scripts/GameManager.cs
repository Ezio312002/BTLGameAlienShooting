using System.Collections;
using System.Collections.Generic;
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

    private int m_enemyPerLeverOrignal;
    private List<Enemy> m_enemySpaweds;
    private int m_killed;
    private int m_level = 1;
    private int m_wave = 1;
    private int score;

    private bool m_isSlowed;
    private bool m_isBeginSlow;

    public int Killed { get => m_killed; set => m_killed = value; }
    public bool IsSlowed { get => m_isSlowed; set => m_isSlowed = value; }
    public int Score { get => score;  }



    public override void Awake()
    {
        MakeSingleton(false);
        m_enemySpaweds = new List<Enemy>();
    }

    public override void Start()
    {
        Spawn();
    }
    public void Spawn()
    {   
        if(!SlowController.Ins || !Player.Ins) return;

        SlowController.Ins.slowdownLength = (enemyUpPerLevel / 2 + 1.5f) - timeDownPerLevel * m_wave;

        Player.Ins.Bullet = enemyUpPerLevel + bulletExtra;
        if (enemyPbs == null || enemyPbs.Length <= 0) return;
        for (int i = 0;i < enemyPerLevel;i++)
        {
            int ranIdx = Random.Range(0, enemyPbs.Length);

            var enemyPb = enemyPbs[ranIdx];

            float spawPosX = Random.Range(-8, 8);
            float spawPosY = Random.Range(-7, 8.5f);
            Vector3 spawnPos = new Vector3(spawPosX, spawPosY, 0f);

            if (enemyPb != null)
            {
                var enemyClone = Instantiate(enemyPb, spawnPos, Quaternion.identity);
                m_enemySpaweds.Add(enemyClone);
            }


        }
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
