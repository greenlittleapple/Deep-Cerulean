using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform m_Player;
    public Vector3 m_Offset = new Vector3(0, 5, -12);



    private void Start()
    {
        transform.position = GetOptimalPosition();
        transform.rotation = GetOptimalRotation();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
            GetOptimalRotation(),
            0.1f);
        transform.position = Vector3.Lerp(transform.position,
            GetOptimalPosition(),
            0.1f);
    }

    Vector3 GetOptimalPosition()
    {
        return m_Player.position
            + m_Player.right * m_Offset.x
            + m_Player.up * m_Offset.y
            + m_Player.forward * m_Offset.z;
    }
    Quaternion GetOptimalRotation()
    {
        Vector3 direction = m_Player.position + m_Player.forward * 5.0f - transform.position;
        return Quaternion.LookRotation(direction);
    }
}
