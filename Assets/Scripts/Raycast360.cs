using UnityEngine;

public class Raycast360 : MonoBehaviour
{
    public int rayCount = 360; // ������ ����
    public float rayLength = 10f; // ������ ����
    public float[] distances; // �Ÿ����� ������ �迭

    void Start()
    {
        distances = new float[rayCount];
        CastRays();
    }

    void CastRays()
    {
        // ������ ���� ���� (��ü �߽�)
        Vector3 origin = transform.position;

        for (int i = 0; i < rayCount; i++)
        {
            // ���� ���� ���
            float angle = i * (360f / rayCount);

            // ������ ���� ���
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;

            // ����ĳ��Ʈ
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, rayLength))
            {
                // ���̰� ��ü�� �ε����� �� �Ÿ��� �迭�� ����
                distances[i] = hit.distance;
            }
            else
            {
                // ���̰� ��ü�� �ε����� �ʾ��� �� �ִ� �Ÿ����� �迭�� ����
                distances[i] = rayLength;
            }
        }
    }
}
