using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LightingZone : MonoBehaviour
{
    [SerializeField] private LightingMode lightingMode = LightingMode.Full;
    [SerializeField] private bool revertToRestrictedOnExit = true;

    private void Reset()
    {
        var triggerCollider = GetComponent<Collider2D>();
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || LightingManager.Instance == null)
        {
            return;
        }

        LightingManager.Instance.SetMode(lightingMode);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!revertToRestrictedOnExit || !collision.CompareTag("Player") || LightingManager.Instance == null)
        {
            return;
        }

        LightingManager.Instance.SetMode(LightingMode.Restricted);
    }
}