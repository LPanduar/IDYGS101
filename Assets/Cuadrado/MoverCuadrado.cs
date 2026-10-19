using UnityEngine;
using UnityEngine.InputSystem;

// Mueve el cuadrado con flechas o WASD y cambia su color cada vez que se pulsa una tecla de movimiento
public class MoverCuadrado : MonoBehaviour
{
    public float velocidad = 5f;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        Vector2 dir = Vector2.zero;
        if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) dir.y += 1;
        if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) dir.y -= 1;
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) dir.x += 1;
        if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) dir.x -= 1;

        transform.Translate(dir.normalized * velocidad * Time.deltaTime);

        bool nuevaTecla = teclado.wKey.wasPressedThisFrame || teclado.aKey.wasPressedThisFrame ||
                          teclado.sKey.wasPressedThisFrame || teclado.dKey.wasPressedThisFrame ||
                          teclado.upArrowKey.wasPressedThisFrame || teclado.downArrowKey.wasPressedThisFrame ||
                          teclado.leftArrowKey.wasPressedThisFrame || teclado.rightArrowKey.wasPressedThisFrame;

        if (nuevaTecla)
            sr.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.8f, 1f);
    }
}
