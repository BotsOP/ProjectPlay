using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    public float raycastDistance;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        
    }

    void FixedUpdate()
    {
    }

    private void Move()
    {
        Vector2 xMov = new Vector2(Input.GetAxisRaw("Horizontal") * transform.right.x, Input.GetAxisRaw("Horizontal") * transform.right.z);
        Vector2 zMov = new Vector2(Input.GetAxisRaw("Vertical") * transform.forward.x, Input.GetAxisRaw("Vertical") * transform.forward.z);

        Vector2 velocity = (xMov + zMov).normalized * speed * 100 * Time.deltaTime;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            Debug.Log("jumped");
        }
    }
    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.blue);
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance);
    }
}
