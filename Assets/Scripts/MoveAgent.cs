using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveAgent : Agent
{
    Rigidbody rBody; // Rigidbody
    //public Camera visualCamera;
    //public RadarSensor radarSensor;
    public Transform Target;
    public float lambdaReward =1f;
    public float PenaltyDistance = 100f;
    public float Reward = 0f;
    public SphereGenerator sphereGenerator; // Reference to the SphereGenerator script
    public Vector3[] targetPositions;
    public RaycastDistances raycastDistances;
    //public Camera visualCamera;
    public float forceMultiplier = 10;
    // private IWorker worker;
    // private Model model;


    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        raycastDistances = GetComponent<RaycastDistances>();

        // Load the ONNX model
        // model = ModelLoader.Load("YOLO.onnx");

        // // Create a Barracuda worker
        // worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    public override void OnEpisodeBegin()
    {
        sphereGenerator.StartNewEpisode();

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 2f, 0);
        // Setting Random Target Position
        Vector3 randomPosition = targetPositions[Random.Range(0, targetPositions.Length)];
        Target.transform.localPosition = randomPosition;

        sphereGenerator.GenerateCubes();

    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Reward=GetCumulativeReward()/(StepCount+1);

        // four discrete actions
        // var discreteActions = actionBuffers.DiscreteActions;

        // int discreteHorizontalAction = discreteActions[0];
        // int discreteVerticalAction = discreteActions[1];

        // // Use discrete actions for movement
        // HandleDiscreteActions(discreteHorizontalAction, discreteVerticalAction);

        // two continous actions = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Calculate distance
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reward Settings.
        float reward = lambdaReward*(1.0f - (distanceToTarget / PenaltyDistance));
        SetReward(reward); 

        // End Episode Conditions
        if (distanceToTarget < 2.42f)
        {
            SetReward(1.0f);
            Reward = 1.0f;
            EndEpisode();
        }

        // End when fall
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect Radar Observation
        if (raycastDistances != null && raycastDistances.Distances != null)
        {
            float[] distances = raycastDistances.Distances;

            for (int i = 0; i < distances.Length; i++)
            {
                float normalizedDistance = distances[i] / raycastDistances.maxDistance;
                sensor.AddObservation(normalizedDistance);
            }
        }
        else{
            Debug.LogError("RaycastDistances or its Distances property is null.");
        }

        // Collect Camera Observation
        // Use existing RenderTexture from the camera
        // RenderTexture.active = visualCamera.targetTexture;
        // Texture2D texture = new Texture2D(visualCamera.targetTexture.width, visualCamera.targetTexture.height, TextureFormat.RGB24, false);
        // texture.ReadPixels(new Rect(0, 0, visualCamera.targetTexture.width, visualCamera.targetTexture.height), 0, 0);
        // texture.Apply();
        // RenderTexture.active = null;

        // float[] normalizedPixels = GetNormalizedPixels(texture);

        // foreach (float pixel in normalizedPixels)
        // {
        //     sensor.AddObservation(pixel);
        // }
    }

    // Code for normalizing Pixels
    private float[] GetNormalizedPixels(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        float[] normalizedPixels = new float[pixels.Length * 3];

        for (int i = 0; i < pixels.Length; i++)
        {
            normalizedPixels[i * 3] = pixels[i].r;
            normalizedPixels[i * 3 + 1] = pixels[i].g;
            normalizedPixels[i * 3 + 2] = pixels[i].b;
        }

        return normalizedPixels;
    }
    // void CollectVisualObservation(VectorSensor sensor)
    // {
    //     // Capture the visual observation from the camera
    //     RenderTexture rt = new RenderTexture(visualWidth, visualHeight, 24);
    //     visualCamera.targetTexture = rt;
    //     Texture2D visualObservation = new Texture2D(visualWidth, visualHeight, TextureFormat.RGB24, false);
    //     visualCamera.Render();
    //     RenderTexture.active = rt;
    //     visualObservation.ReadPixels(new Rect(0, 0, visualWidth, visualHeight), 0, 0);
    //     visualObservation.Apply();
    //     RenderTexture.active = null;

    //     // Flatten and add the visual observation to the sensor
    //     Color[] pixels = visualObservation.GetPixels();
    //     foreach (Color pixel in pixels)
    //     {
    //         sensor.AddObservation(pixel.r);
    //         sensor.AddObservation(pixel.g);
    //         sensor.AddObservation(pixel.b);
    //     }

    //     Destroy(rt);
    // }

    // void CollectRadarObservation(VectorSensor sensor)
    // {
    //     // Assuming RadarSensor is a class you've defined to capture radar data
    //     float[] radarData = radarSensor.GetRadarData();

    //     // Add radar data to the sensor
    //     foreach (float dataPoint in radarData)
    //     {
    //         sensor.AddObservation(dataPoint);
    //     }
    // }

    public void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "others"){
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    // public override void Heuristic(in ActionBuffers actionsOut)
    // {
    //     var discreteActionsOut = actionsOut.DiscreteActions;

    //     // Map keyboard input to discrete actions
    //     int horizontalAction = 0;
    //     int verticalAction = 0;

    //     if (Input.GetKey(KeyCode.A)) horizontalAction = 1; // "a"
    //     if (Input.GetKey(KeyCode.D)) horizontalAction = 2; // "d"
    //     if (Input.GetKey(KeyCode.S)) verticalAction = 3; // "s"
    //     if (Input.GetKey(KeyCode.W)) verticalAction = 4; // "w"
        
    //     // Combine horizontal and vertical actions into a single discrete action
    //     discreteActionsOut[0] = horizontalAction;
    //     discreteActionsOut[1] = verticalAction;
    // }

    // private void HandleDiscreteActions(int horizontalAction, int verticalAction)
    // {
    //     // Implement logic to handle discrete actions
    //     // You can use these actions to control your agent's movement based on the discrete input
    //     // For example, update the agent's velocity or apply forces based on discrete actions.
    //     // You may also want to consider time scaling or other adjustments based on the discrete actions.
    //     // For simplicity, this example assumes that the discrete actions directly control the agent's position.

    //     // Example: Move left/right based on discrete horizontal action
    //     if (horizontalAction == 1) // "a"
    //     {
    //         rBody.AddForce(Vector3.left * forceMultiplier);
    //     }
    //     else if (horizontalAction == 2) // "d"
    //     {
    //         rBody.AddForce(Vector3.right * forceMultiplier);
    //     }

    //     // Example: Move forward/backward based on discrete vertical action
    //     if (verticalAction == 3) // "s"
    //     {
    //         rBody.AddForce(Vector3.back * forceMultiplier);
    //     }
    //     else if (verticalAction == 4) // "w"
    //     {
    //         rBody.AddForce(Vector3.forward * forceMultiplier);
    //     }
    // }
}

