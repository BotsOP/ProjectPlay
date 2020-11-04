using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    public PlayerShoot playerShoot;
    // Start is called before the first frame update
    void Start()
    {
        playerShoot = GameObject.Find("player").GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "player")
        {
            playerShoot.ammo++;
            Destroy(gameObject);
        }
        
    }
}
