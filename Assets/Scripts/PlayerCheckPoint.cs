using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheckPoint : MonoBehaviour
{
    public Vector3 playerRespawn;
    public bool shouldStopMovement;
    Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "RespawnFloor")
        {
            
            SceneManager.LoadScene(scene.name);
            //transform.position = playerRespawn;
            StartCoroutine("StopMovement");
        }
    }

    private IEnumerator StopMovement()
    {
        shouldStopMovement = true;
        yield return new WaitForSeconds(0.2f);
        shouldStopMovement = false;
    }
}
