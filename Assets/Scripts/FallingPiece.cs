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

	private BlockStruct[] shape = new BlockStruct[]
	{
		new BlockStruct( 0, 0, 0 ),
		new BlockStruct( 1, 0, 1 ),
		new BlockStruct( -1, 0, 2) ,
	};

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
		if (timeSinceLastBlockFell >= board.fallSpeed)
		{
			if (!Blockf.DoBlocksCollide(Blockf.MoveBlocks(shape, gridTransform.x, gridTransform.y - 1), board.blocksOnBoard, board.boardSize))
			{
				gridTransform.y -= 1;
			}
			else
			{
				// Move blocks to blocksOnBoard

				// Create new piece
				CreatePiece();
			}
			timeSinceLastBlockFell = 0f;
		}

		Vector2 moveValue = move.ReadValue<Vector2>();
		timeSincePreviousHorizontalMove += Time.deltaTime;
		if (previousMoveValue != moveValue.x || timeSincePreviousHorizontalMove > moveSpeed * 0.1f && gridTransform != null) {
			timeSincePreviousHorizontalMove = 0f;
			previousMoveValue = moveValue.x;
			// Will be replaced with an actual "does position collide" checker, but for now:
			if ( !Blockf.DoBlocksCollide( Blockf.MoveBlocks( shape, gridTransform.x + (int)moveValue.x, gridTransform.y ), board.blocksOnBoard, board.boardSize ) )
			{
				gridTransform.x += (int)moveValue.x;
			}
		}
	}

	void CreatePiece()
	{
		gridTransform.SetPosition( (int)Mathf.Floor(board.boardSize / 2), board.boardSize + 5, true );
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		foreach (BlockStruct part in shape)
		{
			GameObject newBlock;
			newBlock = Instantiate(block);
			newBlock.transform.parent = transform;
			newBlock.transform.localScale = new Vector3(1f, 1f, 1f);
			newBlock.transform.localPosition = new Vector3(part.x, part.y, 0);
			newBlock.GetComponent<Block>().SetValue(part.value);
		}
	}


}
