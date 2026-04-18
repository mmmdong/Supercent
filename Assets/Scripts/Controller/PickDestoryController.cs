using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickDestoryController : RockDestroyController
{
    [SerializeField] private GameObject pick;
    private void OnEnable() => pick.SetActive(true);
    private void OnDisable() => pick.SetActive(false);
}
