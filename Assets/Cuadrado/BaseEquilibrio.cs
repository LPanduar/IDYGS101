using UnityEngine;
using UnityEngine.InputSystem;

// Base de equilibrio: el cuadrado gris lleva pegado el rectangulo y lo mueve todo junto.
// W/S = subir/bajar, flechas izq/der = mover a los lados, A/D = inclinar para equilibrar la pelota.
// R = reiniciar. Colores: W rojo, A/D naranja, S amarillo, sin teclas gris.
public class BaseEquilibrio : MonoBehaviour
{
    public Transform rectanguloAzul;   // el rectangulo (si se deja vacio busca "Square")
    public Rigidbody2D pelota;         // la pelotita (si se deja vacio busca "Circle")

    public float velocidad = 4f;       // movimiento con W/S y flechas
    public float giro = 60f;           // grados por segundo al inclinar con A y D
    public float inclinacionMax = 35f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector3 inicioPelota;
    Vector3 inicioBase;
    float angulo;

    // Si la escena tiene el cuadrado gris pero nadie le puso el script, se lo ponemos solos
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Registrar()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (escena, modo) =>
        {
            if (FindAnyObjectByType<BaseEquilibrio>() != null) return;
            var gris = GameObject.Find("Square (1)");
            if (gris != null && GameObject.Find("Square") != null && GameObject.Find("Circle") != null)
                gris.AddComponent<BaseEquilibrio>();
        };
    }

    void Awake()
    {
        if (rectanguloAzul == null)
        {
            var go = GameObject.Find("Square");
            if (go != null && go != gameObject) rectanguloAzul = go.transform;
        }
        if (pelota == null)
        {
            var go = GameObject.Find("Circle");
            if (go != null)
            {
                QuitarFisica3D(go);
                pelota = go.GetComponent<Rigidbody2D>();
                if (pelota == null) pelota = go.AddComponent<Rigidbody2D>();
            }
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.gray;

        // La base es cinematica: la movemos nosotros pero empuja a la pelota con fisica
        QuitarFisica3D(gameObject);
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        inicioBase = transform.position;
        if (GetComponent<Collider2D>() == null) gameObject.AddComponent<BoxCollider2D>();

        // Conectar el rectangulo al cuadrado gris para que se muevan juntos
        if (rectanguloAzul != null)
        {
            QuitarFisica3D(rectanguloAzul.gameObject);
            var rbAzul = rectanguloAzul.GetComponent<Rigidbody2D>();
            if (rbAzul != null) DestroyImmediate(rbAzul); // que use el Rigidbody de la base
            rectanguloAzul.SetParent(transform, true);
            if (rectanguloAzul.GetComponent<Collider2D>() == null)
                rectanguloAzul.gameObject.AddComponent<BoxCollider2D>();
        }

        // La pelota: circulo con gravedad y algo de friccion para que ruede sobre el rectangulo
        if (pelota != null)
        {
            if (pelota.GetComponent<Collider2D>() == null)
            {
                var col = pelota.gameObject.AddComponent<CircleCollider2D>();
                col.sharedMaterial = new PhysicsMaterial2D("Pelota") { friction = 0.6f, bounciness = 0.1f };
            }
            pelota.bodyType = RigidbodyType2D.Dynamic;
            pelota.gravityScale = 1f;
            pelota.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            pelota.interpolation = RigidbodyInterpolation2D.Interpolate;
            inicioPelota = pelota.transform.position;
        }
    }

    // La escena traia Rigidbody/BoxCollider 3D, que no chocan con la fisica 2D y no dejan poner Rigidbody2D
    static void QuitarFisica3D(GameObject go)
    {
        foreach (var c in go.GetComponents<Collider>()) DestroyImmediate(c);
        var rb3d = go.GetComponent<Rigidbody>();
        if (rb3d != null) DestroyImmediate(rb3d);
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        // Colores segun la tecla
        if (teclado.wKey.isPressed) sr.color = Color.red;
        else if (teclado.aKey.isPressed || teclado.dKey.isPressed) sr.color = new Color(1f, 0.5f, 0f);
        else if (teclado.sKey.isPressed) sr.color = Color.yellow;
        else sr.color = Color.gray;

        // Si la pelota se cae (o se pulsa R), reiniciar
        if (teclado.rKey.wasPressedThisFrame || (pelota != null && pelota.position.y < inicioBase.y - 10f))
            Reiniciar();
    }

    void FixedUpdate()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        Vector2 dir = Vector2.zero;
        if (teclado.wKey.isPressed) dir.y += 1f;
        if (teclado.sKey.isPressed) dir.y -= 1f;
        if (teclado.rightArrowKey.isPressed) dir.x += 1f;
        if (teclado.leftArrowKey.isPressed) dir.x -= 1f;

        float inclinar = 0f;
        if (teclado.aKey.isPressed) inclinar += 1f;   // A inclina a la izquierda
        if (teclado.dKey.isPressed) inclinar -= 1f;   // D inclina a la derecha

        angulo = Mathf.Clamp(angulo + inclinar * giro * Time.fixedDeltaTime, -inclinacionMax, inclinacionMax);

        rb.MovePosition(rb.position + dir * velocidad * Time.fixedDeltaTime);
        rb.MoveRotation(angulo);
    }

    void Reiniciar()
    {
        angulo = 0f;
        transform.SetPositionAndRotation(inicioBase, Quaternion.identity);
        rb.position = inicioBase;
        rb.rotation = 0f;
        if (pelota == null) return;
        pelota.linearVelocity = Vector2.zero;
        pelota.angularVelocity = 0f;
        pelota.position = inicioPelota;
        pelota.rotation = 0f;
    }
}
