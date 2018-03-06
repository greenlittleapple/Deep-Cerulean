using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingController : MonoBehaviour
{
    public Hunt m_HuntingController;
    public AudioClip[] m_BiteSounds;
    AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OtherShark") && other.gameObject.name != "Head")
        {
            SharkBase otherController = other.GetComponentInParent<SharkBase>();
            otherController.GetBitten();
            PlayBiteSound();
        }
        else if(other.GetComponent<Flock>() != null) //eat fish
        {
            other.GetComponent<Flock>().GetEaten();

            m_HuntingController.Eat();
            PlayBiteSound();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("OtherShark") && other.gameObject.name == "Body" && GetComponentInParent<Transform>().GetComponentInParent<SharkBase>().mouthOpen)
        {
            Destroy(other.GetComponentInParent<SharkBase>().gameObject);
        }
    }

    void PlayBiteSound()
    {
        m_AudioSource.clip = m_BiteSounds[Random.Range(0, m_BiteSounds.Length)];
        m_AudioSource.Play();
    }
}
