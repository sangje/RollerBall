using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForward : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

      transform.position += Vector3.forward * 5 * Time.deltaTime;
  
    }
}