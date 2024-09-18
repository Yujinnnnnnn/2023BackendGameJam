using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float EfxTime = 0.5f;
    public float UpdateTime = 0f;

    private void Awake()
    {
        UpdateTime = 0f;
        //Time ÈÄ Object »èÁ¦
        //Destroy(gameObject, Time);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime += Time.deltaTime;
        if (UpdateTime > EfxTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        UpdateTime = 0f;
    }
}
