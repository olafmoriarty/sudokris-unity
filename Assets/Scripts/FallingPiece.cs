using UnityEngine;
using UnityEngine.InputSystem;

public class FallingPiece : MonoBehaviour
{
	private GameManager gm;
	private SudokuBoard board;

	public GameObject block;
	public Block[] blockConfiguration;

	private InputAction move;
	private float previousMoveValue;
	private float timeSincePreviousHorizontalMove = 0f;

	private GridTransform gridTransform;

	public float moveSpeed = 1f;
	private float timeSinceLastBlockFell;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		gm = GameManager.instance;
		board = GetComponentInParent<SudokuBoard>();

		move = InputSystem.actions.FindAction("Move");
		gridTransform = GetComponent<GridTransform>();
		timeSinceLastBlockFell = 0f;

		CreatePiece();
	}

	// Update is called once per frame
	void Update()
	{
		if (board == null)
		{
			return;
		}

		timeSinceLastBlockFell += Time.deltaTime;
		if (timeSinceLastBlockFell >= board.fallSpeed && gridTransform.y > 0)
		{
			gridTransform.y -= 1;
			timeSinceLastBlockFell = 0f;
		}

		Vector2 moveValue = move.ReadValue<Vector2>();
		timeSincePreviousHorizontalMove += Time.deltaTime;
		if (previousMoveValue != moveValue.x || timeSincePreviousHorizontalMove > moveSpeed * 0.1f && gridTransform != null) {
			timeSincePreviousHorizontalMove = 0f;
			previousMoveValue = moveValue.x;
			// Will be replaced with an actual "does position collide" checker, but for now:
			int newX = gridTransform.x + (int)moveValue.x;
			if (newX >= 0 && newX < board.boardSize)
			{
				gridTransform.x += (int)moveValue.x;
			}
		}
	}

	void CreatePiece()
	{
		// TODO Tidy up already existing piece

		GameObject newBlock;
		newBlock = Instantiate(block);
		newBlock.transform.parent = transform;
		newBlock.transform.localScale = new Vector3(1f, 1f, 1f);
		newBlock.transform.localPosition = new Vector3(1f, 0, 0);

		GameObject newBlock2;
		newBlock2 = Instantiate(block);
		newBlock2.transform.parent = transform;
		newBlock2.transform.localScale = new Vector3(1f, 1f, 1f);
		newBlock2.transform.localPosition = new Vector3(0, 0, 0);

		GameObject newBlock3;
		newBlock3 = Instantiate(block);
		newBlock3.transform.parent = transform;
		newBlock3.transform.localScale = new Vector3(1f, 1f, 1f);
		newBlock3.transform.localPosition = new Vector3(0, 1f, 0);

		GameObject gm = GameObject.Find("GameManager");
		newBlock.GetComponent<SpriteRenderer>().sprite = gm.GetComponent<GameManager>().blockTextures[2];

	}


}
