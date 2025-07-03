using UnityEngine;
using UnityEngine.Audio;
using System;
using Random = System.Random;

public class AudioManager : MonoBehaviour
{
	public AudioSource[] EffectsSources = new AudioSource[5];
	public AudioSource[] LoopSources = new AudioSource[1];
	public AudioSource MusicSource;

	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	// Singleton instance.
	public static AudioManager Instance = null;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip)
	{
		foreach(AudioSource source in EffectsSources)
        {
            if (!source.isPlaying)
            {
				source.clip = clip;
				source.Play();
				return;
			}
        }
	}

	public AudioSource PlayLoop(AudioClip clip)
    {
		foreach (AudioSource source in LoopSources)
		{
			if (!source.isPlaying)
			{
				source.clip = clip;
				source.Play();
				return source;
			}
		}
		Debug.Log("replacing last loopsource");
		LoopSources[LoopSources.Length].clip = clip;
		LoopSources[LoopSources.Length].Play();
		return LoopSources[LoopSources.Length];
		
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip)
	{
		MusicSource.clip = clip;
		MusicSource.Play();
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(params AudioClip[] clips)
	{
		//int randomIndex = Random.Range(0, clips.Length);
		//float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		//EffectsSource.pitch = randomPitch;
		//EffectsSource.clip = clips[randomIndex];
		//EffectsSource.Play();
	}
}
