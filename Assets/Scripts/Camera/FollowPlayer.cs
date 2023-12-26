using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    private float pointMove;
    private float OffsetY;

    private void Start()
    {
        OffsetY = transform.position.y;
    }
    private void Update()
    {
        if(player.position.y > pointMove)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + OffsetY, transform.position.z);
            pointMove = player.position.y;
        }
    }
}
