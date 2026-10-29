using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake() {
        if (instance == null) { // Prevent multiple AudioManagers from spawning
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    
    // Enables overlapping sound
    public void PlaySound(AudioClip clip, float volume, float startTime = 0f) {
        if (clip == null) return;

        // Temp object
        GameObject tempGO = new GameObject("TempAudio_" + clip.name);

        // Add and adjust audio 
        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.ignoreListenerPause = true; // Bypass time pauses
        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.time = startTime;

        tempSource.Play(); // Play sound
        Destroy(tempGO, clip.length); // Destroy object after sound completes
    }
}
