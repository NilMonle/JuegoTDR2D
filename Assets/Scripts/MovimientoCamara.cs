using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    public Transform player;       // Referencia al jugador
    public Vector3 offset;         // Offset normal de la cámara
    public float downOffset = 2f;  // Cuánto baja la cámara al presionar S
    public float smoothSpeed = 5f; // Velocidad de movimiento suave

    private Vector3 targetOffset;

    void Start()
    {
        targetOffset = offset;
    }

    void LateUpdate()
    {
        // Detectar si se presiona la tecla S
        if (Input.GetKey(KeyCode.S))
        {
            targetOffset = offset + Vector3.down * downOffset;
        }
        else
        {
            targetOffset = offset;
        }

        // Calcular la posición deseada
        Vector3 desiredPosition = player.position + targetOffset;

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
