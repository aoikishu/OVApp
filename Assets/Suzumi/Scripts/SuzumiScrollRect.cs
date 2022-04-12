using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SuzumiScrollRect : ScrollRect, IPointerEnterHandler, IPointerExitHandler
{
    private bool swallowScroll = true;
    private bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData) => isMouseOver = true;

    public void OnPointerExit(PointerEventData eventData) => isMouseOver = false;

    private void Update()
    {
        // Detect the mouse wheel and generate a scroll. This fixes the issue where Unity will prevent our ScrollRect
        // from receiving any mouse wheel messages if the mouse is over a raycast target (such as a button).
        if (isMouseOver && IsMouseWheelRolling())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                scrollDelta = new Vector2(0, Input.GetAxis("Mouse ScrollWheel"))
            };

            swallowScroll = false;
            OnScroll(pointerData);
            swallowScroll = true;
        }
    }

    public override void OnScroll(PointerEventData data)
    {
        if (!(IsMouseWheelRolling() && swallowScroll))
        {
            // Amplify the mousewheel so that it matches the scroll sensitivity.
            if (data.scrollDelta.y < -Mathf.Epsilon) data.scrollDelta = new Vector2(0, -scrollSensitivity);
            else if (data.scrollDelta.y > Mathf.Epsilon) data.scrollDelta = new Vector2(0, scrollSensitivity);

            base.OnScroll(data);
        }
    }

    private static bool IsMouseWheelRolling()
    {
        return Input.GetAxis("Mouse ScrollWheel") != 0;
    }
}
