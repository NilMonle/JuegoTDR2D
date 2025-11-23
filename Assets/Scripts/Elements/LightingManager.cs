using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum LightingMode
{
    Restricted,
    Full
}

public class LightingManager : MonoBehaviour
{
    [Header("Light References")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D playerLight;

    [Header("Intensities")]
    [SerializeField, Range(0f, 1f)] private float restrictedGlobalIntensity = 0f;
    [SerializeField, Range(0f, 1f)] private float fullGlobalIntensity = 1f;
    [SerializeField, Range(0f, 5f)] private float restrictedPlayerIntensity = 1.2f;
    [SerializeField, Range(0f, 5f)] private float fullPlayerIntensity = 0f;

    public static LightingManager Instance { get; private set; }

    private LightingMode currentMode = LightingMode.Restricted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ApplyLighting(currentMode);
    }

    public void SetMode(LightingMode mode)
    {
        if (currentMode == mode)
        {
            return;
        }

        currentMode = mode;
        ApplyLighting(currentMode);
    }

    private void ApplyLighting(LightingMode mode)
    {
        if (globalLight != null)
        {
            globalLight.intensity = mode == LightingMode.Full ? fullGlobalIntensity : restrictedGlobalIntensity;
        }

        if (playerLight != null)
        {
            playerLight.intensity = mode == LightingMode.Full ? fullPlayerIntensity : restrictedPlayerIntensity;
            playerLight.enabled = playerLight.intensity > 0f;
        }
    }
}