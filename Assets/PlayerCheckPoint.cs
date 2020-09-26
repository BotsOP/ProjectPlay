using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    public Vector3 playerRespawn;
    public bool shouldStopMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "RespawnFloor")
        {
            transform.position = playerRespawn;
            StartCoroutine("StopMovement");
        }
    }

    private IEnumerator StopMovement()
    {
        shouldStopMovement = true;
        yield return new WaitForSeconds(1);
        shouldStopMovement = false;
    }
}
