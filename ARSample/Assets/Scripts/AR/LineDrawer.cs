using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour, InputActions.IUIActions
{
    [SerializeField] private Camera _camera;
    private InputActions _inputActions;
    private LineRenderer _lineRenderer;
    private float _drawingOffset = 0.5f;
    private List<Vector3> _buffer = new List<Vector3>(100);

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
            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, _drawingOffset));
            _buffer.Add(worldPosition);
            RefreshLineRenderer();
        }
    }

    private void RefreshLineRenderer()
    {
        _lineRenderer.positionCount = _buffer.Count;

        // 이거 O(n) 이라서 불편한데 코드 최적화좀 해오세요
        for (int i = 0; i < _buffer.Count; i++)
        {
            _lineRenderer.SetPosition(i, _buffer[i]);
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
