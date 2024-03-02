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
        // ��ȡ����ҵ����ڵķ���
        Vector2 fromPlayerToExit = exit.position - player.position;
        // ��ȡ�÷����Ͼ������ĳ������ĵ㣬����ʹ�õ��Ƿǳ����ֵȷ����һ������Ļ��
        Vector3 farPoint = player.position + (Vector3)fromPlayerToExit.normalized * 1000f;
        // ���õ�ת��Ϊ��Ļ�ռ�����
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(farPoint);
        // ��Ļ���ĵ�
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        // ��ͷӦ��ָ��ķ���
        Vector2 direction = (screenPoint - screenCenter).normalized;
        // ������Ļ��Ե�ĵ�
        Vector2 edgePoint = screenCenter + direction * (Mathf.Min(screenCenter.x, screenCenter.y) - screenBorderBuffer);
        // ת��Ϊ����Ļ����Ϊԭ�������ϵ
        Vector2 anchoredPosition = edgePoint - screenCenter;

        // ���ü�ͷ��RectTransformλ��
        arrowRectTransform.anchoredPosition = anchoredPosition;
    }


    void RotateArrowTowardsExit()
    {
        Vector3 direction = exit.position - player.position;
        direction.z = 0; // ȷ������λ��2Dƽ����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowRectTransform.localEulerAngles = new Vector3(0, 0, angle); // ���ݼ�ͷ��ƿ�����Ҫ�����Ƕ�
    }
}
