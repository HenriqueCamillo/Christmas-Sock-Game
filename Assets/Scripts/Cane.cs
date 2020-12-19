using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cane : MonoBehaviour
{
    public bool connected;
    public HingeJoint2D wreath;
    private Rigidbody2D sockRb;
    private Collider2D col;
    [SerializeField] Sock sock;
    private Animator anim;
    private float blend;
    [SerializeField] float throwSpeed;
    [SerializeField] float retractSpeed;
    [SerializeField] float colliderThreshold;
    private float throwLerpValue;
    private float retractLerpValue;
    [SerializeField] float maxLenghtCooldown;
    [SerializeField] float lenght;

    private float Blend
    {
        get => blend;
        set
        {
            blend = value;
            anim.SetFloat("Blend", blend);

            if (col.enabled)
            {
                if (blend <  colliderThreshold)
                    col.enabled = false;
            }
            else
            {
                if (blend >=  colliderThreshold)
                    col.enabled = true;
            }
        }
    }

    void Start()
    {
        sockRb = sock.GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        col.enabled = false;
        throwLerpValue = Mathf.Lerp(0f, 1f, throwSpeed);
        retractLerpValue = Mathf.Lerp(0f, 1f, retractSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.instance.Paused)
            return;
            
        if (Input.GetMouseButtonDown(0))
        {
            if (connected)
                Disconnect();
            else
            {
                CancelInvoke();
                InvokeRepeating(nameof(Throw), 0f, Time.fixedDeltaTime);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!connected)
        {
            CancelInvoke();
            
            sock.airControlActive = false;
            wreath = other.GetComponent<HingeJoint2D>();
            Vector2 direction = wreath.transform.position - sock.transform.position;

            sock.transform.up = direction;
            wreath.connectedAnchor = new Vector2(0f, lenght * blend);
            wreath.connectedBody = sockRb;

            connected = true;
            sock.SetHooked();
            sock.flying = true;
        }
    }

    private void Throw()
    {
        Blend += throwLerpValue;

        if (Blend > 0.95f)
        {
            Blend = 1f;
            CancelInvoke(nameof(Throw));

            if (!connected)
                InvokeRepeating(nameof(Retract), maxLenghtCooldown, Time.fixedDeltaTime);
        }
        
        anim.SetFloat("Blend", Blend);
    }

    private void Retract()
    {
        if (connected)
        {
            CancelInvoke(nameof(Retract));
            return;
        }
        
        Blend -= retractLerpValue;

        if (Blend < 0.05f)
        {
            Blend = 0f;
            CancelInvoke(nameof(Retract));
        }

        anim.SetFloat("Blend", Blend);
    }

    void Disconnect()
    {
        connected = false;
        sock.SetHooked();
        wreath.connectedBody = null;
        InvokeRepeating(nameof(Retract), 0f, Time.fixedDeltaTime);
    }
}
