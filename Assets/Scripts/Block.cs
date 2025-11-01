using TMPro;
using UnityEngine;

public struct BlockStruct
{
	public int x;
	public int y;
	public int value;
	public Block block;

	public BlockStruct(int newX, int newY, int newValue)
	{
		x = newX;
		y = newY;
		value = newValue;
		block = null;
	}

	public BlockStruct(Block newBlock)
	{
		Vector2 coords = newBlock.GetCoords();
		x = (int)coords.x;
		y = (int)coords.y;
		value = newBlock.value;
		block = newBlock;
	}
}

public class Block : MonoBehaviour
{
	private GameManager gm;
	private TextMeshProUGUI text;
	private SpriteRenderer spriteRenderer;
	public GridTransform gt;
	public int value;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		gm = GameManager.instance;
		text = GetComponentInChildren<TextMeshProUGUI>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		gt = GetComponent<GridTransform>();
	}

	public void SetValue(int newValue)
	{
		value = newValue;
		spriteRenderer.sprite = gm.blockTextures[newValue];
		text.text = gm.blockLabels[newValue];
	}

	public Vector2 GetCoords()
	{
		return new Vector2( gt.x, gt.y );
	}
}
