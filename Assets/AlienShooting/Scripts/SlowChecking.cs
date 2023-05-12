using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowChecking : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemy = col.GetComponent<Enemy>();  
        if (enemy != null)
        {
            enemy.CanSlow = true; 
        }
    }
}
