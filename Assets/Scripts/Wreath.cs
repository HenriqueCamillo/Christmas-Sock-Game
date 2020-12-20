using UnityEngine;

public class Wreath : MonoBehaviour
{
    [SerializeField] Transform[] stopPoints;
    [SerializeField] float speed = 1f;
    int index = 0;

    private void Start() 
    {
        this.transform.parent.position = stopPoints[0].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(stopPoints[index].position, this.transform.parent.position) < 0.25f)
            index = (index + 1) % stopPoints.Length;

        this.transform.parent.position = Vector2.MoveTowards(this.transform.parent.position, stopPoints[index].position, speed * Time.fixedDeltaTime);
    }
}
