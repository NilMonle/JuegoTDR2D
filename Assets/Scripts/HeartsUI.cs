using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [Header("Referencias")]
    public Vida vidaJugador;

    [Header("UI (3 imágenes en escena)")]
    public List<Image> hearts;              // Arrastra aquí los 3 objetos Image (en orden: izquierda→derecha)
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Para saber cuál mostrábamos antes (útil si luego quieres efectos de ganancia/otros)
    int ultimoMostrado = -1;

    void OnEnable()
    {
        if (vidaJugador != null)
        {
            vidaJugador.onVidaCambia.AddListener(OnVidaCambia);
            vidaJugador.onDanio.AddListener(OnDanio);
            // Sincroniza al activar
            OnVidaCambia(vidaJugador.vidaActual, vidaJugador.vidaMax);
        }
    }

    void OnDisable()
    {
        if (vidaJugador != null)
        {
            vidaJugador.onVidaCambia.RemoveListener(OnVidaCambia);
            vidaJugador.onDanio.RemoveListener(OnDanio);
        }
    }

    void OnVidaCambia(int actual, int max)
    {
        // Seguridad: si tienes menos imágenes que vida máxima, solo usa las disponibles
        int n = Mathf.Min(hearts.Count, max);

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < n)
            {
                hearts[i].enabled = true;
                hearts[i].sprite = (i < actual) ? fullHeart : emptyHeart;
            }
            else
            {
                // Oculta imágenes sobrantes si hay más que el máximo
                hearts[i].enabled = false;
            }
        }

        ultimoMostrado = actual;
    }

    // Se llama ANTES que onVidaCambia (en tu clase Vida primero resta, llama onDanio y luego onVidaCambia)
    void OnDanio(int cantidad)
    {
        if (hearts == null || hearts.Count == 0 || vidaJugador == null) return;

        // Tras el daño, vidaActual ya está decrementada: este es el índice del corazón que se pierde
        int idx = Mathf.Clamp(vidaJugador.vidaActual, 0, hearts.Count - 1);
        if (hearts[idx] != null && hearts[idx].enabled)
            StartCoroutine(Punch(hearts[idx].rectTransform));
    }

    System.Collections.IEnumerator Punch(RectTransform rt)
    {
        float t = 0f;
        float dur = 0.18f;
        Vector3 baseScale = rt.localScale;
        Vector3 up = baseScale * 1.25f;

        // Sube rápido
        while (t < dur)
        {
            t += Time.unscaledDeltaTime; // UI → sin afectar por Time.timeScale
            float k = t / dur;
            rt.localScale = Vector3.Lerp(baseScale, up, k);
            yield return null;
        }

        // Baja rápido
        t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = t / dur;
            rt.localScale = Vector3.Lerp(up, baseScale, k);
            yield return null;
        }

        rt.localScale = baseScale;
    }
}
