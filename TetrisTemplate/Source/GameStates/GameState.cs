using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameState
{
	public virtual void Update(float delta) { }
	public virtual void Draw(SpriteBatch batch) { }
}