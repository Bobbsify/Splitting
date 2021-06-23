using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private float height; 
    private float width;

    private float halfH;
    private float halfW;

    private float xTo;
    private float yTo;

    private float shakeLenght = 0.0f;
    private float shakeMagnitude = 0.0f;
    private float shakeRemain = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        height = 2f * Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

        halfH = height * 0.5f;
        halfW = width * 0.5f;

        xTo = transform.position.x;
        yTo = transform.position.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            shakeMagnitude = 4.0f;
            shakeRemain = 4.0f;
            shakeLenght = 12.0f;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

        // Update destination
        if (player)
        {
            xTo = player.transform.position.x;
            yTo = player.transform.position.y;
        }

        // Update position
        transform.position = new Vector3(transform.position.x + ((xTo - transform.position.x) / 25), transform.position.y + ((yTo - transform.position.y) / 25), transform.position.z);

        // Screen shake 
        if (shakeLenght > 0)
        {
            transform.position = new Vector3(transform.position.x + Random.Range(-shakeRemain, shakeRemain), transform.position.y + Random.Range(-shakeRemain, shakeRemain), transform.position.z);
            shakeRemain = Mathf.Max(0, shakeRemain - ((1 / shakeLenght) * shakeMagnitude * Time.deltaTime));
        }
       
    }
}
