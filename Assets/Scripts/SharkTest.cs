using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkTest : MonoBehaviour {

    public float m_MoveSpeed = 5.0f;
    public float m_TurnSpeed = 10.0f;

	
	
	void Update () {
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * m_TurnSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, -Input.GetAxis("Vertical") * m_TurnSpeed * Time.deltaTime, Space.Self);

        if(Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.forward * m_MoveSpeed * Time.deltaTime);
        }
	}
}
