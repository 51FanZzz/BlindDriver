using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip[] bgmClips;

    [Range(0f, 1f)]
    public float volume = 0.5f; 

    private int currentTrack = 0;
    private AudioSource audioSource;

    void Start(){
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        PlayNextTrack();
    }

    void PlayNextTrack(){
        if (bgmClips.Length == 0) return;

        audioSource.clip = bgmClips[currentTrack];
        audioSource.volume = volume;
        audioSource.Play();

        Invoke(nameof(PlayNextTrack), audioSource.clip.length);
        currentTrack = (currentTrack + 1) % bgmClips.Length;
    }

    //real-time adjustments will be applied
    void update(){
        audioSource.volume = volume;
    }
}
