using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    float m_Speed = 0.001f;
    public Vector2 m_SpeedRange = new Vector2(0.8f, 2.0f);
    public float m_FleeSpeed = 2.0f;
    float m_RotSpeed = 5.0f;
    bool m_Turning = false;
    Vector3 m_AverageHeading;
    Vector3 m_AveragePosition;
    float m_NeighborDistance = 2.0f;
    public List<Vector3> m_KnownSharks { get; private set; }
    GlobalFlock m_FlockManager;

    public enum FishState
    {
        Wandering, 
        Fleeing
    }
    public FishState m_FishState = FishState.Wandering;



    private void Start()
    {
        m_Speed = Random.Range(m_SpeedRange.x, m_SpeedRange.y);
    }

    public void Init(GlobalFlock gf)
    {
        m_FlockManager = gf;
    }

    private void Update()
    {
        if (m_FlockManager == null) return;

        switch(m_FishState)
        {
            case FishState.Wandering:
                if (m_Turning || Random.Range(0, 5) < 1)
                {
                    Vector3 direction = m_FlockManager.m_GoalPos - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(direction),
                        m_RotSpeed * Time.deltaTime);
                    m_Speed = Random.Range(m_SpeedRange.x, m_SpeedRange.y);
                }
                else if (Random.Range(0, 5) < 1)
                {
                    ApplyRules(m_FlockManager.m_GoalPos);
                }
                break;
            case FishState.Fleeing:
                ApplyFleeRules();
                break;
        }

        transform.Translate(new Vector3(0, 0, m_Speed * Time.deltaTime));
    }

    public void Flee(List<Vector3> sharks)
    {
        m_KnownSharks = sharks;
        m_FishState = FishState.Fleeing;
        m_Speed = m_FleeSpeed;
        StartCoroutine(FleeRoutine());
    }
    IEnumerator FleeRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        m_Speed = Random.Range(m_SpeedRange.x, m_SpeedRange.y);
        m_FishState = FishState.Wandering;
        m_KnownSharks.Clear();
    }

    void ApplyFleeRules()
    {
        Vector3 vavoid = Vector3.zero;
        foreach(Vector3 shark in m_KnownSharks)
        {
            vavoid += transform.position - shark;
        }
        Vector3 direction = vavoid - transform.position;
        if(direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                m_RotSpeed * Time.deltaTime);
        }
    }

    void ApplyRules(Vector3 goalPos)
    {
        List<GameObject> gos = m_FlockManager.m_AllFish;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        float dist;
        int groupSize = 0;
        foreach(GameObject go in gos)
        {
            if(go != gameObject)
            {

                dist = Vector3.Distance(go.transform.position, transform.position);
                if(dist <= m_NeighborDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;

                    if(dist < 1.5f)
                    {
                        vavoid += transform.position - go.transform.position;
                    }

                    Flock neighborFlock = go.GetComponent<Flock>();
                    gSpeed += neighborFlock.m_Speed;

                    //chance of fleeing when neighbors flee
                    if(neighborFlock.m_FishState == FishState.Fleeing)
                    {
                        if(Random.Range(0, 10) < 1)
                        {
                            Flee(neighborFlock.m_KnownSharks);
                        }
                    }
                }
            }
        }

        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPos - transform.position);
            m_Speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    m_RotSpeed * Time.deltaTime);
        }
    }

    public void GetEaten()
    {
        m_FlockManager.FishEaten(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_Turning = true;
    }
    private void OnTriggerExit(Collider other)
    {
        m_Turning = false;
    }
}
