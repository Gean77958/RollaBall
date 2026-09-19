using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject restartButton; // <--- VARIABLE PARA EL BOTÓN DE REINICIAR

    public AudioClip loseSound;
    public AudioClip hitSound;
    public AudioClip winSound;  // <--- VARIABLE PARA EL AUDIO DE VICTORIA

    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        // Ocultamos el botón de reiniciar al comenzar la partida
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            GetComponent<AudioSource>().Play();
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        // CONDICIÓN DE VICTORIA
        if (count >= 12 && !isGameOver)
        {
            isGameOver = true;
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!";

            // Reproducir sonido de victoria
            if (winSound != null)
            {
                GetComponent<AudioSource>().PlayOneShot(winSound);
            }

            // Mostrar botón de jugar de nuevo
            if (restartButton != null)
            {
                restartButton.SetActive(true);
            }

            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // CONDICIÓN DE DERROTA
        if (collision.gameObject.CompareTag("Enemy") && !isGameOver)
        {
            isGameOver = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;

            if (loseSound != null)
            {
                GetComponent<AudioSource>().PlayOneShot(loseSound);
            }

            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";

            // Mostrar botón de jugar de nuevo
            if (restartButton != null)
            {
                restartButton.SetActive(true);
            }

            Destroy(gameObject, 2.0f);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (hitSound != null)
            {
                GetComponent<AudioSource>().PlayOneShot(hitSound);
            }
        }
    }
}