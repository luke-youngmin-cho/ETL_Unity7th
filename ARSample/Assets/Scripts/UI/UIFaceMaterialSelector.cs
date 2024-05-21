using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class UIFaceMaterialSelector : MonoBehaviour
{
    [SerializeField] private ARFaceManager _faceManager;
    [SerializeField] private Material[] _faceMaterials;
    private List<ARFace> _faces = new List<ARFace>();

    private void Awake()
    {
        _faceManager.facesChanged += OnFacesChanged;

        RectTransform content = transform.Find("Scroll View/Viewport/Content") as RectTransform;
        GridLayoutGroup gridLayoutGroup = content.GetComponent<GridLayoutGroup>();
        content.sizeDelta = new Vector2(gridLayoutGroup.padding.left + gridLayoutGroup.padding.right + gridLayoutGroup.cellSize.x * _faceMaterials.Length + gridLayoutGroup.spacing.x * (_faceMaterials.Length - 1),
                                                        gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom + gridLayoutGroup.cellSize.y);

        for (int i = 0; i < _faceMaterials.Length; i++)
        {
            int tmpIdx = i;
            Material material = _faceMaterials[tmpIdx];
            GameObject slot = new GameObject($"Button - {material.name}");
            slot.transform.SetParent(content);
            Button button = slot.AddComponent<Button>();
            button.targetGraphic = slot.AddComponent<Image>();
            button.onClick.AddListener(() =>
            {
                foreach (var face in _faces)
                {
                    face.GetComponent<MeshRenderer>().material = material;
                }
            });
            RawImage rawImage = new GameObject("Icon").AddComponent<RawImage>();
            rawImage.transform.SetParent(slot.transform);
            rawImage.texture = material.mainTexture;
            rawImage.color = material.color;
            rawImage.raycastTarget = false;
            RectTransform rawImageRectTransform = rawImage.transform as RectTransform;
            rawImageRectTransform.anchorMin = Vector2.zero;
            rawImageRectTransform.anchorMax = Vector2.one;
            rawImageRectTransform.pivot = new Vector2(0.5f, 0.5f);
            rawImageRectTransform.offsetMin = Vector2.zero; // left, bottom
            rawImageRectTransform.offsetMax = Vector2.zero; // -right, -top
            rawImageRectTransform.localScale = Vector3.one;
            rawImageRectTransform.anchoredPosition = Vector2.zero;
        }
    }
    private void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        foreach (var item in args.added)
        {
            _faces.Add(item);
        }

        foreach (var item in args.removed)
        {
            _faces.Remove(item);
        }
    }
}
