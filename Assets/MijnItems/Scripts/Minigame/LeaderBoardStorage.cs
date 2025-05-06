using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string username;
    public int score;
    public float time;
}

[System.Serializable]
public class LeaderboardData
{
    public List<PlayerData> players = new List<PlayerData>();
}

public static class LeaderboardStorage
{
    private static string FilePath => Path.Combine(Application.persistentDataPath, "leaderboard.json");

    public static void SaveScore(PlayerData player)
    {
        LeaderboardData data = LoadLeaderboard();
        data.players.Add(player);

        data.players.Sort((a, b) =>
        {
            int scoreCompare = b.score.CompareTo(a.score);
            return scoreCompare == 0 ? a.time.CompareTo(b.time) : scoreCompare;
        });

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
    }

    public static LeaderboardData LoadLeaderboard()
    {
        if (!File.Exists(FilePath))
            return new LeaderboardData();

        string json = File.ReadAllText(FilePath);
        return JsonUtility.FromJson<LeaderboardData>(json);
    }
}