using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public bool gameIsActive = false;
	public float fallSpeed = 1f;

	public Sprite[] blockTextures;
	public string[] blockLabels;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}
