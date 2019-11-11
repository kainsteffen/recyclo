using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public float backgroundSize;
    public Transform mainCamera;
    public float smoothing = 1f;
    public Transform[] layers;
    public float[] scales;




    public Vector3 previousCamPos;

	// Use this for initialization
	void Start ()
    {
        mainCamera = Camera.main.transform;
        previousCamPos = mainCamera.position;

        scales = new float[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            scales[i] = layers[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            float parallax = (previousCamPos.x - mainCamera.position.x) * scales[i];

            float targetPosX = layers[i].position.x + parallax;

            Vector3 targetPos = new Vector3(targetPosX, layers[i].position.y, layers[i].position.z);

            layers[i].position = Vector3.Lerp(layers[i].position, targetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = mainCamera.position;
	}
}
