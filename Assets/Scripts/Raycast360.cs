using UnityEngine;

public class Raycast360 : MonoBehaviour
{
    public int rayCount = 360; // 레이의 개수
    public float rayLength = 10f; // 레이의 길이
    public float[] distances; // 거리값을 저장할 배열

    void Start()
    {
        distances = new float[rayCount];
        CastRays();
    }

    void CastRays()
    {
        // 레이의 시작 지점 (물체 중심)
        Vector3 origin = transform.position;

        for (int i = 0; i < rayCount; i++)
        {
            // 현재 각도 계산
            float angle = i * (360f / rayCount);

            // 레이의 방향 계산
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;

            // 레이캐스트
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, rayLength))
            {
                // 레이가 물체에 부딪혔을 때 거리를 배열에 저장
                distances[i] = hit.distance;
            }
            else
            {
                // 레이가 물체에 부딪히지 않았을 때 최대 거리값을 배열에 저장
                distances[i] = rayLength;
            }
        }
    }
}
