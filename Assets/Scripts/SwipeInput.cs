using Manager;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    public GameManager gameManager;
    public float minSwipeDistance = 100f;

    private Vector2 _touchStart;
    private bool _swipeHandled = false;

    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            _touchStart = touch.position;
            _swipeHandled = false;
        }
        else if (touch.phase == TouchPhase.Ended && !_swipeHandled)
        {
            Vector2 swipe = touch.position - _touchStart;

            if (swipe.magnitude < minSwipeDistance) return;

            if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
            {
                // Swipe left/right
                if (swipe.x > 0)
                    gameManager.Move(Vector2Int.right);
                else
                    gameManager.Move(Vector2Int.left);
            }
            else
            {
                // Swipe up/down (inverted to match UI coordinates)
                if (swipe.y > 0)
                    gameManager.Move(Vector2Int.down);
                else
                    gameManager.Move(Vector2Int.up);
            }

            _swipeHandled = true;
        }
    }
}