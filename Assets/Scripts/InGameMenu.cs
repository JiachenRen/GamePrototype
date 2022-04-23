using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CanvasGroup))]
public class InGameMenu : MonoBehaviour
{
    // Start is called before the first frame update

    private CanvasGroup canvasGroup;
    public GameObject hub;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Toggle()
    {
        if (canvasGroup.interactable)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void Show()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        hub.SetActive(false);
    }

    public void Hide()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        hub.SetActive(true);
    }
}