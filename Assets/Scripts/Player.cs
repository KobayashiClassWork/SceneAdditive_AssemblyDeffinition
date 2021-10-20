using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IManaged
{
    [SerializeField]
    float m_moveForce = 30f;
    Rigidbody m_rb = null;
    bool m_hasInitialized = false;
    IGameSceneController m_gameSceneController = null;

    public void Initialize(IGameSceneController gameSceneController)
    {
        m_gameSceneController = gameSceneController;
        m_rb = GetComponent<Rigidbody>();
        m_hasInitialized = true;
    }

    private void Update()
    {
        if (!m_hasInitialized) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontal, 0, vertical);
        var forward_quaternion = Quaternion.identity;

        m_rb.AddForce(forward_quaternion * direction * m_moveForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Goal":

                break;

            case "Dead":

                break;
        }
    }


}
