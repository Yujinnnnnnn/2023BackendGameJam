using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler
{
    [System.Serializable]
    public class MoveEvent : UnityEngine.Events.UnityEvent<Vector2> { }
    public MoveEvent onMoveEvent = new MoveEvent();

    public RectTransform joy;
    public RectTransform stick;
    public CanvasGroup group;

    public Vector2 initPos, nowPos;
    public Vector2 pos;

    public float movementRange = 100f;

    private void Update()
    {
        SetTouchPosition();
    }

    public void OnPointerDown(PointerEventData data)
    {
        //Debug.Log(Input.touchCount);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            initPos = touch.position;
        }
    }

    void SetTouchPosition()
    {
        
        if (Input.touchCount == 0)
        {
            if (group.alpha == 0) return;
            group.alpha = 0;
            initPos = Vector2.zero;
            nowPos  = Vector2.zero;
            pos = Vector2.zero;
            onMoveEvent?.Invoke(pos);
            return;
        } 
        else
        {
            if (initPos == Vector2.zero) return;
        }

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                group.alpha = 1;
                joy.position = initPos;
                break;
            case TouchPhase.Moved:
                nowPos = touch.position;
                pos = (nowPos - initPos).normalized * Mathf.Min(Vector2.Distance(nowPos, initPos), movementRange);
                stick.position = initPos + pos;

                onMoveEvent?.Invoke(pos);
                break;
        }
    }
}
