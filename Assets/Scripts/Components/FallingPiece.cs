using UnityEngine;
using UnityEngine.InputSystem;

public class FallingPiece : GridTransform
{
	private GameManager gm;
	private SudokuBoard board;

	public GameObject block;

	private InputAction move;
	private float previousMoveValue;
	private float timeSincePreviousHorizontalMove = 0f;

	public float moveSpeed = 1f;
	private float timeSinceLastBlockFell;

	public BlockStruct[] shape = new BlockStruct[]
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
		timeSinceLastBlockFell = 0f;

		CreatePiece();
	}

	// Update is called once per frame
	public override void Update()
	{
		base.Update();
		if (board == null)
		{
			return;
		}

		timeSinceLastBlockFell += Time.deltaTime;
		if (timeSinceLastBlockFell >= board.fallSpeed)
		{
			if (!Blockf.DoBlocksCollide(Blockf.MoveBlocks(shape, x, y - 1), board.blocksOnBoard, board.boardSize))
			{
				y -= 1;
			}
			else
			{
				// Move blocks to blocksOnBoard
				board.AnchorBlocks(Blockf.MoveBlocks(shape, x, y));

				// Create new piece
				CreatePiece();
			}
			timeSinceLastBlockFell = 0f;
		}

		Vector2 moveValue = move.ReadValue<Vector2>();
		timeSincePreviousHorizontalMove += Time.deltaTime;
		if (previousMoveValue != moveValue.x || timeSincePreviousHorizontalMove > moveSpeed * 0.1f) {
			timeSincePreviousHorizontalMove = 0f;
			previousMoveValue = moveValue.x;
			// Will be replaced with an actual "does position collide" checker, but for now:
			if ( !Blockf.DoBlocksCollide( Blockf.MoveBlocks( shape, x + (int)moveValue.x, y ), board.blocksOnBoard, board.boardSize ) )
			{
				x += (int)moveValue.x;
			}
		}
	}

	void CreatePiece()
	{
		SetPosition( (int)Mathf.Floor(board.boardSize / 2), board.boardSize + 4, true );
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		int shapeSize = shape.Length;
		for (int i = 0;  i < shapeSize; i++)
		{
			BlockStruct part = shape[i];
			GameObject newBlock;
			newBlock = Instantiate(block);
			Block theBlock = newBlock.GetComponent<Block>();
			newBlock.transform.parent = transform;
			newBlock.transform.localScale = new Vector3(1f, 1f, 1f);
			newBlock.transform.localPosition = new Vector3(part.x, part.y, 0);
			newBlock.GetComponent<Block>().SetValue(part.value);
			theBlock.SetPosition(part.x, part.y, true);
			shape[i].block = theBlock;


		}
	}


}
