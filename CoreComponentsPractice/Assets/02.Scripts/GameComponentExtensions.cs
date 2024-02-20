using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameComponentExtensions
{
    public const float groundDetectDistance = 0.5f;
    public static bool IsGrounded(this Component component)
    {
        return Physics.Raycast(component.transform.position,
                               Vector3.down,
                               groundDetectDistance,
                               1 << LayerMask.NameToLayer("Ground"));
    }
}
