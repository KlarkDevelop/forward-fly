using UnityEngine;

public class ElectricTrail : MonoBehaviour
{
    private Material mat;
    public float delay = 0.2f;
    private float reset;

    private void Start()
    {
        mat = GetComponent<TrailRenderer>().material;
        reset = delay;
    }

    private void Update()
    {
        delay -= Time.deltaTime;
        if(delay < 0)
        {
            delay = reset;
            mat.mainTextureOffset = new Vector2(Random.Range(0, 1f), 0);
            mat.mainTextureScale = new Vector2(Random.Range(0.3f, 1), 1);
        }
    }
}
