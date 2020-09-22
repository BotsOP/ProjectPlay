using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float moveSmoothTime = 0.03f;
    [SerializeField] float jumpForce = 5f;
    public float raycastDistance;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    Vector2 momentumDir;
    float momentumSpeed = 10;
    public Camera mainCamera;
    bool groundedStart;
    bool checkAcceleration = true;
    Vector2 playerPos;
    Vector2 tempPlayerPos;
    public Vector2 playerAcceleration;
    Vector2 tempPlayerAcceleration;

    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        StartCoroutine("CalculateAccelerationPlayer");
    }

    private void Move()
    {
        //Gets the input directions locally
        Vector2 xMov = new Vector2(Input.GetAxisRaw("Horizontal") * transform.right.x, Input.GetAxisRaw("Horizontal") * transform.right.z);
        Vector2 zMov = new Vector2(Input.GetAxisRaw("Vertical") * transform.forward.x, Input.GetAxisRaw("Vertical") * transform.forward.z);

        //Adds the z and x movements into one vector and normalizes it
        Vector2 targetDir = new Vector2(xMov.x + zMov.x, xMov.y + zMov.y).normalized;
        //Smooths the movements and multiplies it to make it useable
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        //rb.velocity causes there to be no slippery!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        if(IsGrounded()) 
        {
            
            Vector2 velocity = (currentDir) * speed * 200 * Time.deltaTime;

            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
            groundedStart = true;
        }
        else
        {
            if(groundedStart)
            {
                tempPlayerAcceleration = playerAcceleration;
                groundedStart = false;
            } 
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

            Vector2 velocity = (currentDir) * speed * 100 * Time.deltaTime;
            Vector2 momentumVel = velocity + tempPlayerAcceleration * Time.deltaTime * 10;
            
            rb.velocity = new Vector3(momentumVel.x, rb.velocity.y, momentumVel.y);
        }
    }

    private IEnumerator CalculateAccelerationPlayer()
    {
        if(checkAcceleration)
        {
            checkAcceleration = false;
            playerPos = new Vector2(transform.position.x, transform.position.z);

            yield return new WaitForSeconds(0.1f);
            tempPlayerPos = new Vector2(transform.position.x, transform.position.z);

            playerAcceleration = (tempPlayerPos - playerPos) * 100;
            //Debug.Log(playerAcceleration + "    " + tempPlayerPos + "    " + playerPos);
            checkAcceleration = true;
        }
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            Debug.Log("jumped");
        }
    }
    public bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.blue);
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance);
    }
}