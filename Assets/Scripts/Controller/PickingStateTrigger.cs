using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingStateController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            player.SetPickingMode(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            player.SetPickingMode(false);
    }
}
