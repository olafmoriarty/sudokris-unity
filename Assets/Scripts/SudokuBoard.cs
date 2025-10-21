using UnityEngine;

public class SudokuBoard : MonoBehaviour
{
	public int boardSize = 9;
	private GameManager gm;
	public float fallSpeed;

	[SerializeField] Camera mainCamera;

	void Start()
	{
		gm = GameManager.instance;
		fallSpeed = gm.fallSpeed;
	}

	public void SetBoardSize(int newSize)
	{
		boardSize = newSize;
	}
}
