using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChasingHunter))]
[RequireComponent(typeof(AttackingHunter))]
[RequireComponent(typeof(CameraVisibilityChecker))]
public class HuntModeSwitcher : MonoBehaviour
{

    private ChasingHunter chasingMode;
    private AttackingHunter attackingMode;
    private CameraVisibilityChecker cameraVisibilityChecker;

    private void Awake()
    {
        chasingMode = GetComponent<ChasingHunter>();
        attackingMode = GetComponent<AttackingHunter>();
        cameraVisibilityChecker = GetComponent<CameraVisibilityChecker>();

        chasingMode.setActive(false);
    }

    private void Update()
    {
        bool isVisible = cameraVisibilityChecker.isVisible();
        chasingMode.setActive(!isVisible);
        attackingMode.setActive(isVisible);
    }

}
