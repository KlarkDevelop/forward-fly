using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Player Control")]
    public float speed;
    [SerializeField] private Joystick joystick;

    [SerializeField] private Transform Trajectory;
    [SerializeField] private float rotateSpeed;

    private AudioSource _audioSource;
    private MeshRenderer trajectoryRenderer;
    private Vector3 vec;
    private void Start()
    {
        Physics.IgnoreLayerCollision(3, 6);
        vec = Vector3.up;
        trajectoryRenderer = Trajectory.GetComponentInChildren<MeshRenderer>();
        trajectoryRenderer.enabled = false;
        coins = SaveSystem.Instante.Save.coins;
        coinValue.text = coins.ToString();

        RewordedAd.onContinueAdComplet.AddListener(ContinueGame);
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        playerControlJoy();

        transform.Translate(Vector3.up * Time.deltaTime * speed);

        Trajectory.position = transform.position;
    }

    private float joyDistance;
    public float deathZone;
    private void playerControlJoy()
    {
        joyDistance = Vector2.Distance(Vector2.zero, new Vector2(joystick.Horizontal, joystick.Vertical));
        if (joyDistance > deathZone)
        {
            vec = (new Vector3(joystick.Horizontal, joystick.Vertical, 0)).normalized;
        }
        transform.up = Vector3.MoveTowards(transform.up, vec, Time.deltaTime * rotateSpeed);
        Trajectory.up = Vector3.MoveTowards(Trajectory.up, vec, 1);
    }

    public static UnityEvent<Player> onDoubleTapJoystick = new UnityEvent<Player>();
    private bool tap = false;
    public void ChekDoubleTap()
    {
        if(bomb > 0)
        {
            if (countWorkingCoroutin < 2) StartCoroutine(WaitSecondTap());
            tap = true;
        }
    }
    public float delayTap;
    private int countWorkingCoroutin = 0;
    private IEnumerator WaitSecondTap()
    {
        countWorkingCoroutin++;
        if (tap == true)
        {
            onDoubleTapJoystick.Invoke(this);
        }
        yield return new WaitForSecondsRealtime(delayTap);
        tap = false;
        countWorkingCoroutin--;
    }

    public void ShowTrajectory()
    {
        trajectoryRenderer.enabled = true;
    }
    public void HideTrajectory()
    {
        trajectoryRenderer.enabled = false;
    }

    [Header("Player Collider")]
    public float health = 1;
    public float bomb = 0;
    public int coins = 0;
    public float speedHealthDown = 0.1f;
    [SerializeField] private TMPro.TMP_Text bombValue;
    [SerializeField] private TMPro.TMP_Text coinValue;

    [SerializeField] private AudioClip soundDamage;
    [SerializeField] private AudioClip soundBonus;
    [SerializeField] private AudioClip soundCoin;
    [SerializeField] private AudioClip soundPila;
    [SerializeField] private AudioClip soundShield;

    public bool oneDeath = true;
    private void Update()
    {
        health -= Time.deltaTime * speedHealthDown;
        if(health < 0 && oneDeath)
        {
            Death();
            oneDeath = false;
        }
    }

    public static UnityEvent onDeath = new UnityEvent();
    [SerializeField]private bool OFFdeath = true;
    private void Death()
    {
        Debug.Log("Death");

        if(OFFdeath == false)
        {
            onDeath.Invoke();
            gameObject.SetActive(false);
        }
    }

    public static UnityEvent<Player> onPickUpShield = new UnityEvent<Player>();
    public static UnityEvent onBombPickuped = new UnityEvent();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            _audioSource.PlayOneShot(soundBonus);
            health = 1;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Bomb") && bomb < 3)
        {
            _audioSource.PlayOneShot(soundBonus);
            AddBombs(1);
            onBombPickuped.Invoke();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Coin"))
        {
            _audioSource.PlayOneShot(soundCoin);
            AddCoins(1);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Shield"))
        {
            _audioSource.PlayOneShot(soundShield);
            onPickUpShield.Invoke(this);
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Spike") && delayDmg == false)
        {
            if(delayDmg == false)
            {
                TakeDamage();
            }
            StartCoroutine(DelayDamage());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Pila"))
        {
            _audioSource.PlayOneShot(soundPila);
            speedHealthDown += 0.11f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Pila"))
        {
            speedHealthDown -= 0.11f;
        }
    }

    [SerializeField] private float defoltDamage = 0.10f;
    public void TakeDamage()
    {
        health -= defoltDamage;
        _audioSource.PlayOneShot(soundDamage);
    }

    private bool delayDmg = false;
    private IEnumerator DelayDamage()
    {
        delayDmg = true;
        yield return new WaitForSeconds(0.5f);
        delayDmg = false;
    }

    public void AddBombs(int value)
    {
        bomb += value;
        bombValue.text = bomb.ToString();
    }

    public void AddCoins(int value)
    {
        coins += value;
        coinValue.text = coins.ToString();
        SaveSystem.Instante.Save.coins = coins;
    }

    private void ContinueGame()
    {
        health = 1;
        oneDeath = true;
        gameObject.SetActive(true);
        onPickUpShield.Invoke(this);
    }
}
