using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public GameObject deadVfxPrefab;

    private bool m_canSlow;
    private bool m_isDead;

    public bool CanSlow { get => m_canSlow; set => m_canSlow = value; }
    public bool IsDead { get => m_isDead; set => m_isDead = value; }


    private void Update()
    {
        if (Time.timeScale >= 1) { 
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed, Space.World);
        
        }else
        {
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }
    public void Dead()
    {
        if (m_isDead) return;

        m_isDead = true;

        Instantiate(deadVfxPrefab, transform.position,Quaternion.identity);
        Destroy(gameObject);
        if (GameManager.Ins)
        {
            GameManager.Ins.Killed++;
            GameManager.Ins.AddScore();
        } 
    }
}
