using UnityEngine;

public class SafeArea : MonoBehaviour
{
    void Start()
    {
        Rect safeArea = Screen.safeArea;

        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = safeArea.position + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;


        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }
}
