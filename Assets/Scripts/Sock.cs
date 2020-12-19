using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sock : MonoBehaviour
{
    private TrailRenderer trail;
    private Rigidbody2D rb;
    private Animator anim;
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
    private AudioSource audioSource;
    [SerializeField] AudioClip flyingClip;
    [SerializeField] float trailSpeed;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();

        CheckpointManager.instance.checkpointLocation = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.instance.Paused)
            return;

        movement = Input.GetAxisRaw("Horizontal");


        grounded = !cane.connected && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
        anim.SetBool("Grounded", grounded);

        overturned = !cane.connected && (
                     Physics2D.OverlapCircle(overturnedDetectorLeft.position, overturnCheckRadius, LayerMask.GetMask("Ground"))
                  || Physics2D.OverlapCircle(overturnedDetectorRight.position, overturnCheckRadius, LayerMask.GetMask("Ground"))
                  || Physics2D.OverlapCircle(overturnedDetectorUp.position, overturnCheckRadius, LayerMask.GetMask("Ground")));
        anim.SetBool("Overturned", overturned);

        if (!cane.connected && !flying && !overturned)
        {
            if (rb.velocity.x < -0.1f)
                this.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            else if (rb.velocity.x > 0.1f)
                this.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }

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
        
        anim.SetBool("Walking", movement != 0);

        trail.emitting =  rb.velocity.magnitude > trailSpeed && (cane.connected || flying);

    }

    public void SetHooked()
    {
        anim.SetBool("Hooked", cane.connected);
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

    public void PlayFlyingSound()
    {
        audioSource.PlayOneShot(flyingClip);
    }

    public void Reset(Vector3 pos) 
    {
        this.transform.position = pos;
        rb.velocity = Vector2.zero;
    }
}
