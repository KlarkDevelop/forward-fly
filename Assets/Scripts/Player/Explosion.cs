using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private MeshRenderer mesh;
    private SphereCollider colliderEx;

    [SerializeField] private GameObject crashedGr;

    private AudioSource _audioSource;
    private IEnumerator cor;
    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 7);
        Physics.IgnoreLayerCollision(6, 7);
        mesh = GetComponent<MeshRenderer>();
        colliderEx = GetComponent<SphereCollider>();
        colliderEx.enabled = false;
        mesh.enabled = false;
        startR = transform.localScale;
        Player.onDoubleTapJoystick.AddListener(BlowUp);
        Player.onPickUpShield.AddListener(ActiveShield);

        shieldBar.localScale = new Vector3(0.01f, 1, 1);

        _audioSource = GetComponent<AudioSource>();
    }

    public float radius;
    [SerializeField] private float doration;
    private void BlowUp(Player pl)
    {
        cor = ChangeRadius(radius, pl);
        if (workCoroutine == false && workShield == false) StartCoroutine(cor);
    }

    private Color startCol;
    private Vector3 startR;
    private bool workCoroutine = false;
    [SerializeField] private AudioClip expl;
    private IEnumerator ChangeRadius(float radius, Player pl)
    {
        workCoroutine = true;
        _audioSource.PlayOneShot(expl);
        colliderEx.enabled = true;
        mesh.enabled = true;
        pl.AddBombs(-1);

        startCol = mesh.material.color;
        float t = 0;

        while( t < 1 )
        {
            transform.localScale = Vector3.Lerp(startR, new Vector3(radius, radius, radius), t*t);
            mesh.material.color = Color.Lerp(startCol, new Color(256, 256, 256, 0), t * t);
            t += Time.deltaTime / doration;
            yield return null;
        }

        transform.localScale = startR;
        mesh.material.color = startCol;

        colliderEx.enabled = false;
        mesh.enabled = false;
        workCoroutine = false;
    }

    public float ForceObjects;
    private GameObject newBlock;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            if(workCoroutine == false)
            {
                _audioSource.Play();
            }
            
            newBlock = Instantiate(crashedGr, other.transform.position, crashedGr.transform.rotation);

            StartCoroutine(destroyBlock(newBlock));

            Destroy(other.gameObject);
        }
        if(other.tag == "ExploudingItem" || other.tag == "Spike" || other.CompareTag("Pila"))
        {
            other.gameObject.layer = 3;
            Propeler child = other.GetComponentInChildren<Propeler>();
            if (child != null)
            {
                child.gameObject.layer = 3;
            }
            if (other.gameObject.GetComponent<Rigidbody>() == null) 
            {
                Rigidbody r = other.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
                Vector3 vec;

                if (other.transform.parent.CompareTag("Platform"))
                {
                    other.transform.parent.transform.parent.GetComponent<MovingPlatform>().enabled = false;
                }

                if (other.TryGetComponent<ParticleSystem>(out ParticleSystem p)) p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                if (other.gameObject.transform.parent.TryGetComponent<Canon>(out Canon c))
                {
                    c.enabled = false;
                    c.GetComponent<Animator>().enabled = false; //TODO: возможно это стоит оптимизировать.
                }

                if (other.transform.position.x <= transform.position.x)
                {
                    vec = Vector3.left;
                }
                else
                {
                    vec = Vector3.right;
                }

                if (other.gameObject.TryGetComponent<Animator>( out Animator anim)) anim.enabled = false;
                r.AddForce(vec * ForceObjects, ForceMode.Impulse);
                StartCoroutine(destroyBlock(other.gameObject));
            }
        }
    }

    [Header("Shield settings")]
    [SerializeField] private Transform shieldBar;
    public float extraSpeed = 2;
    public float radiusShield = 1;
    public float timeShield = 3;
    public float speedActive = 0.7f;
    public Color colorShield = new Color();

    public void UseUpgradeLvl(int lvlExpl, int lvlShield)
    {
        switch (lvlExpl)
        {
            default:break;
            case 2: radius = 1.5f; break;
            case 3: radius = 2; break;
            case 4: radius = 4; break;
        }
        switch (lvlShield)
        {
            default: break;
            case 2: timeShield = 3.3f; extraSpeed = 4; break;
            case 3: timeShield = 4; extraSpeed = 6; break;
            case 4: timeShield = 5; extraSpeed = 8; break;
        }
    }

    private void ActiveShield(Player pl)
    {
        if (workCoroutine == true)
        {
            StopCoroutine(cor);
            workCoroutine = false;
        }
        if (workShield == false)
        {
            StartCoroutine(ChangeShield(pl));
        }
        else
        {
            if(inStep3 == true)
            {
                fix = true;
            }
            timer = timeShield;
        }

    }

    private bool workShield = false;

    private float timer = 0;
    private bool inStep3 = false;
    private bool fix = false;

    private IEnumerator ChangeShield(Player pl)
    {
        workShield = true;
        colliderEx.enabled = true;
        mesh.enabled = true;
        Color startCol = mesh.material.color;
        mesh.material.color = colorShield;
        pl.speed += extraSpeed;
        pl.speedHealthDown -= 0.05f;
        timer = timeShield;

    Continiu:
        fix = false;
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.Lerp(startR, new Vector3(radiusShield, radiusShield, radiusShield), t * t);
            t += Time.deltaTime / speedActive;
            yield return null;
        }
    
        while (timer > 0)
        {   
            timer -= Time.deltaTime;
            shieldBar.localScale = new Vector3(Mathf.Lerp(0.01f, 7.12f, timer/timeShield), 1, 1);
            yield return null;
        }

        Vector3 startR2 = transform.localScale;
        t = 0;

        while (t < 1)
        {
            inStep3 = true;
            if(fix == true)
            {
                goto Continiu;
            }
            transform.localScale = Vector3.Lerp(startR2, startR, t * t);
            t += Time.deltaTime / speedActive;
            yield return null;
        }
        inStep3 = false;
        mesh.material.color = startCol;

        pl.speedHealthDown += 0.05f;
        pl.speed -= extraSpeed;
        mesh.enabled = false;
        colliderEx.enabled = false;
        workShield = false;
    }
    private IEnumerator destroyBlock(GameObject block)
    {
        yield return new WaitForSeconds(4f);

        Destroy(block);
    }
}
