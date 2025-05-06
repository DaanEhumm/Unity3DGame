using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leaderboardTextField;

    private void Start()
    {
        LeaderboardData data = LeaderboardStorage.LoadLeaderboard();
        leaderboardTextField.text = "Leaderboard\n";

        int rank = 1;
        foreach (PlayerData player in data.players)
        {
            leaderboardTextField.text += $"{rank}. {player.username} - Score: {player.score}, Time: {player.time:F2}\n";
            rank++;
        }
    }
}
