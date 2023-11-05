using System.Collections;
using UnityEngine;

//Sample Singleton pattern
public class AudioManager : MonoBehaviour
{
	public const string c_volumePresetKey = "Volume";

	private Coroutine _coroutine;
	private float _volume;

	private static AudioManager _instance;

	[SerializeField]
	private AudioSource _backgroundSource;
	[SerializeField]
	private AudioSource _attackSource;
	[SerializeField]
	private AudioSource _eventsSource;
	[SerializeField]
	private AudioSource _pigSource;

	[SerializeField]
	private AudioClip _click;
	[SerializeField]
	private AudioClip _noEnergy;
	[SerializeField]
	private AudioClip _win;
	[SerializeField]
	private AudioClip _lose;
	[SerializeField]
	private AudioClip _pig;
	[SerializeField]
	private float _pigDuration;
	
	public static float Volume
	{
		get => _instance._volume;
		set
		{
			_instance._volume = value;
			_instance._backgroundSource.volume = value;
			_instance._attackSource.volume = value;
			_instance._eventsSource.volume = value;
		}
	}

	public static void SetSoundtrack(AudioClip sound)
		=> _instance._backgroundSource.clip = sound;
	
	public static void PlayAttack()
		=> _instance._attackSource.Play();

	public static void StopAttack()
		=> _instance._attackSource.Stop();

	public static void PlayEventClick()
		=> _instance._eventsSource.PlayOneShot(_instance._click);
	public static void PlayEventNoEnergy()
		=> _instance._eventsSource.PlayOneShot(_instance._noEnergy);
	public static void PlayEventWin()
		=> _instance._eventsSource.PlayOneShot(_instance._win);
	public static void PlayEventLose()
		=> _instance._eventsSource.PlayOneShot(_instance._lose);
	public static void PlayEventPig() //todo
	{
		if(_instance._coroutine != null)
			_instance.StopCoroutine(_instance._coroutine);
		_instance._coroutine = _instance.StartCoroutine(_instance.Pig());
	}

	private IEnumerator Pig()
	{
		_pigSource.Stop();
		_pigSource.Play();
		yield return new WaitForSeconds(_pigDuration);
		_pigSource.Stop();
		_coroutine = null;
	}
	
	private void Awake()
	{
		_instance = this;

		Volume = PlayerPrefs.GetFloat(c_volumePresetKey, 0.3f);
		DontDestroyOnLoad(gameObject);
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetFloat(c_volumePresetKey, Volume);
	}
}