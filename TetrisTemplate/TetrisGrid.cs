using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    /// The sprite of a single empty cell in the grid.
    Texture2D emptyCell;

    /// The position at which this TetrisGrid should be drawn.
   public Vector2 Position;

    /// The number of grid elements in the x-direction.
    public int Width { get { return 10; } }

    /// The number of grid elements in the y-direction.
    public int Height { get { return 20; } }

    Color[,] grid;
    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        grid = new Color[Width, Height];
        ClearGrid();
        //grid[2, 3] = Color.Red;
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        Position = Vector2.Zero;
        Clear();
    }

    public void ClearGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                grid[x, y] = Color.White;
            }
        }

    }
    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y) 
            {
                Vector2 pos = new Vector2(x * emptyCell.Width, y * emptyCell.Height)+Position;
                Color col = grid[x, y];
                spriteBatch.Draw(emptyCell, pos, col);

          
            }
        
        
        }
        
    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
    }
}

