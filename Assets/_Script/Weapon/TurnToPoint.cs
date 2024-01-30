using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPoint : MonoBehaviour
{
    private Camera  _camera;

    [SerializeField]
    [Tooltip("FaceToPointer")]
    Vector3 MousePos;
    public  bool point_dir;
    public float angle;
    private Vector3 relative_pos;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        TurnToPointfuc();
    }

    void TurnToPointfuc()
    {
        MousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        relative_pos = MousePos - transform.position;
        point_dir = relative_pos.x > 0 ? true : false;
        angle = Mathf.Atan2(relative_pos.x, relative_pos.y) * Mathf.Rad2Deg;
        if (!point_dir) { angle = -(180 - Mathf.Abs(angle)); transform.rotation = Quaternion.Euler(180, 0, -angle); }
        else { transform.rotation = Quaternion.Euler(0, 0, -angle);}
    }
}
