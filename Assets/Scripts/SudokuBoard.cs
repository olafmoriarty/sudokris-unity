using UnityEngine;
using System.Collections.Generic;

public class SudokuBoard : MonoBehaviour
{
	public int boardSize = 9;
	private Vector2 sectionSize;
	private GameManager gm;
	public float fallSpeed;

	[SerializeField] Camera mainCamera;
	[SerializeField] Transform whiteSquare;
	[SerializeField] Transform lineContainer;
	[SerializeField] GameObject linePrefab;
	public List<BlockStruct> blocksOnBoard;
	public GameObject blockContainer;

	void Start()
	{
		gm = GameManager.instance;
		fallSpeed = gm.fallSpeed;
		SetBoardSize(9);

	}

	public void SetBoardSize(int newSize)
	{
		// Save new size to variable
		boardSize = newSize;

		sectionSize = Blockf.GetSectionSize(boardSize);

		// Scale and position white square
		whiteSquare.localScale = new Vector3(newSize, newSize, 1);
		whiteSquare.localPosition = new Vector3(((float)newSize / 2f) - 0.5f, ((float)newSize / 2f) - 0.5f, 1f);

		// Scale and position camera
		mainCamera.orthographicSize = ((float)newSize + 6f) / 2f;
		mainCamera.transform.position = new Vector3((float)newSize / 2f - 0.5f, ((float)newSize + 6f) / 2f - 0.5f, -10f);

		// Remove any existing lines
		foreach (Transform child in lineContainer)
		{
			Destroy(child.gameObject);
		}

		// Draw new lines
		for (int i = 0; i <= newSize; i++)
		{
			GameObject horizontalLine = Instantiate(linePrefab);
			horizontalLine.transform.parent = lineContainer;
			horizontalLine.transform.localPosition = new Vector3(-0.5f, -0.5f + i, 0);
			horizontalLine.transform.localScale = new Vector3(1, 1, newSize);

			if (i % sectionSize.y == 0)
			{
				LineRenderer lr = horizontalLine.GetComponent<LineRenderer>();
				lr.startWidth = 0.1f;
				lr.endWidth = 0.1f;
			}

			GameObject verticalLine = Instantiate(linePrefab);
			verticalLine.transform.parent = lineContainer;
			verticalLine.transform.localPosition = new Vector3(-0.5f + i, -0.5f, 0);
			verticalLine.transform.rotation = Quaternion.Euler(270, 0, 0);
			verticalLine.transform.localScale = new Vector3(1, 1, newSize);

			if (i % sectionSize.x == 0)
			{
				LineRenderer lr = verticalLine.GetComponent<LineRenderer>();
				lr.startWidth = 0.1f;
				lr.endWidth = 0.1f;
			}
		}
	}

	public void AnchorBlocks(Block[] blocks)
	{
		foreach (Block block in blocks)
		{
			block.transform.parent = blockContainer.transform;
		}
	}
}
