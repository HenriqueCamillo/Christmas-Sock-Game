using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Sock sock = null;
    [SerializeField] float impulseForce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (sock == null)
            sock = other.GetComponent<Sock>();
        
        sock.SpringImpulse();
    }
}
