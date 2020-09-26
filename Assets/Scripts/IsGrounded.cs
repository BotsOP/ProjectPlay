using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public BasicMovement basicMovement;
    // Start is called before the first frame update
    void Start()
    {
        basicMovement = gameObject.transform.parent.gameObject.GetComponent<BasicMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        basicMovement.isGrounded = true;
    }
    void OnTriggerExit(Collider other)
    {
        basicMovement.isGrounded = false;
    }
}
