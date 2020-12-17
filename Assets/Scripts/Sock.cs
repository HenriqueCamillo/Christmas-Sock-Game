using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sock : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movement;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float swingForce;
    [SerializeField] float groundCheckRadius;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform overturnedDetectorLeft, overturnedDetectorRight, overturnedDetectorUp;
    [SerializeField] Cane cane;
    private bool grounded; 
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool flying;
    public bool overturned;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
        overturned = Physics2D.OverlapCircle(overturnedDetectorLeft.position, groundCheckRadius, LayerMask.GetMask("Ground"))
                  || Physics2D.OverlapCircle(overturnedDetectorRight.position, groundCheckRadius, LayerMask.GetMask("Ground"))
                  || Physics2D.OverlapCircle(overturnedDetectorUp.position, groundCheckRadius, LayerMask.GetMask("Ground"));

        if (grounded)
        {
            flying = false;
            if (Input.GetKeyDown(KeyCode.W))
                Jump();
        }
        else if (Input.GetKeyDown(KeyCode.W) && overturned && Mathf.Abs(rb.velocity.y) < 0.05f)
            Jump();
        
    }

    private void FixedUpdate()
    {
        if (cane.connected)
            rb.AddForce(this.transform.right * movement * swingForce);
        else if (!flying && !overturned)
            rb.velocity = new Vector2(movement * speed, rb.velocity.y);

        // if(rb.velocity.y < 0)
        //     rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        // else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        //     rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    [SerializeField] float liftUpSpeed;
    private float liftUpValue;

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        if (overturned)
        {
            CancelInvoke(nameof(LiftUp));
            liftUpValue = Vector2.SignedAngle(Vector2.up, this.transform.up) > 0 ? liftUpSpeed : -liftUpSpeed;
            InvokeRepeating(nameof(LiftUp), 0f, Time.fixedDeltaTime);
        }
    }


    private void LiftUp()
    {
        this.transform.Rotate(0f, 0f, liftUpValue);

        if (Vector2.Angle(Vector2.up, this.transform.up) < 1f)
            CancelInvoke(nameof(LiftUp));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(overturnedDetectorLeft.position, groundCheckRadius);
        Gizmos.DrawWireSphere(overturnedDetectorRight.position, groundCheckRadius);
        Gizmos.DrawWireSphere(overturnedDetectorUp.position, groundCheckRadius);
    }
}
