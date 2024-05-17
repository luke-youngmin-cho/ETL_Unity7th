using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class BallSpawner : MonoBehaviour, InputActions.IPlayerActions
{
    [SerializeField] private ARRaycastManager _raycaster;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _ball;
    private InputActions _inputActions;
    [SerializeField] private Transform _arCamera;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _inputActions = new InputActions();
        _inputActions.Player.SetCallbacks(this);
        _inputActions.Player.Enable();
    }

    private void Update()
    {
        if (_raycaster.Raycast(Mouse.current.position.ReadValue(), _hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            _lineRenderer.SetPosition(0, _arCamera.position);
            _lineRenderer.SetPosition(1, _hits[0].pose.position);
            _lineRenderer.enabled = true;
            Debug.Log("Hit");
            Debug.Log($"{transform.position}, {_hits[0].pose.position}");
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject ball = Instantiate(_ball, _arCamera.position + _arCamera.forward * 0.5f, _arCamera.rotation);
            ball.GetComponent<Rigidbody>().AddForce(_arCamera.forward * 500.0f);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }
}
