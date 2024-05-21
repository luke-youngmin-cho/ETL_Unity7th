using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class AnchorMarker : MonoBehaviour, InputActions.IUIActions
{
    [SerializeField] private ARRaycastManager _arRaycastManager;
    [SerializeField] private ARAnchorManager _arAnchorManager;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    [SerializeField] private Camera _camera;
    private InputActions _inputActions;

    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.UI.AddCallbacks(this);
        _inputActions.Enable();
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
        if (context.performed)
        {
            Debug.Log("Touched");
            Vector2 touchPosition = context.ReadValue<Vector2>();

            if (_arRaycastManager.Raycast(_camera.ScreenPointToRay(touchPosition), _hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds))
            {
                if (_hits[0].trackable.gameObject.TryGetComponent(out ARPlane plane))
                {
                    _arAnchorManager.AttachAnchor(plane, _hits[0].pose);
                    Debug.Log("Attached Anchor !");
                }
            }
        }
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
