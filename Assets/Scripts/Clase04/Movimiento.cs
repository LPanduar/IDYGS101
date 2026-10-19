using UnityEngine;

// Clase 04 - Actividad 2: el mismo movimiento, pero con W A S D.
// Reto: si el GameObject también tiene CambioColor, cada dirección cambia el color.
public class Movimiento : MonoBehaviour
{
    public float velocidad = 5f;

    private CambioColor cambioColor;

    void Start()
    {
        // Comunicación entre Scripts: buscamos el otro componente del mismo GameObject.
        cambioColor = GetComponent<CambioColor>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * velocidad * Time.deltaTime);
            AvisarColor(Color.green);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * velocidad * Time.deltaTime);
            AvisarColor(Color.yellow);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * velocidad * Time.deltaTime);
            AvisarColor(Color.red);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * velocidad * Time.deltaTime);
            AvisarColor(Color.blue);
        }
    }

    private void AvisarColor(Color nuevoColor)
    {
        if (cambioColor != null)
        {
            cambioColor.CambiarColor(nuevoColor);
        }
    }
}
