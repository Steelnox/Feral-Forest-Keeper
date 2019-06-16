using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{

    public GameObject continued;
    public GameObject credits;

    public float timer;

    void Start()
    {
        
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 3f)
        {
            continued.SetActive(false);
            credits.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
