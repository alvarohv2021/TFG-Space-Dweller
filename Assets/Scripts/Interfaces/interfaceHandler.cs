using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class interfaceHandler : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject winMenu;
    public GameObject pauseMenu;
    public GameObject scoreCount;
    public TMP_Text scoreText;
    private bool isPauseMenuActive = false;
    private int points = 0;
    private float lastUpdateTime;

    void Start()
    {
        if (gameOverMenu != null && pauseMenu != null)
        {
            gameOverMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }

        lastUpdateTime = Time.time;
        scoreText.text = points + " :Puntuación";
    }

    void FixedUpdate()
    {
        // Calcula la diferencia de tiempo desde la última actualización
        float currentTime = Time.time;
        float deltaTime = currentTime - lastUpdateTime;

        // Si ha pasado al menos un segundo, resta un punto por cada segundo transcurrido
        if (deltaTime >= 1f)
        {
            int secondsPassed = Mathf.FloorToInt(deltaTime);
            ScorePoints(-secondsPassed);
            lastUpdateTime = currentTime; // Actualiza el tiempo de la última actualización
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SeeControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego 
    }

    // Método para habilitar el menú de pausa
    public void TagglePause()
    {
        if (isPauseMenuActive)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego
            isPauseMenuActive = !isPauseMenuActive;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego 
            isPauseMenuActive = !isPauseMenuActive;
        }
    }

    internal void ScorePoints(int points)
    {
        // Solo actualiza los puntos si son mayores que 0 después de la resta
        if (this.points + points >= 0)
        {
            Debug.Log(points);
            this.points += points;
            scoreText.text = this.points + " :Puntuación";
        }
        else
        {
            this.points = 0;
            scoreText.text = this.points + " :Puntuación";
        }
    }

    public void EnableWinMenu()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego 
    }

}
