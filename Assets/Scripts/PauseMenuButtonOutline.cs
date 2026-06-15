using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuButtonOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    private static readonly Color HoverColor = Color.red;
    private static readonly Color SelectedColor = Color.white;
    private static readonly Vector2 OutlineDistance = new Vector2(4f, -4f);

    private Outline outline;
    private bool isHovered;
    private bool isSelected;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline == null)
            outline = gameObject.AddComponent<Outline>();

        outline.effectDistance = OutlineDistance;
        outline.useGraphicAlpha = false;
        RefreshVisual();
    }

    private void OnDisable()
    {
        isHovered = false;
        isSelected = false;
        RefreshVisual();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        RefreshVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        RefreshVisual();
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        RefreshVisual();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        RefreshVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current?.SetSelectedGameObject(gameObject);
    }

    private void RefreshVisual()
    {
        if (outline == null)
            return;

        if (isSelected)
        {
            outline.enabled = true;
            outline.effectColor = SelectedColor;
            return;
        }

        if (isHovered)
        {
            outline.enabled = true;
            outline.effectColor = HoverColor;
            return;
        }

        outline.enabled = false;
    }
}
