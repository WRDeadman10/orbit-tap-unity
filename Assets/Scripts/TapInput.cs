using UnityEngine;

public sealed class TapInput : MonoBehaviour
{
    private enum TapMode
    {
        SwitchDirection,
        JumpRadius
    }

    [SerializeField] private OrbitController orbitController;
    [SerializeField] private TapMode tapMode = TapMode.SwitchDirection;
    [SerializeField, Min(0.1f)] private float radiusStep = 0.75f;
    [SerializeField, Min(0.1f)] private float minimumRadius = 2.5f;
    [SerializeField, Min(0.1f)] private float maximumRadius = 4f;

    private void Update()
    {
        if (!WasTapped() || orbitController == null)
        {
            return;
        }

        if (tapMode == TapMode.SwitchDirection)
        {
            orbitController.SwitchDirection();
            return;
        }

        var nextRadius = orbitController.Radius + radiusStep;
        if (nextRadius > maximumRadius)
        {
            nextRadius = minimumRadius;
        }

        orbitController.SetRadius(nextRadius);
    }

    private static bool WasTapped()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).phase == TouchPhase.Began;
        }

        return Input.GetMouseButtonDown(0);
    }
}
