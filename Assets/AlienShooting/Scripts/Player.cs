using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public GameObject viewFinderPb;

    private int m_bullet;
    private Camera m_camera;
    private GameObject m_viewFinderClone;

    public int Bullet { get => m_bullet; set => m_bullet = value; }

    public override void Awake()
    {
        MakeSingleton(false);

    }
    public override void Start()
    {


        if (!GameManager.Ins) return;
        
        m_camera = GameManager.Ins.cam;
         
        if (GameManager.Ins.isOnMobile || !viewFinderPb) return;

        m_viewFinderClone = Instantiate(viewFinderPb, new Vector3(10000, 10000, 0f), Quaternion.identity);

    }
    private void Update()
    {
        if (!m_camera) return;

        Vector3 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);


        m_viewFinderClone.transform.position = new Vector3( mousePos.x, mousePos.y,0f);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(mousePos);
        }

    }

    private void Shoot (Vector3 mousePos)
    {
        if(m_bullet <= 0) return;
        m_bullet --;

        Vector3 shootingDir = m_camera.transform.position - mousePos;
        shootingDir.Normalize();
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, shootingDir,0.1f );

        if(hits == null || hits.Length <= 0) return;


        for (int i = 0 ; i < hits.Length; i++)
        {
            var hitted = hits[i];

            if (!hitted.collider) continue;

            Debug.Log(hitted.collider.gameObject.name);
                var enemy = hitted.collider.GetComponent<Enemy>();

                if (enemy != null ) {
                    enemy.Dead();

                }
            
        }
 
    }
}

