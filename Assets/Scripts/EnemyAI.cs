using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float detectionRange = 20f; // Rango de detección
    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    public Animator animator;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rigidbody2D;
    private bool facingRight = true;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // Llama al método "UpdatePath" en x segundos, luego lo repite cada x segundos
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        // Inicia un nuevo cálculo de ruta si el anterior ha finalizado
        if (seeker.IsDone())
            seeker.StartPath(rigidbody2D.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        // Si no hay errores en el cálculo de la ruta, establece la nueva ruta
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        // Comprobar la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector2.Distance(rigidbody2D.position, target.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Si la distancia es menor o igual al rango de detección, el enemigo empieza a seguir al jugador
            // Comprueba si se ha alcanzado el final de la ruta
            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            // Obtiene la dirección hacia el siguiente waypoint
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody2D.position).normalized;

            // Calcula la fuerza a aplicar al enemigo en la dirección del siguiente waypoint
            Vector2 force = direction * speed * Time.deltaTime;
            
            animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));

            rigidbody2D.AddForce(force);

            // Averiguar la distancia hasta el próximo punto
            float distancia = Vector2.Distance(rigidbody2D.position, path.vectorPath[currentWaypoint]);

            if (distancia < nextWayPointDistance)
            {
                // Hemos alcanzado el próximo punto y nos movemos al siguiente 
                currentWaypoint++;
            }

            // Rotar el objeto para que mire en la dirección en la que se está moviendo en el eje horizontal
            if (direction.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && facingRight)
            {
                Flip();
            }
        }
        else
        {
            // Si el jugador está fuera del rango de detección, el enemigo se detiene o realiza otra acción
            animator.SetFloat("Speed", 0);
        }
    }

    void Flip()
    {
        // Cambia la dirección en la que mira el objeto
        facingRight = !facingRight;

        // Rota el objeto 180 grados alrededor del eje Y
        transform.Rotate(0f, 180f, 0f);
    }
}
