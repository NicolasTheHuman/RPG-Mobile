using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource musicSource;
	private AudioSource SFXSource;

	private void Awake()
	{
		musicSource = gameObject.AddComponent<AudioSource>();
		SFXSource = gameObject.AddComponent<AudioSource>();

		musicSource.loop = true;
	}

	public void PlayMusic(AudioClip musicClip)
	{
		musicSource.volume = 1f;
		musicSource.clip = musicClip;
		musicSource.Play();
	}

	public void PlayMusic(AudioClip musicClip, float volume)
	{
		musicSource.clip = musicClip;
		musicSource.volume = volume;
		musicSource.Play();
	}

	public void PlaySFX(AudioClip clip)
	{
		SFXSource.PlayOneShot(clip);
	}

	public void PlaySFX(AudioClip clip, float volume)
	{
		SFXSource.PlayOneShot(clip, volume);
	}

	public void StopSFX(AudioClip clip)
	{
		if (SFXSource.clip == clip)
			SFXSource.Stop();
	}
}
