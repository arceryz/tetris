using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

public class PlayingState: GameState
{
	// Game settings.
	public static float NumPreviewBlocks = 3;
	public static float BlockFallIntervalFast = 0.05f;

	public static int[] LevelScores = { 200, 700, 1500, -1 };
	public static float[] BlockFallIntervals = { 0.75f, 0.5f, 0.4f, 0.35f };
	public static bool MultiplayerMode = false;

	public static TetrisGrid TetrisGrid;
	public static FallingTetrisBlock Player1;
	public static FallingTetrisBlock Player2;
	public static int Score { get; private set; } = 0;
	public static int SpeedLevel = 0;

	static int nextLevelScore;
	static float scoreFlashTimer = 0;
	static string scoreFlashText = "";
	static List<TetrisBlock> previewBlocks;

	ImageFont arcadeFont;

    public PlayingState()
	{
		TetrisGrid = new TetrisGrid();
		TetrisGrid.Position = TetrisGame.GetAnchor(0, -1, -5 * 30, 0);
		Score = 0;
		SpeedLevel = 0;
		nextLevelScore = LevelScores[0];
		arcadeFont = TetrisGame.Load<ImageFont>(Assets.Fonts.Arcade);

		previewBlocks = new();
		for (int i = 0; i < NumPreviewBlocks; i++)
		{
			previewBlocks.Add(TetrisBlock.GetRandom());
		}

		CreateFallingBlock(0);
		if (MultiplayerMode)
			CreateFallingBlock(1);
    }

	public override void Update(float delta)
	{
		Player1.Update(delta);
		if (MultiplayerMode)
			Player2.Update(delta);
		if (scoreFlashTimer > 0) scoreFlashTimer -= delta;
	}

	public override void Draw(SpriteBatch batch)
	{
        TetrisGrid.Draw(batch);
		Player1.Draw(batch);
		if (MultiplayerMode)
			Player2.Draw(batch);

		// Draw upcoming blocks.
		for (int i = 0; i < NumPreviewBlocks; i++)
		{
			TetrisBlock block = previewBlocks[i];
			Vector2 pos = TetrisGame.GetAnchor(0.65f, -0.5f + 0.5f * i);
			block.Draw(batch, pos);
		}

		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, -1, 20, 20), $"{Score:D6}", Color.White, 4, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(1, -1, -190, 20), $"level {SpeedLevel+1}", Color.White, 3, false);
		if (nextLevelScore > 0)
			arcadeFont.DrawString(batch, TetrisGame.GetAnchor(1, -1, -200, 60), $"next at {nextLevelScore}", Color.Gray, 2, false);

		if (scoreFlashTimer > 0 && scoreFlashTimer % 0.2f < 0.1f)
		{
			arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, -3, -3), scoreFlashText, Color.Orange, 6, true);
			arcadeFont.DrawString(batch, TetrisGame.GetAnchor(0, 0, 0, 0), scoreFlashText, Color.White, 6, true);
		}

		Color controlColor = new Color(50, 50, 50);
		if (MultiplayerMode)
			arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 10, -130), "player 2: wasd", controlColor, 2, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -100), "arrows: move", controlColor, 2, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -80), "down: faster", controlColor, 2, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -50), "z : rotate l", controlColor, 2, false);
		arcadeFont.DrawString(batch, TetrisGame.GetAnchor(-1, 1, 20, -30), "up: rotate r", controlColor, 2, false);
	}

	public static void GameOver()
	{
		TetrisGame.ChangeState(new EndState());
	}

	public static void CreateFallingBlock(int playerIndex)
	{
		if (playerIndex == 0)
			Player1 = new FallingTetrisBlock(0, GetNextBlock());
		if (playerIndex == 1)
			Player2 = new FallingTetrisBlock(1, GetNextBlock());
	}

	public static void AddScore(int score)
	{
		if (score <= 0) return;

		string[] goodWords = { "good", "nice", "got em", "well done", "try harder", "ok", "not bad", "cool" };
		string[] betterWords = { "sublime", "crazy", "master", "pro", "damn", "wow", "impressive", "genius" };
		string[] ultraWords = { "insanity", "godlike", "perfect", "omg", "lucky", "god", "ultra" };

		string word = goodWords[TetrisGame.Random.Next(goodWords.Length)];
		if (score > 200) word = betterWords[TetrisGame.Random.Next(betterWords.Length)];
		if (score > 600) word = ultraWords[TetrisGame.Random.Next(ultraWords.Length)];

		scoreFlashText = $"{word} {score}";
		scoreFlashTimer = 1;
		Score += score;

		if (Score >= nextLevelScore && SpeedLevel < BlockFallIntervals.Length - 1)
		{
			SpeedLevel += 1;
			scoreFlashText = $"level {SpeedLevel+1}";
			nextLevelScore = LevelScores[SpeedLevel];
		}
	}

	static TetrisBlock GetNextBlock()
	{
		TetrisBlock next = previewBlocks[0];
		previewBlocks.RemoveAt(0);
		previewBlocks.Add(TetrisBlock.GetRandom());
		return next;
	}
}
