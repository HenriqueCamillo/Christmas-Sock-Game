using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    private Collider2D col;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        animator = this.transform.parent.GetComponent<Animator>();
        GiftManager.instance.Total++;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        col.enabled = false;
        animator.SetTrigger("Destroy");
        GiftManager.instance.Gifts++;
        audioSource.Play();
        Destroy(this.gameObject, 0.25f);
    }
}
