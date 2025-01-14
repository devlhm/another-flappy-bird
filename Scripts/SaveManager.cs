using Godot;

namespace AnotherFlappyBird.Scripts;

public partial class SaveManager : Node
{
	private const string Path = "res://savegame.data";
	public static void SaveScore(int score)
	{
		var file = FileAccess.Open(Path, FileAccess.ModeFlags.Write);
		file.StoreVar(score);
		file.Close();
	}

	public static int LoadScore()
	{
		var file = FileAccess.Open(Path, FileAccess.ModeFlags.Read);

		var score = 0;
		
		if (file.GetLength() > 0)
			score = (int) file.GetVar();
		
		file.Close();
		return score;
	}
}