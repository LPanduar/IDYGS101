using UnityEngine;

// Clase 04 - Segundo Script: cambia el color del Sprite.
// CambiarColor() puede ser llamado desde otros Scripts (Movimiento, Reto final).
public class CambioColor : MonoBehaviour
{
    public KeyCode teclaColor = KeyCode.Space;
    public Color colorTecla = Color.red;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKey(teclaColor))
        {
            CambiarColor(colorTecla);
        }
    }

    public void CambiarColor(Color nuevoColor)
    {
        spriteRenderer.color = nuevoColor;
    }
}
