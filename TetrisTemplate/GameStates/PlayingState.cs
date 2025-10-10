
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class PlayingState: GameState
{
	TetrisGrid grid;
	TetrisBlock block;
    public PlayingState()
	{
		block = new TetrisBlock(TetrisBlockData.GetRandom());
		block.Position = new Vector2(250, 100);
        grid = new TetrisGrid();
        grid.Position = new Vector2(250, 0);
    }

	public override void Update(float delta)
	{
	}

	public override void Draw(SpriteBatch batch)
	{
		grid.Draw(batch);
		block.Draw(batch);
    }
}
