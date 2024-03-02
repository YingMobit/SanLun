using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class TrackingArrow : MonoBehaviour
{
    // 声明
    public Transform player; // 玩家位置
    public Transform exit; // 出口位置
    public float visibilityDistance = 20f; // 可视距离
    private Vector3 screenCenter;
    private float screenBorderBuffer = 50f; // 箭头与屏幕边界的缓冲距离


    // 函数
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.position, gameObject.transform.position);

        if (distance > visibilityDistance)
        {
            // 计算方向并指向目标
            Vector3 direction = (gameObject.transform.position- player.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // 确定箭头位置
            PlaceArrowOnScreenBorder(direction);

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void PlaceArrowOnScreenBorder(Vector3 direction)
    {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 screenPosition = screenCenter + direction * (Mathf.Min(Screen.width, Screen.height) / 2 - screenBorderBuffer);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0; // 确保箭头不会移动到3D空间中
        transform.position = worldPosition;
    }
}
