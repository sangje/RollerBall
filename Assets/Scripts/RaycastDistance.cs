using UnityEngine;

public class RaycastDistances : MonoBehaviour
{
    public int numberOfRays = 360;
    public float maxDistance = 100f;

    // Expose distances as a property
    public float[] Distances { get; private set; }

    void Start()
    {
        float[] distances_init = new float[numberOfRays];

        for (int i = 0; i < numberOfRays; i++)
        {
            distances_init[i] = 1.0f;
        }
        Distances = distances_init;
    }

    void Update()
    {
        Vector3 rayOrigin = transform.position;

        float[] distances = new float[numberOfRays];
        for (int i = 0; i < numberOfRays; i++)
        {
            Vector3 currentRayDirection = Quaternion.Euler(0f, i, 0f) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, currentRayDirection, out hit, maxDistance))
            {
                distances[i] = hit.distance;
                //Debug.Log($"Ray {i + 1}: {hit.collider.gameObject.name}, Distance: {hit.distance}");
            }
            else
            {
                distances[i] = maxDistance;
                //Debug.Log($"Ray {i + 1} did not hit, Max Distance: {maxDistance}");
            }
        }

        // Update the property with distances
        Distances = distances;

        //Debug.Log("Distances Array: " + string.Join(", ", Distances));
    }
}
