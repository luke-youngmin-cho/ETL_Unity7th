using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIGoogleStaticMap : MonoBehaviour
{
    public int zoomLevel
    {
        get => _zoomLevel;
        set
        {
            if (_zoomLevel == value)
                return;

            _zoomLevel = value;
            onZoomLevelChanged?.Invoke(value);
        }
    }

    private enum Resolution
    {
        Low = 1,
        High = 2,
    }
    private Resolution _resolution = Resolution.Low;
    private enum TextureStyle
    {
        Roadmap,
        Satellite,
        Gybrid,
        Terrain,
    }
    private TextureStyle _textureStyle = TextureStyle.Roadmap;
    private readonly string GOOGLE_MAP_API_KEY = "AIzaSyB3NA2eKGtGUYxPbxZWaHtijNfB8M6oWRo";
    private RectTransform _rawImageRectTransform;
    private RawImage _rawImage;
    private int _zoomLevel = 10;
    private Vector2 _imageSize;
    private IGPS _gps;
    public event Action<int> onZoomLevelChanged;


    private void Awake()
    {
        _rawImage = GetComponentInChildren<RawImage>();
        _rawImageRectTransform = _rawImage.GetComponent<RectTransform>();
        _imageSize = new Vector2(_rawImageRectTransform.rect.width, _rawImageRectTransform.rect.height);
        _gps = new MockUnitOfWork().gps;

        Button zoomIn = _rawImage.transform.Find("Button - ZoomIn").GetComponent<Button>();
        Button zoomOut = _rawImage.transform.Find("Button - ZoomOut").GetComponent<Button>();
        zoomIn.onClick.AddListener(() => zoomLevel++);
        zoomOut.onClick.AddListener(() => zoomLevel--);
        onZoomLevelChanged += value =>
        {
            zoomIn.interactable = value < 20;
            zoomOut.interactable = value > 5;
            StartCoroutine(C_RequestMap());
        };
    }

    private void Start()
    {
        StartCoroutine(C_RequestMap());
    }

    IEnumerator C_RequestMap()
    {
        string url = $"https://maps.googleapis.com/maps/api/staticmap?center={_gps.latitude},{_gps.longitude}&zoom={_zoomLevel}&size={_imageSize.x}x{_imageSize.y}&scale={_resolution}&maptype={_textureStyle}&key={GOOGLE_MAP_API_KEY}";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest(); // 웹 요청 및 응답 대기

        if (www.result == UnityWebRequest.Result.Success)
        {
            _rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        else if (www.result == UnityWebRequest.Result.InProgress)
        {
            Debug.Log($"[UIGoogleStaticMap] : WebRequest is not finished...");
        }
        else
        {
            Debug.LogWarning($"[UIGoogleStaticMap] : WebRequest has an error {www.error}");
        }
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch");
    }
}
