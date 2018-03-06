using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {

    public GameObject m_FishPrefab;
    public static float m_SpawnNewFishPercent = 0.1f;
    public static float m_TankSize = 10.0f;

    static int m_NumFish = 40;
    public List<GameObject> m_AllFish = new List<GameObject>();

    public Vector3 m_GoalPos = Vector3.zero;
    Vector3 m_SpawnPoint = Vector3.zero;

    static int fishBorn = 0;
    static int fishEaten = 0;


    private void Start()
    {
        m_SpawnPoint = transform.position;

        for(int i=0; i<m_NumFish; i++)
        {
            SpawnNewFish();
        }
    }

    private void Update()
    {
        if(Random.Range(0, 100) < 1)
        {
            m_GoalPos = GetPosInTank();
        }
        if(Random.Range(0, 100) < m_SpawnNewFishPercent)
        {
            if (fishBorn > fishEaten)
                m_SpawnNewFishPercent *= 0.5f;
            SpawnNewFish();
        }
    }

    void SpawnNewFish()
    {
        Vector3 pos = GetPosInTank();
        GameObject fish = Instantiate(m_FishPrefab, pos, Quaternion.identity);
        fish.GetComponent<Flock>().Init(this);
        m_AllFish.Add(fish);
    }
    public void FishEaten(GameObject fish)
    {
        m_AllFish.Remove(fish);
        if (fishEaten > fishBorn)
            m_SpawnNewFishPercent *= 2.0f;
    }

    Vector3 GetPosInTank()
    {
        return m_SpawnPoint + new Vector3(
            Random.Range(-m_TankSize, m_TankSize),
            Random.Range(-m_TankSize, m_TankSize),
            Random.Range(-m_TankSize, m_TankSize));
    }

}
