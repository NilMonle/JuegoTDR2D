using UnityEngine;
using UnityEngine.UI;

public class HeartsSpriteUI : MonoBehaviour
{
    [Header("Referencias")]
    public Vida vida;         // arrastra aquí el jugador (tiene Vida.cs)
    public Image targetImage; // arrastra aquí la Image del HUD

    [Header("Sprites por vida actual")]
    // index 0 = 0 corazones, index 1 = 1, index 2 = 2, index 3 = 3
    public Sprite[] spritesPorVida;

    void Start()
    {
        if (targetImage == null) targetImage = GetComponent<Image>();
        if (vida == null || targetImage == null || spritesPorVida == null || spritesPorVida.Length < 4)
        {
            Debug.LogError("HeartsSpriteUI: faltan referencias o sprites (debe haber 4: 0..3).");
            return;
        }

        vida.onVidaCambia.AddListener(ActualizarHUD);
        ActualizarHUD(vida.vidaActual, vida.vidaMax);
    }

    void OnDestroy()
    {
        if (vida != null) vida.onVidaCambia.RemoveListener(ActualizarHUD);
    }

    void ActualizarHUD(int vidaActual, int vidaMax)
    {
        // Asegura que hay sprite para 0..3
        int idx = Mathf.Clamp(vidaActual, 0, spritesPorVida.Length - 1);
        targetImage.sprite = spritesPorVida[idx];
        targetImage.preserveAspect = true;
    }
}
