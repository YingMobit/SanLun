using UnityEngine;

public class TrackingArrow : MonoBehaviour
{
    public Transform player;
    public Transform exit;
    private float screenBorderBuffer = 50f; // ��ͷ����Ļ�߽�Ļ������
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
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(player.position + (Vector3)fromPlayerToExit.normalized * 1000); // ��Ŀ��λ��ͶӰ����Զ�ĵط���ȷ������������Ļ��
        viewportPosition = viewportPosition * 2 - Vector2.one; // ת��������Ļ����Ϊԭ�������ϵ
        float max = Mathf.Max(Mathf.Abs(viewportPosition.x), Mathf.Abs(viewportPosition.y));
        viewportPosition = viewportPosition / max; // ȷ����ͷ����Ļ��Ե
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
