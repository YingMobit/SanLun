using UnityEngine;

public class TrackingArrow : MonoBehaviour
{
    public Transform player;
    public Transform exit;
    private float screenBorderBuffer = 50f; // 箭头与屏幕边界的缓冲距离
    private RectTransform arrowRectTransform;

    private void Start()
    {
        arrowRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        PositionArrow();
        RotateArrowTowardsExit();
    }

    void PositionArrow()
    {
        Vector2 fromPlayerToExit = exit.position - player.position;
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(player.position + (Vector3)fromPlayerToExit.normalized * 1000); // 将目标位置投影到很远的地方以确保它总是在屏幕外
        viewportPosition = viewportPosition * 2 - Vector2.one; // 转换到以屏幕中心为原点的坐标系
        float max = Mathf.Max(Mathf.Abs(viewportPosition.x), Mathf.Abs(viewportPosition.y));
        viewportPosition = viewportPosition / max; // 确保箭头在屏幕边缘
        Vector2 arrowPosition = new Vector2(viewportPosition.x * (Screen.width / 2 - screenBorderBuffer), viewportPosition.y * (Screen.height / 2 - screenBorderBuffer));

        arrowRectTransform.anchoredPosition = arrowPosition;
    }

    void RotateArrowTowardsExit()
    {
        Vector2 direction = exit.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
