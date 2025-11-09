using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Functions that has to do with blocks on a sudoku board.
/// </summary>
public static class Blockf
{

	// ===== D =====

	/// <summary>
	/// Checks if an array of BlockStruct values contains elements that collide with an existing list of Blockstruct values.
	/// </summary>
	/// <param name="piece">The blocks in the current game piece. Normally Blockf.MoveBlocks() is used before this method runs.</param>
	/// <param name="blocksOnBoard">The blocks already on the board.</param>
	/// <param name="squareSize">The size of the board. If given, the method will return true if part of the piece falls outside the board. If not, no such check will be done.</param>
	/// <returns>True if the blocks collide with the existing blocks (or goes outside the board), false if it doesn't.</returns>
	public static bool DoBlocksCollide(BlockStruct[] piece, List<BlockStruct> blocksOnBoard, int squareSize = -1)
	{

		// Check if piece is outside of board
		if (squareSize > -1)
		{
			if (piece.Where(block => block.x < 0 || block.x >= squareSize || block.y < 0).ToArray().Length > 0)
			{
				return true;
			}
		}

		// Check if piece collides with another block
		if (piece != null && blocksOnBoard != null && piece.Where(pBlock => blocksOnBoard.Where(bBlock => pBlock.x == bBlock.x && pBlock.y == bBlock.y).ToArray().Length > 0).ToArray().Length > 0)
		{
			return true;
		}

		return false;
	}

	// ===== G =====

	/// <summary>
	/// Given the size of the board, calculates what the dimensions of a section should be.
	/// </summary>
	/// <param name="boardSize">The size of the board.</param>
	/// <returns>A Vector2 value containing the x width and y height of each section of the board.</returns>
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

	/// <summary>
	/// Adjusts the fall speed of the game piece.
	/// </summary>
	/// <param name="baseSpeed">The speed set in the Game Manager.</param>
	/// <param name="blockCount">How many blocks are currently on the board?</param>
	/// <param name="squareSize">The size of the board.</param>
	/// <returns>The adjusted fall speed in seconds (if this returns 0.5, it will take the piece 0.5 seconds to fall 1 unit).</returns>
	public static float GetSpeed(float baseSpeed, int blockCount, int squareSize)
	{
		float level = (blockCount * 3f / Mathf.Pow(squareSize, 2f)) + 1f;
		float adjustedFallSpeed = baseSpeed * Mathf.Pow(0.8f - ((level - 1) * 0.007f), level - 1);
		return adjustedFallSpeed;
	}

	// ===== M =====

	/// <summary>
	/// Changes the x/y coordinates of the game piece.
	/// Note that this will not move the actual GameObject, it will only set the x and y values in the BlockStruct so that it can be used in calculations and comparisons. To actually move the GameObject, feed this return value into PositionBlocks().
	/// </summary>
	/// <param name="blocks">The current game piece.</param>
	/// <param name="adjustX">How many units to move the block horizontally.</param>
	/// <param name="adjustY">How many units to move the block vertically.</param>
	/// <returns>A BlockStruct array with new x/y values.</returns>
	public static BlockStruct[] MoveBlocks(BlockStruct[] blocks, int adjustX, int adjustY)
	{
		return blocks.Select(block =>
		{
			block.x += adjustX;
			block.y += adjustY;
			return block;
		}).ToArray();
	}

	// ===== P =====

	/// <summary>
	/// Sets the x and y values of the block GameObject in a BlockStruct to the x and y values specified in the BlockStruct.
	/// Normally, before moving or rotating a block, you would first generate a new blockstruct and then check that it is allowed to move to the specified position, and if it is, you'd run PositionBlocks() after that.
	/// </summary>
	/// <param name="blocks">The blocks to reposition.</param>
	/// <param name="instant">If set to true, the blocks will be moved instantaneously instead of slowly morphing there..</param>
	public static void PositionBlocks(BlockStruct[] blocks, bool instant = false)
	{
		foreach (BlockStruct block in blocks)
		{
			block.block.SetPosition(block.x, block.y, instant);
		}
	}

	// ===== R =====

	/// <summary>
	/// Get the position of a BlockStruct array if all pieces rotate around the piece's origin. 
	/// </summary>
	/// <param name="piece">The game piece to rotate.</param>
	/// <param name="rotations">rotations = 0: don't rotate
	/// rotations = 1: 90° clockwise
	/// rotations = 2: 180° clockwise
	/// rotations = 3: 90° counterclockwise</param>
	/// <returns>The rotated piece.</returns>
	public static BlockStruct[] RotatePiece(BlockStruct[] piece, int rotations = 1)
	{
		rotations = (rotations + 4) % 4;
		if (rotations == 0)
		{
			return piece;
		}
		BlockStruct[] newPiece = new BlockStruct[piece.Length];
		Array.Copy(piece, newPiece, piece.Length);

		if (rotations >= 2)
		{
			// Rotate 180 degrees
			newPiece = newPiece.Select(el => new BlockStruct(0 - el.x, 0 - el.y, el.value, el.block)).ToArray();
		}
		if (rotations % 2 == 1)
		{
			// Rotate 90 degrees
			newPiece = newPiece.Select(el => new BlockStruct(-el.y, el.x, el.value, el.block)).ToArray();
		}
		return newPiece;
	}
}