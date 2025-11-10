using UnityEngine;
using UnityEngine.InputSystem;

public class FallingPiece : GridTransform
{
	private GameManager gm;
	private SudokuBoard board;

	public GameObject block;

	[SerializeField] InputActionAsset inputActions;
	private InputAction move;
	private InputAction rotate;
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

	void OnEnable()
	{
		inputActions.FindActionMap("Block").Enable();
	}

	void OnDisable()
	{
		inputActions.FindActionMap("Block").Disable();
	}

	void Awake()
	{
		move = InputSystem.actions.FindAction("MoveBlock");
		rotate = InputSystem.actions.FindAction("RotateClockwise");
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		gm = GameManager.instance;
		board = GetComponentInParent<SudokuBoard>();

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
				//AudioManager.instance.PlaySound( AudioManager.SoundFX.Explosion );
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

		int movement = (int)move.ReadValue<float>();
		if (movement != 0)
		{
			AttemptHorizontalMove(movement);
		}
		if (rotate.WasPressedThisFrame())
		{
			AttemptRotation(1);
		}
	}

	void AttemptHorizontalMove( int movement )
	{
		timeSincePreviousHorizontalMove += Time.deltaTime;
		if (previousMoveValue != movement || timeSincePreviousHorizontalMove > moveSpeed * 0.1f) {
			timeSincePreviousHorizontalMove = 0f;
			previousMoveValue = movement;
			// Will be replaced with an actual "does position collide" checker, but for now:
			if ( !Blockf.DoBlocksCollide( Blockf.MoveBlocks( shape, x + movement, y ), board.blocksOnBoard, board.boardSize ) )
			{
				x += movement;
			}
		}
	}

	void AttemptRotation( int rotations = 1 )
	{
		AudioManager.instance.PlaySound(AudioManager.SoundFX.Explosion);
		BlockStruct[] rotatedPiece = Blockf.RotatePiece(shape, rotations);
		if (!Blockf.DoBlocksCollide(Blockf.MoveBlocks(rotatedPiece, x, y), board.blocksOnBoard, board.boardSize))
		{
			shape = rotatedPiece;
			Blockf.PositionBlocks(rotatedPiece);			
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
