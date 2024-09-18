using UnityEngine;

public class WinObjectScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameMode.isOnWinTile = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameMode.isOnWinTile = false;
    }
}
