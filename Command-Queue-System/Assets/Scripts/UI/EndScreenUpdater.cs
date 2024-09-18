using TMPro;
using UnityEngine;

public class EndScreenUpdater : MonoBehaviour
{

    public delegate void ShowEndScreenDelegate(bool canContinue, string endText);
    public static ShowEndScreenDelegate show;

    private CanvasGroup canvasGroup;
    private GameObject continueButton;
    private void Start()
    {
        show += ShowEndScreen;

        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        continueButton = transform.GetChild(1).gameObject;
        continueButton.SetActive(false);
    }

    public void ShowEndScreen(bool canContinue, string endText)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (canContinue)
        {
            continueButton.SetActive(true);
        }
        gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = endText;
    }
}
