using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 8);
        if(direction == Vector3.zero)
        {
            direction = Vector3.up;
        }
    }

    public Vector3 direction;
    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player pl))
        {
            pl.TakeDamage();
            Debug.Log("Hit");
        }
        Destroy(gameObject);
    }
}
