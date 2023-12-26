using UnityEngine;
using UnityEngine.Events;

public class Canon : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform pointShoot;
    [SerializeField] private Bullet bullet;
    [SerializeField] private ParticleSystem spark;

    public static UnityEvent onShot = new UnityEvent();

    private void Start()
    {
        anim = GetComponent<Animator>();
        timer = fireRate;
    }

    public float fireRate;
    private float timer;
    public float delayStart;
    private void Update()
    {
        if(delayStart > 0)
        {
            delayStart -= Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                onShot.Invoke();
                GameObject newBullet = Instantiate(bullet.gameObject, pointShoot.transform.position, Quaternion.identity);
                newBullet.GetComponent<Bullet>().direction = transform.up;
                Destroy(newBullet, 10);
                spark.Play();

                anim.SetTrigger("Shoot");
                timer = fireRate;
            }
        }
    }
}
