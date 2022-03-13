using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))] 
public class PauseMenuToggle : MonoBehaviour
{
    // Start is called before the first frame update

    private CanvasGroup  canvasGroup;
    void Awake()
    {
        try{
            canvasGroup = GetComponent<CanvasGroup>();
        }catch{
            Debug.LogError("Unable to find CanvasGroup object");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (canvasGroup.interactable) {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
            Time.timeScale = 1f;
            } else {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            Time.timeScale = 0f;
            }
        }
    }
}