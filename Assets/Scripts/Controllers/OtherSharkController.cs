using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSharkController : SharkBase {

    public float m_WanderForce = 5;
    public float m_HuntForce = 20;

    Bounds m_WanderBounds = new Bounds(new Vector3(0, 25, 0), new Vector3(75, 25, 75));

    Vector3 m_WanderGoal = Vector3.zero;
    GameObject m_Prey;

    public enum SharkState
    {
        Wandering,
        Hunting,
        Cooldown,
        Fighting
    }
    public SharkState m_SharkState = SharkState.Wandering;


    new void Start () {
        base.Start();
	}

    new void Update() {
        base.Update();

        switch (m_SharkState)
        {
            
            case SharkState.Wandering:
                {
                    if (m_WanderGoal == Vector3.zero || Vector3.Distance(transform.position, m_WanderGoal) < 5.0f)
                    {
                        m_WanderGoal = GetWanderSpot();
                    }
                    Vector3 direction = m_WanderGoal - transform.position;
                    /*transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(direction),
                        m_TurnSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * m_WanderSpeed * Time.deltaTime);*/
                    TurnTowards(direction);
                    MoveForward(m_WanderForce);
                }
                break;
            case SharkState.Hunting:
                {
                    if(!m_WanderBounds.Contains(m_Prey.transform.position))
                    {
                        m_SharkState = SharkState.Wandering;
                        return;
                    }

                    Vector3 direction = m_Prey.transform.position - transform.position;
                    TurnTowards(direction);
                    MoveForward(m_HuntForce);

                    if(Vector3.Distance(m_Prey.transform.position, transform.position) < 5.0f
                        && Vector3.Dot(m_Prey.transform.position - transform.position, transform.forward) > 0)
                    {
                        //OpenMouth(true);
                        //Strike(direction);
                        Strike();
                        StartCoroutine(Cooldown());
                    }
                }
                break;
        }
	}

    Vector3 GetWanderSpot()
    {
        float wanderRadius = 20.0f;
        Vector3 spot = transform.position + Random.onUnitSphere * wanderRadius;
        if (!m_WanderBounds.Contains(spot))
            return m_WanderBounds.ClosestPoint(spot);
        else return spot;
    }

    public void Hunt(GameObject prey)
    {
        m_Prey = prey;
        m_SharkState = SharkState.Hunting;
    }
    IEnumerator Cooldown()
    {
        m_SharkState = SharkState.Cooldown;
        yield return new WaitForSeconds(0.5f);
        OpenMouth(true);
        yield return new WaitForSeconds(2.0f);
        m_WanderGoal = Vector3.zero;
        m_SharkState = SharkState.Wandering;
    }


}
