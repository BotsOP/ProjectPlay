using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    PlayerCheckPoint playerCheckPoint;
    // Start is called before the first frame update
    void Start()
    {
        playerCheckPoint = GameObject.Find("player").GetComponent<PlayerCheckPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "player")
        {
            playerCheckPoint.playerRespawn = transform.position;
        }
    }
}
