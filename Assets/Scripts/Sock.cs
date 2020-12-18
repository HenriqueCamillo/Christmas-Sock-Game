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
    [SerializeField] float airControlForce;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float overturnCheckRadius;
    [SerializeField] float liftUpSpeed;
    private float liftUpValue;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform overturnedDetectorLeft, overturnedDetectorRight, overturnedDetectorUp;
    [SerializeField] Cane cane;
    private bool grounded; 
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool flying;
    public bool overturned;
    public bool airControlActive;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        grounded = !cane.connected && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
        overturned = !cane.connected && (
                     Physics2D.OverlapCircle(overturnedDetectorLeft.position, overturnCheckRadius, LayerMask.GetMask("Ground"))
                  || Physics2D.OverlapCircle(overturnedDetectorRight.position, overturnCheckRadius, LayerMask.GetMask("Ground"))
                  || Physics2D.OverlapCircle(overturnedDetectorUp.position, overturnCheckRadius, LayerMask.GetMask("Ground")));

        if (grounded)
        {
            flying = false;
            if (Input.GetKeyDown(KeyCode.W))
                Jump();
        }
        else if (overturned && Mathf.Abs(rb.velocity.y) < 0.05f)
        {
            flying = false;
            if (Input.GetKeyDown(KeyCode.W))
                Jump();
        }
        
    }

    private void FixedUpdate()
    {
        if (cane.connected)
            rb.AddForce(this.transform.right * movement * swingForce);
        else if (!flying && !overturned || airControlActive)
        {

            rb.velocity = new Vector2(movement * speed, rb.velocity.y);

            if(rb.velocity.y < 0)
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (flying && movement != 0f)
        {
            rb.AddForce(Vector2.right * movement * airControlForce);
        }
    }

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

    public void SpringImpulse()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        Gizmos.DrawWireSphere(overturnedDetectorLeft.position, overturnCheckRadius);
        Gizmos.DrawWireSphere(overturnedDetectorRight.position, overturnCheckRadius);
        Gizmos.DrawWireSphere(overturnedDetectorUp.position, overturnCheckRadius);
    }
}
