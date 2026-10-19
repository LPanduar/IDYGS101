using UnityEngine;
using UnityEngine.InputSystem;

// Clase 06 - Reto final: impulso de la pelota con Jump y detección del suelo.
public class PelotaInput : MonoBehaviour
{
    public float fuerzaImpulso = 4f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnJump(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        rb.AddForce(
            Vector2.up * fuerzaImpulso,
            ForceMode2D.Impulse
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Suelo")
        {
            Debug.Log("La pelota cayó al suelo.");
        }
    }
}
