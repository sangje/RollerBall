using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{
    public RenderTexture targetTexture; // Inspector���� ��� �ؽ�ó�� �Ҵ��ϱ� ���� ����
    public int width = 120;
    public int height = 80;
    public Camera agentCamera; // ������Ʈ�� �پ� �ִ� ī�޶�

    void Start()
    {
        // ��� �ؽ�ó�� null�̸� ���ο� RenderTexture�� �����Ͽ� �Ҵ�
        if (targetTexture == null)
        {
            targetTexture = new RenderTexture(width, height, 24);
            targetTexture.antiAliasing = 1;
            targetTexture.filterMode = FilterMode.Trilinear;
        }

        // ī�޶��� ��� �ؽ�ó�� �Ҵ� (������Ʈ�� �پ� �ִ� ī�޶� ���)
        if (agentCamera != null)
        {
            agentCamera.targetTexture = targetTexture;
        }
        else
        {
            Debug.LogError("CameraRenderTexture Error.");
        }
    }
}

