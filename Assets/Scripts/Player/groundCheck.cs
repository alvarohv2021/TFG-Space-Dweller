using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public LayerMask groundLayer;
    private playerMovement playerMovement;

    private void Awake()
    {
        // Encuentra el script Movement en el padre 
        playerMovement = GetComponentInParent<playerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            playerMovement.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            playerMovement.isGrounded = false;
        }
    }
}
