using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using System.Diagnostics;

/// <summary>
/// Contrary to the normal TetrisBlock class, this class manages the
/// actual falling block in the tetris grid, its animations and its
/// drawing in the grid. Also maps input controls from players.
/// </summary>
public class FallingTetrisBlock
{
	public Vector2I gridPos;
	int playerIndex;
	TetrisBlock block;
	float fallTimer = 0.0f;
	bool isDisabled = false;

	/// <summary>
	/// Creates a new falling block for the assigned grid.
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <param name="assignedGrid"></param>
	public FallingTetrisBlock(int playerIndex, TetrisBlock block)
	{
		this.playerIndex = playerIndex;
		this.block = block;
		gridPos = new Vector2I(5, 2);

		if (PlayingState.TetrisGrid.IsBlockColliding(block, gridPos))
		{
			isDisabled = true;
			PlayingState.GameOver();
		}
	}

	public void Update(float delta)
	{
		if (isDisabled) 
			return;

		if (TetrisGame.Input.KeyPressed(Keys.Z))
			TryRotate();
		if (TetrisGame.Input.KeyPressed(Keys.Up))
			TryRotate(false);

		if (TetrisGame.Input.KeyPressed(Keys.Left))
			TryMoveX(-1);
		if (TetrisGame.Input.KeyPressed(Keys.Right))
			TryMoveX(1);

		// Apply falling behavior.
		float fallInterval = TetrisGame.Input.KeyDown(Keys.Down) ?
			PlayingState.BlockFallIntervalFast : PlayingState.BlockFallInterval;
		if (fallTimer > fallInterval)
		{
			fallTimer = 0;
			TryMoveY(1);
		}
		fallTimer += delta;
	}

	public void Draw(SpriteBatch batch)
	{
		if (isDisabled) 
			return;

		// Visualise the current landing position by
		// iterating steps downward until there is a collision.
		Vector2I landingPos = gridPos;
		for (int dy = 0; dy < PlayingState.TetrisGrid.Height; dy++)
		{
			Vector2I testPos = landingPos + new Vector2I(0, 1);
			if (PlayingState.TetrisGrid.IsBlockColliding(block, testPos))
				break;
			landingPos = testPos;
		}

		block.Draw(batch, PlayingState.TetrisGrid.GridToScreenPos(landingPos), true);
		block.Draw(batch, PlayingState.TetrisGrid.GridToScreenPos(gridPos));
	}

	/// <summary>
	/// Try rotating the block if no collision.
	/// </summary>
	/// <param name="left"></param>
	public void TryRotate(bool left=true)
	{
		TetrisBlock newBlock = block.Duplicate();
		newBlock.Rotate(left);
		if (!PlayingState.TetrisGrid.IsBlockColliding(newBlock, gridPos))
			block = newBlock;
	}

	/// <summary>
	/// Try moving horizontally if no collision.
	/// </summary>
	/// <param name="dx"></param>
	public void TryMoveX(int dx)
	{
		Debug.WriteLine($"Trying to move dx={dx}");
		// First try moving horizontally.
		Vector2I pos = gridPos + new Vector2I(dx, 0);
		if (!PlayingState.TetrisGrid.IsBlockColliding(block, pos))
			gridPos = pos;
	}

	/// <summary>
	/// Try moving vertically, merge if collision.
	/// </summary>
	/// <param name="dy"></param>
	public void TryMoveY(int dy)
	{
		Debug.WriteLine($"Trying to move dy={dy}");
		Vector2I pos = gridPos + new Vector2I(0, dy);
		if (PlayingState.TetrisGrid.IsBlockColliding(block, pos))
		{
			PlayingState.TetrisGrid.MergeBlock(block, gridPos);
			isDisabled = true;
			PlayingState.CreateFallingBlock(playerIndex);
		}
		else
		{
			gridPos = pos;
		}
	}
}
