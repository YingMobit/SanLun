using UnityEngine;
using UnityEngine.UI;

public class TrackingArrow : MonoBehaviour
{
    public Transform player;
    public Transform exit;
    private RectTransform arrowRectTransform;
    private Camera mainCamera;
    private float screenBorderBuffer;

    private void Awake()
    {
        arrowRectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
        screenBorderBuffer = 500;
    }

    void Update()
    {
        if (player != null && exit != null && mainCamera != null)
        {
            PositionArrow();
            RotateArrowTowardsExit();
        }
    }

    void PositionArrow()
    {
        // 获取从玩家到出口的方向
        Vector2 fromPlayerToExit = exit.position - player.position;
        // 获取该方向上距离玩家某个距离的点，这里使用的是非常大的值确保它一定在屏幕外
        Vector3 farPoint = player.position + (Vector3)fromPlayerToExit.normalized * 1000f;
        // 将该点转换为屏幕空间坐标
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(farPoint);
        // 屏幕中心点
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        // 箭头应该指向的方向
        Vector2 direction = (screenPoint - screenCenter).normalized;
        // 计算屏幕边缘的点
        Vector2 edgePoint = screenCenter + direction * (Mathf.Min(screenCenter.x, screenCenter.y) - screenBorderBuffer);
        // 转换为以屏幕中心为原点的坐标系
        Vector2 anchoredPosition = edgePoint - screenCenter;

        // 设置箭头的RectTransform位置
        arrowRectTransform.anchoredPosition = anchoredPosition;
    }


    void RotateArrowTowardsExit()
    {
        Vector3 direction = exit.position - player.position;
        direction.z = 0; // 确保方向位于2D平面上
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowRectTransform.localEulerAngles = new Vector3(0, 0, angle); // 根据箭头设计可能需要调整角度
    }
}
