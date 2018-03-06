using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeShark : MonoBehaviour {

    public float m_SightRange = 10.0f;
    public float m_SightConeRadius = 30.0f;
    public int m_LookForSharkPercent = 10;

    Flock m_FlockScript;


    private void Start()
    {
        m_FlockScript = GetComponent<Flock>();
    }
    private void Update()
    {
        if(Random.Range(0, 100) < m_LookForSharkPercent)
        {
            LookForSharks();
        }
    }

    void LookForSharks()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_SightRange);
        List<Vector3> sharkPositions = new List<Vector3>();
        for(int i=0; i<hitColliders.Length; i++)
        {
            if(hitColliders[i].gameObject.GetComponent<SharkTest>() != null
                || hitColliders[i].gameObject.CompareTag("OtherShark"))
            {
                Vector3 direction = hitColliders[i].transform.position - transform.position;
                if(Vector3.Angle(direction, transform.forward) < m_SightConeRadius)
                {
                    sharkPositions.Add(hitColliders[i].transform.position);
                }
            }
        }

        if(sharkPositions.Count > 0)
        {
            m_FlockScript.Flee(sharkPositions);
        }
    }
}
