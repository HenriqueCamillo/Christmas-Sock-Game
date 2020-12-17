using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    private Camera cam;
    [SerializeField] Cane cane;
    Vector2 destination;
    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        destination = cane.connected ? (Vector2)cane.wreath.transform.position : (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.up = destination - (Vector2)this.transform.position;
    }
}
