using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Windows.WebCam;
using System;
using System.IO;

public class PhotoCaptureExample : MonoBehaviour
{
    PhotoCapture photoCaptureObject = null;
	string filepath = "Assets/capture.png";
    public GameObject rect;
    public GameObject graphRenderer;
    public Camera camera;

    // Use this for initialization
    public void TakePhoto()
    {
		print("take photo called");
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        // Create a PhotoCapture object
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
				print("taking picture xd");
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        Transform rectTransform = rect.GetComponent<Transform>();
        Vector3 rectPosition = rectTransform.position;
        Vector3 rectScale = rectTransform.localScale;
        Vector3 topLeft = rectPosition + new Vector3(-(rectScale.x / 2), -rectScale.y / 2, 0);
        Vector3 bottomRight = rectPosition + new Vector3(rectScale.x / 2, rectScale.y / 2, 0);
        Vector3 topLeftScreen = camera.WorldToScreenPoint(topLeft);
        Vector3 bottomRightScreen = camera.WorldToScreenPoint(bottomRight);

        Texture2D uncroppedImage = new Texture2D(1280, 720);
        photoCaptureFrame.UploadImageDataToTexture(uncroppedImage);
        int width = (int)bottomRightScreen.x - (int)topLeftScreen.x;
        int height = (int)bottomRightScreen.y - (int)topLeftScreen.y;
        print($"width = {width}, height = {height}, x = {topLeftScreen.x}, y = {topLeftScreen.y}");
        Texture2D croppedImage = new Texture2D(width, height);
        croppedImage.SetPixels(uncroppedImage.GetPixels((int)topLeftScreen.x, (int)topLeftScreen.y, width, height));
        byte[] pngImage = uncroppedImage.EncodeToPNG();
        byte[] croppedPngImage = croppedImage.EncodeToPNG();
        string enc2 = Convert.ToBase64String(croppedPngImage);
        string enc = Convert.ToBase64String(pngImage);
        print(enc);

        string path = "Assets/test.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(enc);

        string path2 = "Assets/testCrop.txt";
        StreamWriter writer2 = new StreamWriter(path2, true);
        writer2.WriteLine(enc2);

        StartCoroutine(graphRenderer.GetComponent<GraphDisplay>().imageProcessor.PostImage(enc2));

        writer.Close();
        writer2.Close();
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown the photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}