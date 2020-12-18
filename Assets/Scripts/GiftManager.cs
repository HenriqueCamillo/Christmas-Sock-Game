using UnityEngine;
using TMPro;

public class GiftManager : MonoBehaviour
{
    public static GiftManager instance;
    [SerializeField] TextMeshProUGUI text;
    private int gifts;
    private int total;

    public int Total
    {

        get => total;
        set 
        {
            total = value;
            text.SetText($"{gifts}/{total}");
        }
    }

    public int Gifts
    {
        get => gifts;
        set
        {
            gifts = value;
            text.SetText($"{gifts}/{total}");
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
