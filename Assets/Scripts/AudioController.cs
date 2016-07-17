using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public float menuVolume;
    public float musicVolume;
    public AudioSource menuSound;
    public AudioSource backgroundMusic;

    private float startTime = 0;

    public void PlaySfx(AudioClip clip) {
        if ((Time.time - startTime) >= 0.05) {
            startTime = Time.time;
            menuSound.PlayOneShot(clip);
        }                
    }

    public void SetMenuVolume(float volume) {
        menuVolume = volume;
        menuSound.volume = volume;
    }

    public void SetMusicVolume(float volume) {
        musicVolume = volume;
        backgroundMusic.volume = volume;
    }
}