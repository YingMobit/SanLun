using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class TrackingArrow : MonoBehaviour
{
    // ����
    public Transform player; // ���λ��
    public Transform exit; // ����λ��
    public float visibilityDistance = 20f; // ���Ӿ���
    private Vector3 screenCenter;
    private float screenBorderBuffer = 50f; // ��ͷ����Ļ�߽�Ļ������


    // ����
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
            // ���㷽��ָ��Ŀ��
            Vector3 direction = (gameObject.transform.position- player.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // ȷ����ͷλ��
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
        worldPosition.z = 0; // ȷ����ͷ�����ƶ���3D�ռ���
        transform.position = worldPosition;
    }
}
