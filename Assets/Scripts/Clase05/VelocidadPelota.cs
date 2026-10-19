using UnityEngine;

// Clase 05 - Ejercicio 3: velocidad inicial con Rigidbody2D.linearVelocity.
public class VelocidadPelota : MonoBehaviour
{
    public float velocidadHorizontal = 4f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.linearVelocity = new Vector2(
            velocidadHorizontal,
            0f
        );
    }
}
