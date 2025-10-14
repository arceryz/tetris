using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TetrisBlock
{
    public static Texture2D BlockTex;
	public static Texture2D BlockOutlineTex;
	public static void LoadContent()
    {
        BlockTex = TetrisGame.Load<Texture2D>(Assets.Textures.Block);
		BlockOutlineTex = TetrisGame.Load<Texture2D>(Assets.Textures.BlockOutline);
	}

    public static TetrisBlock[] Blocks =
    {
        new TetrisBlock(Color.Blue, new Vector2I(1, 1), true, new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 1 },
        }),

        new TetrisBlock(Color.Orange, new Vector2I(1, 1), true, new int[,]
        {
            { 0, 0, 1 },
            { 1, 1, 1 },
        }),

        new TetrisBlock(Color.Magenta, new Vector2I(1, 1), true, new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 },
        }),

        new TetrisBlock(Color.Lime, new Vector2I(1, 1), true, new int[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 },
        }),

        new TetrisBlock(Color.Red, new Vector2I(1, 1), true, new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 },

        }),

        new TetrisBlock(Color.Cyan, new Vector2I(2, 0), true, new int[,]
        {
            { 1, 1, 1, 1 },
        }),

        new TetrisBlock(Color.Yellow, new Vector2I(0, 0), false, new int[,]
        {
            { 1, 1 },
            { 1, 1 },
        }),
    };
    public static TetrisBlock GetRandom()
    {
        return Blocks[TetrisGame.Random.Next() % Blocks.Length].Duplicate();
    }

    public int[,] Grid { get; private set; }
    public Color Color;
    public Vector2I Origin { get; private set; }
    bool canRotate;

    TetrisBlock(Color color, Vector2I origin, bool canRotate, int[,] grid) 
    {
        this.Grid = grid.Clone() as int[,];
        this.Color = color;
        this.Origin = origin;
        this.canRotate = canRotate;
    }

    public TetrisBlock Duplicate()
    {
        return new TetrisBlock(Color, Origin, canRotate, Grid);
    }

    public void Draw(SpriteBatch batch, Vector2 position, bool outline=false)
    {
        for (int y = 0; y < Grid.GetLength(0); y++)
        {
            for (int x = 0; x < Grid.GetLength(1); x++)
            {
                if (Grid[y, x] == 1)
                {
                    Vector2 pos = position + new Vector2((x-Origin.X) * BlockTex.Width, (y-Origin.Y) * BlockTex.Height);
                    batch.Draw(outline ? BlockOutlineTex: BlockTex, pos, Color);
                }
            }
        }
    }

    /// <summary>
    /// Rotate the block clockwise or counter-clockwise (default).
    /// </summary>
    /// <param name="left"></param>
    public void Rotate(bool left=true)
    {
        // Certain blocks should not be rotateable (like the square).
        if (!canRotate)
            return;

        // The rotated grid has flipped width and height.
        // Origin is rotated as well.
        int[,] rotatedGrid = new int[Grid.GetLength(1), Grid.GetLength(0)];
        Origin = GetRotatedPoint(Origin, left);

		for (int y = 0; y < Grid.GetLength(0); y++)
        {
            for (int x = 0; x < Grid.GetLength(1); x++)
            {
                Vector2I newPoint = GetRotatedPoint(new Vector2I(x, y), left);
                rotatedGrid[newPoint.Y, newPoint.X] = Grid[y, x];
			}
		}
		Grid = rotatedGrid;
    }

    /// <summary>
    /// Return a rotated point in the grid.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="left"></param>
    /// <returns></returns>
    public Vector2I GetRotatedPoint(Vector2I point, bool left=true)
    {
        Vector2I newPoint = Vector2I.Zero;

		// For the rotation we are essentially chaining two array operations:
		// Naturally the two are reverse operations of each other.
		//
		// Left : Flip Rows -> Transpose
		// Right: Transpose -> Flip Rows
		//
		// When translating this to an operation for every (x,y), we get
		// the below two cases for left/right rotation.
		if (left)
		{
			newPoint.X = point.Y;
			newPoint.Y = Grid.GetLength(1) - 1 - point.X;
		}
		else
		{
			newPoint.X = Grid.GetLength(0) - 1 - point.Y;
			newPoint.Y = point.X;
		}

        return newPoint;
	}
}
