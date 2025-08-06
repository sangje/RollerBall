using System.Collections;
using UnityEngine;

public class AutonomousVehicle : MonoBehaviour
{
    [SerializeField]
    public float speed = 20.0f;
    [SerializeField]
    public float safeDistance = 10.0f;
    [SerializeField]
    public float stopDistance = 5.0f;
    [SerializeField]
    public float laneChangeDistance = 5.0f;
    [SerializeField]
    public float avoidanceSpeed = 10.0f;
    [SerializeField]
    public string[] obstacleTags;

    private bool changingLane = false;

    void Update()
    {
        
        RaycastHit frontHit;
        RaycastHit rearHit;

        if ( Physics.Raycast(transform.position, -transform.forward, out rearHit, stopDistance) && CheckTag(rearHit.collider.tag))
        {
          
            StopVehicle();
        }

        else if (Physics.Raycast(transform.position, -transform.forward, out rearHit, safeDistance) && CheckTag(rearHit.collider.tag))
        {
           
            SlowDown();
        }

        else if (Physics.Raycast(transform.position, transform.forward, out frontHit, safeDistance) && CheckTag(frontHit.collider.tag))
        {
            BustUp();
        }


        else
        {
           
            speed = 20.0f;
        }

        if (!changingLane)
        {
           
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, laneChangeDistance) && CheckTag(hit.collider.tag))
            {
              
                ChangeLane();
            }
        }

        transform.Translate(Vector3.back * Time.deltaTime * speed);
    }

    void SlowDown()
    {
       
        speed = avoidanceSpeed;
    }

    void ChangeLane()
    {
       
        Vector3 newLanePosition = transform.position + new Vector3(1, 0, 1) * 6.0f;
        StartCoroutine(MoveToNewLane(newLanePosition));
    }

    void StopVehicle()
    {

        speed = 0.0f;
    }

    void BustUp()
    {
        speed = 30.0f;
    }

    IEnumerator MoveToNewLane(Vector3 newPosition)
    {
        changingLane = true;
        float journeyLength = Vector3.Distance(transform.position, newPosition);
        float startTime = Time.time;
        float distanceCovered = 0.0f;

        while (distanceCovered < journeyLength)
        {
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, newPosition, fractionOfJourney);
            distanceCovered = (Time.time - startTime) * speed;
            yield return null;
        }

        changingLane = false;
    }

    bool CheckTag(string tagToCheck)
    {
        foreach (string obstacleTag in obstacleTags)
        {
            if (tagToCheck == obstacleTag)
            {
                return true;
            }
        }
        return false;
    }
}
