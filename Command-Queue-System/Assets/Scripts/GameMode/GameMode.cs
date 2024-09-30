using UnityEngine;

public class GameMode : MonoBehaviour
{

    public static bool isOnWinTile;

    public delegate void OnWinGameDelegate();
    public static OnWinGameDelegate onWinGame;
    public delegate void OnLoseGameDelegate(bool isOutOfBounds);
    public static OnLoseGameDelegate onLoseGame;

    private void Start()
    {
        onWinGame += WinGame;
        onLoseGame += LoseGame;
    }

    public void WinGame()
    {
        EndScreenUpdater.show.Invoke(true, "You Won, Congratulations!");
    }

    public void LoseGame(bool isOutOfBounds)
    {
        string loseMessage = "You lose. You didn't make it to the win tile";
        if (isOutOfBounds)
        {
            loseMessage = "You lose, You went out of bounds";
            // change text of endscreen to out of bounds
        }
        // leave text as it is if the player is not out of bounds

        EndScreenUpdater.show.Invoke(false, loseMessage);
    }

}
