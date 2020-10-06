using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyPos : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player_Owl").transform;
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(player.position.x, 
            gameObject.transform.position.y, 0);
    }
}
