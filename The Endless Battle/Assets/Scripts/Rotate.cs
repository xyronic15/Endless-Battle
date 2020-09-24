using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate infinitely
        transform.Rotate(new Vector3(0, spinSpeed * Time.timeScale, 0), Space.Self);
    }
}
