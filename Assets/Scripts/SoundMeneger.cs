using UnityEngine;
using System.Collections;

public class SoundMeneger : MonoBehaviour
{
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Canon.onShot.AddListener(PlaySoundCanon);
        Player.onDeath.AddListener(PlayDeathSound);
    }

    [SerializeField] private float delay = 0.1f;

    private void PlaySoundCanon()
    {
        if(workCanon == false)
        {
            StartCoroutine(CorCanon());
        }
    }

    private bool workCanon = false;
    [SerializeField] private AudioClip soundCannon;
    private IEnumerator CorCanon()
    {
        workCanon = true;
        _audioSource.PlayOneShot(soundCannon);
        yield return new WaitForSeconds(delay);
        workCanon = false;
    }

    [SerializeField] private AudioClip soundDeath;
    private void PlayDeathSound()
    {
        _audioSource.PlayOneShot(soundDeath);
    }
}
