using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

class TetrisBlockData
{
    public static TetrisBlockData[] Blocks =
    {
        new TetrisBlockData(Color.Blue, new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }),

        new TetrisBlockData(Color.Orange, new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }),

        new TetrisBlockData(Color.Purple, new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }),

        new TetrisBlockData(Color.Green, new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }),

        new TetrisBlockData(Color.Red, new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }),

        new TetrisBlockData(Color.Cyan, new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }),

        new TetrisBlockData(Color.Yellow, new int[,]
        {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }),
    };
    
    public static TetrisBlockData GetRandom()
    {
        return Blocks[TetrisGame.Random.Next() % Blocks.Length];
    }

    public int[,] Grid;
    public Color Color;
    TetrisBlockData(Color color, int[,] grid) 
    {
        this.Grid = grid;
        this.Color = color;
    }
}

class TetrisBlock
{
    Texture2D blockTex;
    TetrisBlockData data;
    public Vector2 Position = Vector2.Zero;
    
    public TetrisBlock(TetrisBlockData data)
    {
        this.data = data;
        blockTex = TetrisGame.ContentManager.Load<Texture2D>("block");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < 5; x++) 
        {
            for (int y = 0; y < 5; y++)
            {
                if (data.Grid[y,x] == 0) continue;

                Vector2 pos = new Vector2((x-2)*blockTex.Width , (y-2)*blockTex.Height) + Position;
                spriteBatch.Draw(blockTex, pos, data.Color);
            }  
        }
    }
}
