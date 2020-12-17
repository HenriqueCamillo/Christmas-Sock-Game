using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftManager : MonoBehaviour
{
    public static GiftManager instance;
    [SerializeField] int gifts;
    public int Gifts
    {
        get => gifts;
        set
        {
            gifts = value;
            // TODO update UI
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }
}
