using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Blockf
{
	/// <summary>
	/// Check if any of the blocks in array piece collides with any blocks in the list blocksOnBoard.
	/// </summary>
	/// <param name="piece"></param>
	/// <param name="blocksOnBoard"></param>
	/// <param name="squareSize"></param>
	/// <returns></returns>
	public static bool DoBlocksCollide( BlockStruct[] piece, List<BlockStruct> blocksOnBoard, int squareSize = -1 ) {

		// Check if piece is outside of board
		if (squareSize > -1)
		{
			if (piece.Where(block => block.x < 0 || block.x >= squareSize || block.y < 0).ToArray().Length > 0)
			{
				return true;
			}
		}

		// Check if piece collides with another block
		if (piece != null && blocksOnBoard != null && piece.Where(pBlock => blocksOnBoard.Where(bBlock => pBlock.x == bBlock.x && pBlock.y == bBlock.y).ToArray().Length > 0).ToArray().Length > 0) {
			return true;
		}

		return false;
	}

	public static Vector2 GetSectionSize(int boardSize)
	{
		int sectionHeight = (int)Math.Floor(Math.Sqrt(boardSize));
		while (boardSize % sectionHeight > 0)
		{
			sectionHeight--;
		}
		int sectionWidth = boardSize / sectionHeight;

		return new Vector2(sectionWidth, sectionHeight);
	}

	public static float GetSpeed(float baseSpeed, int blockCount, int squareSize )
	{
		float level = ( blockCount * 3f / Mathf.Pow(squareSize, 2f) ) + 1f;
		float adjustedFallSpeed = baseSpeed * Mathf.Pow( 0.8f - ( (level - 1) * 0.007f ), level - 1);
		return adjustedFallSpeed;	
	}

	public static BlockStruct[] MoveBlocks(BlockStruct[] blocks, int adjustX, int adjustY)
	{
		return blocks.Select(block =>
		{
			block.x += adjustX;
			block.y += adjustY;
			return block;
		}).ToArray();
	}


}