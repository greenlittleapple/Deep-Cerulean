using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : MonoBehaviour {

    public float m_SightRange = 12.0f;
    public float m_SightConeRadius = 30.0f;
    public int m_ScanEnvironmentPercent = 10;
    float m_Hunger = 0;

    OtherSharkController m_SharkController;

    private void Start()
    {
        m_SharkController = GetComponent<OtherSharkController>();
    }
    private void Update()
    {
        if(m_SharkController.m_SharkState == OtherSharkController.SharkState.Wandering
            && Random.Range(0, 100) < m_Hunger)
        {
            ScanEnvironment();
        }

        if(m_Hunger < m_ScanEnvironmentPercent)
        {
            m_Hunger += Time.deltaTime * 0.1f;
        }
    }

    void ScanEnvironment()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_SightRange);
        for(int i=0; i<hitColliders.Length; i++)
        {
            if(hitColliders[i].gameObject.GetComponent<Flock>() != null
                || hitColliders[i].gameObject.CompareTag("OtherShark"))
            {
                
                Vector3 direction = hitColliders[i].transform.position - transform.position;
                if(Vector3.Angle(direction, transform.forward) < m_SightConeRadius)
                {
                    if (hitColliders[i].gameObject.CompareTag("OtherShark"))
                        Debug.Log("Shark Attack!");
                    m_SharkController.Hunt(hitColliders[i].gameObject);
                    return;
                }
            }
        }
    }

    public void Eat()
    {
        m_Hunger = 0;
    }
}
