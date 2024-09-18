using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static void MoveToScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
