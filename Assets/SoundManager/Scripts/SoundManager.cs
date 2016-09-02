///
/// Original created by : Damar Inderajati
///

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DI.SoundManager
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundManager : MonoBehaviour {

		public static SoundManager instance = null;

		SoundData data;
		AudioSource sources;
		Queue<AudioSource> sfxSources = new Queue<AudioSource>();

		public bool autoChangeBGMusic = true;
		public bool restartBGMusicWhenLoaded = false;
		public int soundFxSourceSize = 10;

		void Awake() {
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);

			DontDestroyOnLoad(gameObject);
		}

		void Start() {
			sources = GetComponent<AudioSource>();
			data = Resources.Load("SoundData") as SoundData;

			for (int i = 0; i < soundFxSourceSize; i++)
			{
				GameObject sfxSource = new GameObject("SfxSource" + i.ToString());
				sfxSource.transform.parent = transform;
				AudioSource audio = sfxSource.AddComponent<AudioSource>();
				audio.spatialBlend = 0;
				audio.playOnAwake = false;
				audio.loop = false;

				sfxSources.Enqueue(audio);
			}

			if (autoChangeBGMusic)
			{
				if (data.GetBackgroundMusic(SceneManager.GetActiveScene().name) == sources.clip && !restartBGMusicWhenLoaded) {
					//DO NOTHING
				}else
					ChangeBackgroundMusic(SceneManager.GetActiveScene().name);
			}

		}

		void OnLevelWasLoaded(int index) {
			if (autoChangeBGMusic)
			{
				if (data.GetBackgroundMusic(SceneManager.GetActiveScene().name) == sources.clip && !restartBGMusicWhenLoaded)
					return;
				ChangeBackgroundMusic(SceneManager.GetActiveScene().name);
			}
		}

		public void ChangeBackgroundMusic(string id) {
			AudioClip clip = data.GetBackgroundMusic(id);
			if (clip) {
				sources.clip = clip;
				sources.Play();
			}
		}
		public void ChangeBackgroundMusic(AudioClip clip)
		{
			if (clip)
			{
				sources.clip = clip;
				sources.Play();
			}
		}
		public void PlayClip(string id, Vector3 position) {
			AudioClip clip = data.GetSoundFX(id);
			if (clip)
			{
				AudioSource.PlayClipAtPoint(clip, position);
			}
		}
		public void PlayClip2D(string id)
		{
			AudioClip clip = data.GetSoundFX(id);
			if (clip)
			{
				AudioSource audio = sfxSources.Dequeue();
				audio.clip = clip;
				audio.Play();
			}
		}
		public void PlayClip2D(AudioClip clip)
		{
			if (clip)
			{
				AudioSource audio = sfxSources.Dequeue();
				audio.clip = clip;
				audio.Play();
			}
		}


	}
}