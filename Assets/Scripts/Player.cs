using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IManaged
{
    [SerializeField]
    float m_moveForce = 30f;
    Rigidbody m_rb = null;
    bool m_hasInitialized = false;

    public IGameSceneController gameSceneController { get; set; }

    public void Initialize()
    {
        m_rb = GetComponent<Rigidbody>();
        m_hasInitialized = true;
    }

    public void Setup()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        m_rb.Sleep();
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
                gameSceneController.SceneMext();
                break;

            case "Dead":
                gameSceneController.SceneReload();
                break;
        }
    }
}
