using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
public class TetrisGrid
{
    public Color EmptyColor { get { return Color.White; } }
    public Vector2 Position;
    public int Width { get { return 10; } }
    public int Height { get { return 20; } }

    List<Color[]> grid = new();
    Texture2D emptyBlockTex;
    SoundEffect rowClearSFX;

	public TetrisGrid()
    {
        rowClearSFX = TetrisGame.Load<SoundEffect>(Assets.Sounds.RowClear);
        emptyBlockTex = TetrisGame.Load<Texture2D>(Assets.Textures.EmptyBlock);
		Position = Vector2.Zero;
		for (int y = 0; y < Height; y++)
        {
            Color[] row = new Color[Width];
            grid.Add(row);
        }
		Clear();
	}
    
    /// <summary>
    /// Reset grid to empty state.
    /// </summary>
    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                grid[y][x] = EmptyColor;
            }
        }
    }
   
    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="batch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(SpriteBatch batch)
    {
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y) 
            {
                Vector2 pos = Position + new Vector2(x * emptyBlockTex.Width, y * emptyBlockTex.Height);
                Color col = grid[y][x];
                batch.Draw(col == EmptyColor ? emptyBlockTex: TetrisBlock.BlockTex, pos, col);
            }
        }
    }

    /// <summary>
    /// Returns whether the given tetris block is colliding with the grid
    /// at the given integer grid position. Returns an additional direction of
    /// collision information (1=right, -1=left) that can be used to offset the block.
    /// </summary>
    /// <param name="block"></param>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public (bool, int) IsBlockCollidingSide(TetrisBlock block, Vector2I gridPos)
    {
        for (int y = 0; y < block.Grid.GetLength(0); y++)
        {
            for (int x = 0; x < block.Grid.GetLength(1); x++)
            {
                if (block.Grid[y, x] == 1)
                {
                    Vector2I pos = gridPos + new Vector2I(x, y) - block.Origin;
                    if (GetColorAt(pos) != EmptyColor)
                        return (true, x > block.Origin.X ? 1: -1);
                }
            }
        }
        return (false, 0);
    }

    /// <summary>
    /// Returns whether block is colliding.
    /// </summary>
    /// <param name="block"></param>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public bool IsBlockColliding(TetrisBlock block, Vector2I gridPos)
    {
       (bool coll, int side) = IsBlockCollidingSide(block, gridPos);
        return coll;
    }

	/// <summary>
	/// Returns the color at the given position, solid (black) if out of bounds.
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public Color GetColorAt(Vector2I pos)
    {
        if (pos.X >= 0 && pos.Y >= 0 && pos.X < Width && pos.Y < Height)
            return grid[pos.Y][pos.X];
        return Color.Black;
    }

	/// <summary>
	/// Set color at position and ignore if out of bounds.
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public void SetColorAt(Vector2I pos, Color col)
	{
        if (pos.X >= 0 && pos.Y >= 0 && pos.X < Width && pos.Y < Height)
            grid[pos.Y][pos.X] = col;
	}

	/// <summary>
	/// Merges the block at specified position with the tetris grid.
	/// </summary>
	/// <param name="block"></param>
	/// <param name="gridPos"></param>
	public void MergeBlock(TetrisBlock block, Vector2I gridPos)
    {
		for (int y = 0; y < block.Grid.GetLength(0); y++)
		{
			for (int x = 0; x < block.Grid.GetLength(1); x++)
			{
                if (block.Grid[y, x] == 1)
                {
                    Vector2I pos = gridPos + new Vector2I(x, y) - block.Origin;
                    SetColorAt(pos, block.Color);
                }
			}
		}
        PopFullRows();
	}

    /// <summary>
    /// Iterates the grid and removes any full rows, shifting down
    /// all the blocks that lie above it.
    /// </summary>
    public void PopFullRows()
    {
        int popCount = 0;
        for (int y = 0; y < Height; y++)
        {
            bool isFull = true;
            for (int x = 0; x < Width; x++)
            {
                if (GetColorAt(new Vector2I(x, y)) == EmptyColor)
                {
                    isFull = false;
                    break;
                }
            }

            if (isFull)
            {
                grid.RemoveAt(y);
                Color[] newRow = new Color[Width];
                for (int i = 0; i < Width; i++) newRow[i] = EmptyColor;
                grid.Insert(0, newRow);
                popCount += 1;
            }
        }

        if (popCount > 0)
        {
            PlayingState.AddScore(100 * popCount * popCount);
            TetrisGame.AddCameraShake(popCount * 100);
            rowClearSFX.Play(0.5f, 0, 0);
        }
    }

    /// <summary>
    /// Returns the screen position in pixels of the given grid coordinate.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 GridToScreenPos(Vector2I pos)
    {
        return Position + new Vector2(pos.X * TetrisBlock.BlockTex.Width, pos.Y * TetrisBlock.BlockTex.Height);
    }
}

