using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelController : MonoBehaviour {

    public float m_TurnSpeed = 4.0f;
    public float m_MoveSpeed = 3.0f;

    Bounds m_WanderBounds = new Bounds(new Vector3(0, 15, 0), new Vector3(75, 15, 75));

    Vector3 m_WanderGoal = Vector3.zero;


    void Update()
    {

        if (m_WanderGoal == Vector3.zero || Vector3.Distance(transform.position, m_WanderGoal) < 5.0f)
        {
            m_WanderGoal = GetWanderSpot();
        }
        Vector3 direction = m_WanderGoal - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            m_TurnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * m_MoveSpeed * Time.deltaTime);
    }

    Vector3 GetWanderSpot()
    {
        float wanderRadius = 20.0f;
        Vector3 spot = transform.position + Random.onUnitSphere * wanderRadius;
        if (!m_WanderBounds.Contains(spot))
            return m_WanderBounds.ClosestPoint(spot);
        else return spot;
    }

}
