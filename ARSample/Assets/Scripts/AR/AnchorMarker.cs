using Google.XR.ARCoreExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using static GeospatialData;

[RequireComponent(typeof(LineRenderer))]
public class AnchorMarker : MonoBehaviour, InputActions.IUIActions
{
    [SerializeField] private ARRaycastManager _arRaycastManager;
    [SerializeField] private ARAnchorManager _arAnchorManager;
    [SerializeField] private AREarthManager _arEarthManager;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    [SerializeField] private Camera _camera;
    private InputActions _inputActions;
    [SerializeField] private Transform _rayHitVisualizer;
    private GeospatialData _geospatialData;
    private List<GeospatialData.Pose> _dataPoses; // 아직 앵커 추가 안된 대기중인 포즈들
    [SerializeField] private double _tolerance = 0.01;
    private IGPS _gps;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.UI.AddCallbacks(this);
        _inputActions.Enable();
        _geospatialData = Resources.Load<GeospatialData>($"GeospatialData/{nameof(GeospatialData)}");
        _dataPoses = _geospatialData.poses;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    IEnumerator Start()
    {
#if UNITY_EDITOR
        _gps = new MockUnitOfWork().gps;
#elif UNITY_ANDROID
        _gps = new UnitOfWork().gps;
#else
#endif

        yield return new WaitUntil(() => _gps.isValid);
        yield return new WaitUntil(() => _arEarthManager.EarthTrackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
        _arAnchorManager.AddAnchor(_gps.latitude, _gps.longitude, _gps.altitude, Quaternion.identity);
        _lineRenderer.positionCount = _dataPoses.Count;

        for (int i = 0; i < _dataPoses.Count; i++)
        {
            _arAnchorManager.AddAnchor(_dataPoses[i].latitude, _dataPoses[i].longitude, _gps.altitude, Quaternion.identity);
            _lineRenderer.SetPosition(i, new Vector3((float)_dataPoses[i].latitude, (float)_dataPoses[i].longitude, _gps.altitude));
        }
    }

    private void Update()
    {
        if (_arRaycastManager.Raycast(_camera.ViewportPointToRay(Vector2.one / 2), _hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes ))
        {
            _rayHitVisualizer.position = _hits[0].pose.position;
            _rayHitVisualizer.rotation = _hits[0].pose.rotation;
            GeospatialPose geospatialPose = _arEarthManager.Convert(_hits[0].pose);
            Debug.Log($"Target Lat : Lng : Alt : eunRot = {geospatialPose.Latitude} : {geospatialPose.Longitude} : {geospatialPose.Altitude} : {geospatialPose.EunRotation}");

            if (_dataPoses.Count > 0)
            {
                int index = _dataPoses.FindIndex(pose => Math.Abs(pose.latitude - geospatialPose.Latitude) < _tolerance &&
                                                                               Math.Abs(pose.longitude - geospatialPose.Longitude) < _tolerance); 

                if (index >= 0)
                {
                    if (_hits[0].trackable.TryGetComponent(out ARPlane plane))
                    {
                        _arAnchorManager.AttachAnchor(plane,
                                                                        new Pose(new Vector3((float)_dataPoses[index].latitude, (float)_dataPoses[index].longitude, _gps.altitude), _dataPoses[index].eunRotation));
                        Debug.Log($"[AnchroMarker] : Attached anchor at {_dataPoses[index].latitude}, {_dataPoses[index].longitude}, {_gps.altitude}");
                        _dataPoses.RemoveAt(index);
                    }
                }
            }
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        /*if (context.performed)
        {
            Vector2 touchPosition = context.ReadValue<Vector2>();

            if (_arRaycastManager.Raycast(_camera.ScreenPointToRay(touchPosition), _hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds))
            {
                if (_hits[0].trackable.gameObject.TryGetComponent(out ARPlane plane))
                {
                    _arAnchorManager.AttachAnchor(plane, _hits[0].pose);
                    Debug.Log("Attached Anchor !");
                }
            }
        }*/
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }
}
