using System.Collections.Generic;
using UnityEngine;

	//Sample Singleton pattern
public class AudioManager : MonoBehaviour
{
	public const string c_volumePresetKey = "Volume";

	private enum AudioSourceType : byte
	{
		None = 0,
		Tower = 1,
		Projectile = 2,
	}

	private float _volume;

	private static AudioManager _instance;
	private readonly Dictionary<AudioSourceType, AudioSource> _sources = new(8);

	[SerializeField]
	private AudioSource _towerSource;

	[SerializeField]
	private AudioSource _projectileSource;

	public static float Volume
	{
		get => _instance._volume;
		set
		{
			_instance._volume = value;
			//_towerSource.volume = value;
			//_projectileSource.volume = value;
		}
	}

	public static void PlayAttack(AudioClip clip)
		=> _instance.PlayClip(clip, AudioSourceType.Tower);

	public static void PlayHit(AudioClip clip)
		=> _instance.PlayClip(clip, AudioSourceType.Projectile);

	private void PlayClip(AudioClip clip, AudioSourceType type)
	{
		if (_sources.TryGetValue(type, out var source))
			source.PlayOneShot(clip);
	}



	private void Awake()
	{
		_instance = this;

		Volume = PlayerPrefs.GetFloat(c_volumePresetKey, 1f);

		DontDestroyOnLoad(gameObject);

		/*if(_towerSource != null)
			_sources.Add(AudioSourceType.Tower, _towerSource);
		if(_projectileSource != null)
			_sources.Add(AudioSourceType.Projectile, _projectileSource);*/
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetFloat(c_volumePresetKey, Volume);
	}
}