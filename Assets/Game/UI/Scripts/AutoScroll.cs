using UnityEngine;
using System.Collections;

public class AutoScroll: MonoBehaviour
{
    public enum ScrollDirection
    {
        Up = 0, 
        Down,
        Left,
        Right
    }

    public ScrollDirection direction = ScrollDirection.Up;
    public float speed = 1;
    public float delay = 1;
    public bool loop = false;

    private Vector2 startPosition;
    private RectTransform rectTransform;
    private RectTransform viewport;

    private float elapsed = 0;

    public void Restart()
    {
        elapsed = 0;
        ResetPosition();
    }

    private void ResetPosition()
    {
        rectTransform.position = startPosition;
    }

    #region MonoBehaviour

    void Awake()
    {
        rectTransform = (RectTransform)transform;
        viewport = GetComponentInParent<RectTransform>();
    }

    void Start()
    {
        startPosition = rectTransform.position;
    }

	// Update is called once per frame
	void Update()
    {
        if (elapsed < delay)
        {
            elapsed += Time.unscaledDeltaTime;
        }
        else 
        {
            Vector2 velocity;
            switch (direction)
            {
                case ScrollDirection.Up:
                    if (rectTransform.anchoredPosition.y > 0)
                        goto Skip;
                    velocity = Vector2.up;
                    break;
                case ScrollDirection.Down:
                    if (rectTransform.anchoredPosition.y < 0)
                        goto Skip;
                    velocity = Vector2.down;
                    break;
                case ScrollDirection.Left:
                    if (rectTransform.anchoredPosition.x < 0)
                        goto Skip;
                    velocity = Vector2.left;
                    break;
                case ScrollDirection.Right:
                    if (rectTransform.anchoredPosition.x > 0)
                        goto Skip;
                    velocity = Vector2.right;
                    break;
                default:
                    velocity = Vector2.zero;
                    break;
            }

            rectTransform.anchoredPosition += velocity * speed * Time.unscaledDeltaTime;
        }

        return;

        Skip:

        if (loop)
            ResetPosition();
	}

    #endregion
}
