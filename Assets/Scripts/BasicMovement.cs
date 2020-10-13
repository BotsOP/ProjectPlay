using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float moveSmoothTime = 0.03f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpMomentumForce = 5;
    public float raycastDistance;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    public Camera mainCamera;
    bool groundedStart;
    bool slideStart;
    bool checkAcceleration = true;
    public bool lerpSlide;
    public float speedSlide;
    float timeSlide;
    public float timeToSlide;
    Vector2 playerPos;
    Vector2 tempPlayerPos;
    public Vector2 playerAcceleration;
    Vector2 tempPlayerAcceleration;
    Vector2 tempPlayerAccelerationSlide;
    public bool isGrounded;
    public Rigidbody rb;
    PlayerCheckPoint playerCheckPoint;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerCheckPoint = GetComponent<PlayerCheckPoint>();
        rb = GetComponent<Rigidbody>();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame

    //MAKE CAPSULE HAVE MORE WEIGHT SO THAT IT FALLS FASTER ITS IMPORTANT WAY TOO FLOATY
    void Update()
    {
        if(!playerCheckPoint.shouldStopMovement)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
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

        if(isGrounded) 
        {   
            Vector2 velocity = (currentDir) * speed * 200 * Time.deltaTime;
            if(!Input.GetKey(KeyCode.LeftControl))
            {
                rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
            }
            
            groundedStart = true;
            
        }

        //momentum when jumping
        else
        {
            if(groundedStart)
            {
                tempPlayerAcceleration = playerAcceleration;
                groundedStart = false;
            }

            Vector2 velocity = (currentDir) * speed * 100 * Time.deltaTime;
            Vector2 momentumVel = velocity + tempPlayerAcceleration * Time.deltaTime * jumpMomentumForce;
            
            rb.velocity = new Vector3(momentumVel.x, rb.velocity.y, momentumVel.y);
        }

        //sliding
        if(Input.GetKey(KeyCode.LeftControl))
        {
            if(slideStart && isGrounded)
            {
                tempPlayerAccelerationSlide = playerAcceleration;
                timeSlide = Time.time;
                speedSlide = 10;
                slideStart = false;
            }

            if(isGrounded)
            {
                Vector2 velocitySlide = (currentDir) * speed * 100 * Time.deltaTime;
                Vector2 momentumVel = velocitySlide + tempPlayerAccelerationSlide * Time.deltaTime * speedSlide;
                rb.velocity = new Vector3(momentumVel.x, rb.velocity.y, momentumVel.y);
            }

            if(Time.time > timeSlide + timeToSlide)
                lerpSlide = false;
            else
                lerpSlide = true;
                
        }
        else
            slideStart = true;
        if(lerpSlide)
        {
            speedSlide = Mathf.LerpUnclamped(0,8, (timeSlide - Time.time + timeToSlide) / timeToSlide);
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
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }
}