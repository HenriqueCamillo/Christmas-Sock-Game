using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    Sock sock = null;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (sock == null) 
            sock = other.GetComponent<Sock>();

        audioSource.Play();
        sock.Reset(CheckpointManager.instance.checkpointLocation);
    }
}
