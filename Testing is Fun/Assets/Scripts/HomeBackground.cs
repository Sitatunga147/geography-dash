using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBackground : MonoBehaviour
{
    private Transform transform;
    private float speed = 2.0f;

    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            if(transform.position.x <= -20)
            {
                transform.SetPositionAndRotation(new Vector3(20, 0, 0), transform.rotation);
            }
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
    }
}
