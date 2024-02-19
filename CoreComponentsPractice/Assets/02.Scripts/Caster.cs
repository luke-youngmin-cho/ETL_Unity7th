using UnityEngine;

public enum MyLayerMask
{
    Default =       0 << 0,
    BuiltInLayer0 = 1 << 0, // ... 00000000
    BuiltInLayer1 = 1 << 1, // ... 00000001
    BuiltInLayer2 = 1 << 2, // ... 00000010
    UserLayer3    = 1 << 3, // ... 00000100 // ground
    BuiltInLayer4 = 1 << 4, // ... 00001000 // water
    BuiltInLayer5 = 1 << 5, // ... 00010000
    UserLayer6    = 1 << 6, // ... 00100000
    UserLayer7    = 1 << 7, // ... 01000000

    UserLayer3_Or_BuiltInLayer4 = UserLayer3 | BuiltInLayer4,
}

public class Caster : MonoBehaviour
{
    [SerializeField] LayerMask _targetMask;

    private void OnEnable()
    {
        int targetMask = (int)(MyLayerMask.UserLayer3 | MyLayerMask.BuiltInLayer4);

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, float.PositiveInfinity, _targetMask))
        {
            if (hit.collider.TryGetComponent(out IHp hpInterface))
            {
                hpInterface.DepleteHp(10.0f);
            }
        }
    }
}
