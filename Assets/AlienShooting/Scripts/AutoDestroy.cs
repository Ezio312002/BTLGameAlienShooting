using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float delay;

    void OnEnable()
    {
        DestroyTrigger();
    }

    public void DestroyTrigger()
    {
        Destroy(gameObject, delay);
    }
}
