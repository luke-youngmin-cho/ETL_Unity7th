using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeospatialManager : MonoBehaviour
{
    private AREarthManager _arEarthManager;
    private IGPS _gps;


    private void Awake()
    {
        _arEarthManager = GetComponent<AREarthManager>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        _gps = new MockUnitOfWork().gps;
#elif UNITY_ANDROID
        _gps = new UnitOfWork().gps;
#else
#endif
        StartCoroutine(C_CheckValidation());
    }

    IEnumerator C_CheckValidation()
    {
        FeatureSupported featureSupported = FeatureSupported.Unsupported;

        yield return new WaitUntil(() =>
        {
            featureSupported = _arEarthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
            return featureSupported != FeatureSupported.Unknown; // Unknown : 아직 이 기기에서 GeospatialMode 가 지원되는지 결정안되었다는 값 (결정될때까지 기다려야함)
        });

        switch (featureSupported)
        {
            case FeatureSupported.Supported:
                {
                    // todo -> 초기화할거 쓰기
                    Debug.Log("This device supports geospatial mode !");
                }
                break;
            default:
                {
                    Debug.LogWarning($"This device does not support geospatial mode.. ");
                    Application.Quit();
                }
                break;
        }

        yield return new WaitUntil(() => _gps.isValid);
        VpsAvailabilityPromise vpsAvailablilityPromise = AREarthManager.CheckVpsAvailabilityAsync(_gps.latitude, _gps.longitude);
        yield return vpsAvailablilityPromise;
        Debug.Log($"{_gps.latitude}. {_gps.longitude}. {_gps.altitude}, {vpsAvailablilityPromise.Result}");
    }
}
