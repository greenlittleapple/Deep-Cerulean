using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBase : MonoBehaviour {

    public float m_TurnTorque = 1.0f;
    public float m_StrikeForce = 3000.0f;
    public GameObject m_BloodPrefab;

    // Component vars
    protected Rigidbody rb;
    public Transform head;
    public Transform body;
    public Transform tail;
    public Animator m_Animator;

    // Game vars
    protected float health = 100f;
    public bool mouthOpen = false;
    protected float maxSpeed = 10f;

	// Use this for initialization
	public void Start () {
        //head = transform.Find("Head").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        //body = transform.Find("Body").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	public void Update () {
        body.rotation = Quaternion.Lerp(body.rotation, head.transform.rotation * Quaternion.Euler(90, 0, 0), .05f);
        body.position = Vector3.Lerp(body.position, head.transform.position - head.forward.normalized * 2.0f, .2f);
        tail.rotation = Quaternion.Lerp(tail.rotation, body.rotation, 0.1f);
        tail.position = Vector3.Lerp(tail.position, body.position - body.up.normalized * 2.4f, 0.5f);

        // Make sure the shark never moves faster than the maxSpeed
        if (rb.velocity.magnitude >= maxSpeed)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    // General code for moving a shark forward (e.g. setting velocity)
    public void MoveForward(float force = 5)
    {
        
        rb.AddForce(head.forward.normalized * force * (Mathf.Abs(head.transform.rotation.y - rb.transform.localRotation.y) > .10f ? 2f : 1));
    }

    // Code for turning (not swaying head, but veering the body)
    public void Turn(bool right)
    {
        if (rb.velocity.magnitude >= maxSpeed)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        rb.AddTorque(new Vector3(0, right ? m_TurnTorque : -m_TurnTorque, 0));
        rb.AddForce(head.transform.forward.normalized * .2f);
    }

    public void TurnTowards(Vector3 direction)
    {
        direction.Normalize();
        Turn(Vector3.Cross(transform.forward, direction).y > 0);
        //Ascend(direction.y > transform.forward.y);
        TurnHead(direction);
    }

    public void TurnHead(float amt)
    {
        head.transform.localRotation = Quaternion.Euler(0, amt * 90, 0);
    }
    public void TurnHead(float xAmt, float yAmt)
    {
        head.transform.localRotation = Quaternion.Euler(xAmt, yAmt, 0);
    }
    public void TurnHead(Vector3 direction)
    {
        head.transform.rotation = Quaternion.Slerp(head.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 2.0f);
    }

    // Ascending / Descending
    protected void Ascend(bool up)
    {
        if (rb.velocity.magnitude >= maxSpeed)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        rb.AddForce(new Vector3(0, up ? 2 : -2, 0));
    }

    // Opens or closes a shark's mouth, allowing or disallowing eating
    protected void OpenMouth(bool open)
    {
        //mouthOpen = open;
        if(open)
        {
            print("mouth open");
            // animation for opening mouth
            m_Animator.SetTrigger("OpenMouth");
        }
    }

    protected void Strike()//Vector3 direction)
    {
        m_Animator.SetTrigger("Strike");
        rb.AddForce(head.transform.forward.normalized * m_StrikeForce);
    }
    public void GetBitten()
    {
        Debug.Log("I have been bitten");

        StartCoroutine(Bleed());
    }
    IEnumerator Bleed()
    {
        GameObject blood = Instantiate(m_BloodPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5);
        Destroy(blood);

    }


}
