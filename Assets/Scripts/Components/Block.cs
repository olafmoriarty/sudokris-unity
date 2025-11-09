using System;
using TMPro;
using UnityEngine;

[Serializable]
public struct BlockStruct
{
	public int x;
	public int y;
	public int value;
	public Block block;

	public BlockStruct(int newX, int newY, int newValue, Block newBlock)
	{
		x = newX;
		y = newY;
		value = newValue;
		block = newBlock;
	}

	public BlockStruct(int newX, int newY, int newValue)
	{
		x = newX;
		y = newY;
		value = newValue;
		block = null;
	}

	public BlockStruct(Block newBlock)
	{
		x = newBlock.x;
		y = newBlock.y;
		value = newBlock.value;
		block = newBlock;
	}
}

public class Block : GridTransform
{
	private GameManager gm;
	private TextMeshProUGUI text;
	private SpriteRenderer spriteRenderer;
	public int value;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		gm = GameManager.instance;
		text = GetComponentInChildren<TextMeshProUGUI>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetValue(int newValue)
	{
		value = newValue;
		spriteRenderer.sprite = gm.blockTextures[newValue];
		text.text = gm.blockLabels[newValue];
	}

}
