using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public bool gameIsActive = false;
	public float fallSpeed = 1f;

	public Sprite[] blockTextures;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}
