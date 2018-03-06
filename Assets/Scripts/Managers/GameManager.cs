using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject m_EelPrefab;
    public int m_NumEels = 15;
    Bounds m_WanderBounds = new Bounds(new Vector3(0, 15, 0), new Vector3(75, 15, 75));


    private void Start()
    {
        RenderSettings.fogColor = Camera.main.backgroundColor;
        RenderSettings.fogDensity = 0.03f;
        RenderSettings.fog = true;

        for(int i=0; i<m_NumEels; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(m_WanderBounds.min.x, m_WanderBounds.max.x),
                Random.Range(m_WanderBounds.min.y, m_WanderBounds.max.y),
                Random.Range(m_WanderBounds.min.z, m_WanderBounds.max.z));

            Instantiate(m_EelPrefab, spawnPos, Random.rotation);
        }
    }
}
