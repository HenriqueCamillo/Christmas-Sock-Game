using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Sock sock = null;
    [SerializeField] float impulseForce;
    private Animator anim;

    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (sock == null)
            sock = other.GetComponent<Sock>();
        
        sock.SpringImpulse();
        anim.SetTrigger("Bounce");
    }
}
