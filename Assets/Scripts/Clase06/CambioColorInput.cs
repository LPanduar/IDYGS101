using UnityEngine;
using UnityEngine.InputSystem;

// Clase 06 - Ejercicio 3: la acción ChangeColor alterna entre blanco y rojo.
public class CambioColorInput : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool colorAlternativo;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnChangeColor(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        colorAlternativo = !colorAlternativo;

        spriteRenderer.color = colorAlternativo
            ? Color.red
            : Color.white;
    }
}
