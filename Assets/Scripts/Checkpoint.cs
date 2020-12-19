using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    private Collider2D col;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        animator = this.transform.parent.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        col.enabled = false;
        animator.SetTrigger("Destroy");
        CheckpointManager.instance.checkpointLocation = this.transform.position;
        audioSource.Play();
        Destroy(this.gameObject, 0.5f);
    }
}
