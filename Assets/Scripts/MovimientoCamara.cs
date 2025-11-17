using UnityEngine;
using Cinemachine;

public class CameraDownCinemachine : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public float downOffset = 2f;
    public float smoothSpeed = 5f;

    private CinemachineFramingTransposer transposer;
    private float defaultY;

    void Start()
    {
        transposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (transposer != null)
        {
            defaultY = transposer.m_TrackedObjectOffset.y;
        }
    }

    void Update()
    {
        if (transposer == null) return;

        float targetY = defaultY;

        if (Input.GetKey(KeyCode.S))
        {
            targetY = defaultY - downOffset;
        }

        // Movimiento suave
        Vector3 offset = transposer.m_TrackedObjectOffset;
        offset.y = Mathf.Lerp(offset.y, targetY, smoothSpeed * Time.deltaTime);
        transposer.m_TrackedObjectOffset = offset;
    }
}
