using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnemyScript : MonoBehaviour
{
    public bool eventDone;
    public List<GameObject> enemy_list;


    void Start()
    {
        eventDone = false;
    }

    void Update()
    {
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!eventDone)
            {
                foreach (GameObject enemy in enemy_list)
                {
                    enemy.SetActive(true);
                }
                eventDone = true;
            }
        }
    }
}
