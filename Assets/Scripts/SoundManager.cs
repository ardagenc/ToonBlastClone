using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioResource buttonSfx;

    bool playMusic = false;
    bool playSFX = true;

    private void Start()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
    public void ToggleSound()
    {
        playMusic = !playMusic;

        if (playMusic)
        {
            musicSource.Stop();
        }
        else
        {
            musicSource.Play();
        }
    }
    public void ToggleSFX()
    {
        playSFX = !playSFX;
        sfxSource.enabled = playSFX;

    }

    public void PlaySFX()
    {
        sfxSource.resource = buttonSfx;
        sfxSource.Play();
    }
}
