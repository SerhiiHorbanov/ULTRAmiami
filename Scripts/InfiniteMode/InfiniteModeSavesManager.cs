using System;
using System.IO;
using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;

namespace ULTRAmiami.InfiniteMode;

public partial class InfiniteModeSavesManager : Node
{
	[Export] private string _bestTimeSavesDirectory = "user://InfiniteMode/BestTime/";
	[Export] private string _maxKillsSavesDirectory = "user://InfiniteMode/MaxKills/";
	[Export] private string _maxBloodConsumedDirectory =  "user://InfiniteMode/MaxBloodConsumed/";
	[Export] private string _maxBloodLostDirectory = "user://InfiniteMode/MaxBloodLost/";
	
	private const int SavesForEachStatAmount = 3;

	private InfModeScore[] _bestTimeScores;
	private InfModeScore[] _maxKillsScores;
	private InfModeScore[] _maxBloodConsumedScores;
	private InfModeScore[] _maxBloodLostScores;

	public override void _Ready()
	{
		LoadScores();
	}

	public void UnloadScores()
	{
		_bestTimeScores = null;
		_maxKillsScores = null;
		_maxBloodConsumedScores = null;
		_maxBloodLostScores = null;
	}
	
	public void LoadScores()
	{
		_bestTimeScores = LoadScoresAndSort(_bestTimeSavesDirectory, InfModeScore.CompareByTime);
		_maxKillsScores = LoadScoresAndSort(_maxKillsSavesDirectory, InfModeScore.CompareByKills);
		_maxBloodConsumedScores = LoadScoresAndSort(_maxBloodConsumedDirectory, InfModeScore.CompareByBloodConsumed);
		_maxBloodLostScores = LoadScoresAndSort(_maxBloodLostDirectory, InfModeScore.CompareByBloodLost);
	}

	private InfModeScore[] LoadScoresAndSort(string dir, Comparison<InfModeScore> comparison)
	{
		InfModeScore[] scores = LoadPlayerScoresFromDir(dir);
		Array.Sort(scores, comparison);
		Array.Reverse(scores);
		return scores;
	}
	
	private InfModeScore[] LoadPlayerScoresFromDir(string dir)
	{
		Directory.CreateDirectory(ProjectSettings.GlobalizePath(dir)); 
		DirAccess bestTimeSavesDir = DirAccess.Open(dir);
		
		string[] fileNames = bestTimeSavesDir.GetFiles();
		InfModeScore[] scores = new InfModeScore[fileNames.Length];

		for (int i = 0; i < fileNames.Length; i++)
		{
			InfModeScore score = ResourceLoader.Load<InfModeScore>(dir + fileNames[i]);
			scores[i] = score;
		}
		
		return scores;
	}
	
	private void SaveScore(PlayerScore score)
	{
		InfModeScore infModeScore = new(score);
		
		if (_bestTimeScores.Length < SavesForEachStatAmount || infModeScore.Time > _bestTimeScores[^1].Time)
		{
			int place = PlaceScoreInBestTimeScores(infModeScore);
			SaveScores(_bestTimeScores, _bestTimeSavesDirectory);
		}
		
		if (_bestTimeScores.Length < SavesForEachStatAmount || infModeScore.Kills > _bestTimeScores[^1].Kills)
		{
			int place = PlaceScoreInMaxKillsScores(infModeScore);
			SaveScores(_maxKillsScores, _maxKillsSavesDirectory);
		}
	}

	private void SaveScores(InfModeScore[] scores, string dir)
	{
		DirAccess dirAcces = DirAccess.Open(dir);
		
		foreach (string file in dirAcces.GetFiles())
			dirAcces.Remove(file);

		foreach (InfModeScore score in scores)
		{
			if (score is null)
				continue;
			
			string path = dir + score.Time.ToString("0.000") + ".tres";
			var huh = ResourceSaver.Save(score, path);
			GD.Print(huh);
		}
	}

	private int PlaceScoreInBestTimeScores(InfModeScore score)
		=> InsertScore(score, ref _bestTimeScores, InfModeScore.CompareByTime);
	private int PlaceScoreInMaxKillsScores(InfModeScore score)
		=> InsertScore(score, ref _maxKillsScores, InfModeScore.CompareByKills);

	private int InsertScore(InfModeScore score, ref InfModeScore[] scores, Comparison<InfModeScore> comparison)
	{
		int place = Array.FindIndex(scores, a => comparison(a, score) != 1);

		if (place == -1 || scores.Length < SavesForEachStatAmount)
		{
			place = scores.Length;
			Array.Resize(ref scores, scores.Length + 1);
			scores[^1] = score;
			return place;
		}
		
		// shifting needed elements to the right
		Array.Copy(scores, place, scores, place + 1, scores.Length - place - 1);
		
		scores[place] = score;
		return place;
	}

	private void SaveCurrentScore(Hit _)
	{
		SaveScore(PlayerScore.Current);
	}
}