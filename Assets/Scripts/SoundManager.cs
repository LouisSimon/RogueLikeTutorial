using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public const float LOW_PITCH_RANGE = .95f;
    public const float HIGH_PITCH_RANGE = 1.05f;

    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

	// Use this for initialization
	void Awake () {
	    if (instance == null)
	    {
	        instance = this;
	    }
	    else if(instance != this)
	    {
	        Destroy(gameObject);
	    }

        DontDestroyOnLoad(gameObject);
	}

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LOW_PITCH_RANGE, HIGH_PITCH_RANGE);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}
