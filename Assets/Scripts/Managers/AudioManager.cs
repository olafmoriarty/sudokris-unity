using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	[SerializeField] Transform mainCamera;
	[SerializeField] AudioClip[] sounds;
	[SerializeField] AudioSource source;

	public enum SoundFX
	{
		Explosion = 0,
	}


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void PlaySound(SoundFX soundNumber, float volume = 1f)
	{
		AudioSource audioSource = Instantiate(source, mainCamera.position, Quaternion.identity);
		audioSource.clip = sounds[(int)soundNumber];
		audioSource.Play();
		Destroy(audioSource.gameObject, audioSource.clip.length);
	}
}
