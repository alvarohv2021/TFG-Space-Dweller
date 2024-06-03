using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public playerMovement player;
    // Array de imágenes que representan las vidas del jugador
    public Image[] imagesLive;

    private float hurtAnimationDuration = .2f;
    private float invulnerabilityTime = 3f;
    private float invulnerabilityTimeAfterGetingHurt;
    [SerializeField]
    private GameObject blinkl;
    private Coroutine blinkCoroutine;

    void Start()
    {
        blinkl.SetActive(false);
        GetingHit();
    }

    void FixedUpdate()
    {
        if (Time.time <= invulnerabilityTimeAfterGetingHurt)
        {
            if (blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(Blink());
            }
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
                blinkl.SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Danger") && player.numOfLives > 0 && Time.time > invulnerabilityTimeAfterGetingHurt)
        {
            player.numOfLives--;  // Reduce las vidas antes de actualizar la UI
            GetingHit();
            AnimationGetingHit();

            if (player.numOfLives == 0)
            {
                player.interfaces.EnableGameOverMenu();
            }
        }
        if (collision.gameObject.tag == "Win")
        {
            player.interfaces.EnableWinMenu();
        }
    }

    void GetingHit()
    {
        // Establece las vidas iniciales
        for (int i = 0; i < imagesLive.Length; i++)
        {
            if (i < player.numOfLives)
            {
                imagesLive[i].enabled = true;  // Activa la imagen si hay una vida
            }
            else
            {
                imagesLive[i].enabled = false; // Desactiva la imagen si no hay vida
            }
        }
    }

    void AnimationGetingHit()
    {
        invulnerabilityTimeAfterGetingHurt = Time.time + invulnerabilityTime;
        player.animator.SetBool("IsHurt", true);
        Invoke("DoneGetingHurt", hurtAnimationDuration);
    }

    void DoneGetingHurt()
    {
        player.animator.SetBool("IsHurt", false);
    }

    private IEnumerator Blink()
    {
        while (Time.time <= invulnerabilityTimeAfterGetingHurt)
        {
            EnableBLink();
            yield return new WaitForSeconds(0.1f);
            DisableBLink();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void EnableBLink()
    {
        blinkl.SetActive(true);
    }

    void DisableBLink()
    {
        blinkl.SetActive(false);
    }
}
