using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float DeleteTime = 2F;

    private float _startTime;

    void Update()
    {
        _startTime += Time.deltaTime;

        if (_startTime > DeleteTime)
        {
            Destroy(gameObject);
        }
    }
}
