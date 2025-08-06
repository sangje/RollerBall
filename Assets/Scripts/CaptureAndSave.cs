using System.Collections;
using System.IO;
using UnityEngine;

public class CaptureAndSave : MonoBehaviour
{
    public string folderPath = "Assets/CapturedImages";
    public string fileNamePrefix = "Screenshot";
    public int captureInterval = 1; // �̹��� ĸó ����(��)

    private Camera captureCamera;

    void Start()
    {
        // 'Police' ���� ������Ʈ�� ����� ��� Camera ������Ʈ�� ã���ϴ�.
        Camera[] cameras = GetComponentsInChildren<Camera>();

        // ������ Camera�� ã���ϴ�. �� ���������� ù ��°�� ã�� Camera�� ����մϴ�.
        if (cameras.Length > 0)
        {
            captureCamera = cameras[0];
        }
        else
        {
            Debug.LogError("No Camera component found in the 'Police' hierarchy.");
            return; // ������Ʈ�� ������ ����
        }

        // ������ �������� ������ �����մϴ�.
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // ���� �������� �̹��� ĸó�� �����մϴ�.
        StartCoroutine(CaptureImages());
    }

    IEnumerator CaptureImages()
    {
        while (true)
        {
            yield return new WaitForSeconds(captureInterval);

            // captureCamera�� null�� �ƴ��� Ȯ��
            if (captureCamera != null)
            {
                CaptureAndSaveImage();
            }
            else
            {
                Debug.LogError("Camera component is null. Cannot capture and save image.");
            }
        }
    }

    void CaptureAndSaveImage()
    {
        // 'captureCamera'�� null���� Ȯ�� �� ��� ����
        if (captureCamera == null)
        {
            Debug.LogError("Camera component is null. Cannot capture and save image.");
            return;
        }

        // ī�޶󿡼� ��ũ������ ĸó�մϴ�.
        RenderTexture renderTexture = captureCamera.targetTexture;
        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture is null. Make sure the Camera component has a valid target texture.");
            return;
        }

        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(rect, 0, 0);
        screenshot.Apply();

        // ��ũ������ PNG ���Ϸ� �����մϴ�.
        byte[] bytes = screenshot.EncodeToPNG();
        string filePath = Path.Combine(folderPath, $"{fileNamePrefix}_{Time.time.ToString("yyyyMMdd_HHmmss")}.png");
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"�̹����� ����Ǿ����ϴ�: {filePath}");
    }
}
