using UnityEngine;

public class Movement3D : MonoBehaviour
{

    private void Update()
    {
        transform.position += Vector3.forward * 2 * Time.deltaTime;
    }

}
