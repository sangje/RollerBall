using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public Vector3[] candidatePositions;
    public int numberOfCubesToGenerate = 30;
    public string targetTag; // 삭제 대상 태그 배열

    public void StartNewEpisode()
    {
        // 각 태그를 가진 모든 오브젝트 찾기
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);

        // 찾은 모든 오브젝트 삭제
        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }
    }
    

    public void GenerateCubes()
    {
        if (cubePrefab == null || candidatePositions.Length < numberOfCubesToGenerate)
        {
            Debug.LogError("Please assign the cubePrefab and make sure you have enough candidate positions.");
            return;
        }

        // Shuffle candidate positions array using Fisher-Yates algorithm
        for (int i = candidatePositions.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = candidatePositions[i];
            candidatePositions[i] = candidatePositions[j];
            candidatePositions[j] = temp;
        }

        // Create cubes at the selected positions
        for (int i = 0; i < numberOfCubesToGenerate; i++)
        {
            GameObject cube = Instantiate(cubePrefab, candidatePositions[i], Quaternion.identity);
            // You can further customize or modify the cube here if needed
        }
    }
}

