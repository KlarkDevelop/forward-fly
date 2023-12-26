using UnityEngine;

public class WheelTrail : MonoBehaviour
{
    private Material mat;
    private Player _player;

    private void Start()
    {
        mat = GetComponent<TrailRenderer>().material;
        _player = transform.parent.transform.parent.GetComponent<Player>();
        Debug.Log(_player.speed);
    }

    private void Update()
    {
        mat.mainTextureOffset += new Vector2(Time.deltaTime * -((_player.speed / 10)*2.2f), 0);
    }
}
