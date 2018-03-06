using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caustics : MonoBehaviour {

    public float m_ScaleSpeed = 1.0f;
    public float m_ScaleDiff = 20.0f;
    public float m_RotSpeed = 1.0f;

    Vector3 m_PosOffset;
    float m_Timer = 0;

    private void Start()
    {
        m_PosOffset = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, m_RotSpeed * Time.deltaTime);
        transform.position = m_PosOffset + new Vector3(0, m_ScaleDiff * Mathf.Sin(m_Timer), 0);
        m_Timer += m_ScaleSpeed * Time.deltaTime;
        if (m_Timer >= 2 * Mathf.PI)
            m_Timer %= 2 * Mathf.PI;
    }
}
