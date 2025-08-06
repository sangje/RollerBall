using System.Collections;
using System.IO;
using UnityEngine;

public class CaptureAndSave : MonoBehaviour
{
    public string folderPath = "Assets/CapturedImages";
    public string fileNamePrefix = "Screenshot";
    public int captureInterval = 1; // 이미지 캡처 간격(초)

    private Camera captureCamera;

    void Start()
    {
        // 'Police' 게임 오브젝트에 연결된 모든 Camera 컴포넌트를 찾습니다.
        Camera[] cameras = GetComponentsInChildren<Camera>();

        // 적절한 Camera를 찾습니다. 이 예제에서는 첫 번째로 찾은 Camera를 사용합니다.
        if (cameras.Length > 0)
        {
            captureCamera = cameras[0];
        }
        else
        {
            Debug.LogError("No Camera component found in the 'Police' hierarchy.");
            return; // 컴포넌트가 없으면 종료
        }

        // 폴더가 존재하지 않으면 생성합니다.
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 일정 간격으로 이미지 캡처를 시작합니다.
        StartCoroutine(CaptureImages());
    }

    IEnumerator CaptureImages()
    {
        while (true)
        {
            yield return new WaitForSeconds(captureInterval);

            // captureCamera가 null이 아닌지 확인
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
        // 'captureCamera'가 null인지 확인 후 계속 진행
        if (captureCamera == null)
        {
            Debug.LogError("Camera component is null. Cannot capture and save image.");
            return;
        }

        // 카메라에서 스크린샷을 캡처합니다.
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

        // 스크린샷을 PNG 파일로 저장합니다.
        byte[] bytes = screenshot.EncodeToPNG();
        string filePath = Path.Combine(folderPath, $"{fileNamePrefix}_{Time.time.ToString("yyyyMMdd_HHmmss")}.png");
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"이미지가 저장되었습니다: {filePath}");
    }
}
