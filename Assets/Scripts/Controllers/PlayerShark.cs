using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Child class of SharkBase because the Player shark shares many attributes with AI sharks
public class PlayerShark : SharkBase
{
    public float m_MoveSpeed = 25.0f;
    public float m_YawSpeed = 4.0f;
    public float m_PitchSpeed = 2.0f;


    new void Update()
    {
        base.Update();

        MoveForward(Input.GetAxisRaw("Thrust") * m_MoveSpeed);
        Vector3 direction = head.forward 
            + head.right * Input.GetAxis("Horizontal") * Time.deltaTime * m_YawSpeed
            + head.up * Input.GetAxis("Vertical") * Time.deltaTime * m_PitchSpeed;
        TurnTowards(direction);
    }


}
