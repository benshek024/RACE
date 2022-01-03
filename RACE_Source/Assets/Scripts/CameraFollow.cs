using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objToFollow;
    public Vector3 eulerRotation;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = eulerRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (objToFollow == null)
        {
            Debug.LogError("ERROR: No game object is assigned to follow!");
            return;
        }

        Vector3 objPos = new Vector3(objToFollow.transform.position.x,
                                         objToFollow.transform.position.y + 20f,
                                         objToFollow.transform.position.z - 13f);

        transform.position = Vector3.Lerp(transform.position, objPos, smoothing * Time.deltaTime);
    }
}
