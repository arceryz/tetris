
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class PlayingState: GameState
{
	GameWorld gameWorld;

	public PlayingState()
	{
		gameWorld = new GameWorld();
		gameWorld.Reset();
	}

	public override void Update(float delta)
	{
		gameWorld.Update(delta);
	}

	public override void Draw(SpriteBatch batch)
	{
		gameWorld.Draw(batch);
	}
}
