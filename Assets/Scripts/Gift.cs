using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GiftManager.instance.Gifts++;
        Destroy(this.gameObject);
    }
}
