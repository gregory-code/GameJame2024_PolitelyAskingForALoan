using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRenderer : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private bool isActiveGroup;

    public void SetUIStatus(bool state)
    {
        isActiveGroup = state;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public bool isActive()
    {
        return isActiveGroup;
    }

    public void SetStartingActive(bool state)
    {
        isActiveGroup = state;
    }

    public void LateUpdate()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        int alpha = (isActiveGroup) ? 1 : 0;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, alpha, 18 * Time.deltaTime);
    }
}
