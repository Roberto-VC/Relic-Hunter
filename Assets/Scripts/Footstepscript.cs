using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstepscript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject footstep;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


    }
}
