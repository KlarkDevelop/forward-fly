using UnityEngine;

public class RecordWriter : MonoBehaviour
{
    public int record;
    private float oldY;
    [SerializeField] private TMPro.TMP_Text[] counters;

    private void Start()
    {
        oldY = transform.position.y;
    }

    private void Update()
    {
        if(transform.position.y > oldY)
        {
            record = (int)Mathf.Round(transform.position.y);
            for(int i = 0; i < counters.Length; i++)
            {
                counters[i].text = record.ToString();
            }
            oldY = transform.position.y;
        }
    }
}
