using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAnimPos : MonoBehaviour
{
    private float zAxis;
    //private float yAxis;
    // Start is called before the first frame update
    void Start()
    {
        zAxis = transform.position.z;
        //yAxis = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zAxis);
    }
}
