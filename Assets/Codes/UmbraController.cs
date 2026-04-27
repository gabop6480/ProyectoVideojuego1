using UnityEngine;

public class UmbraMovement : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Detección de Suelo")]
    public Transform groundCheck; // Crea un objeto vacío a los pies de Umbra
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround; // Asigna aquí la capa de tu suelo/sombras

    private Rigidbody2D rb;
    private Animator anim;
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Lógica de animación: Solo nos importa si se está moviendo horizontalmente
        // No añadimos parámetros de salto al Animator para cumplir lo que pides
        anim.SetBool("isMoving", moveInput != 0);

        // Salto con Espacio
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Girar el sprite
        if (moveInput > 0) transform.localScale = new Vector3((float)2.5, (float)2.5, (float)2.5);
        else if (moveInput < 0) transform.localScale = new Vector3(-(float)2.5, (float)2.5, (float)2.5);
    }

    void FixedUpdate()
    {
        // Revisar si toca el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // Aplicar movimiento
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
}