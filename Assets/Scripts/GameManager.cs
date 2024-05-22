using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

// Setting variables
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public Transform pelletParent;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    public int ghostMultiplier = 1;
    public int score = 0;
    public int lives = 3;

    private void Awake()
    {
        if(GameManager.Instance != null)
            return; 
        GameManager.Instance = this;
    }

    // Starting the game and running the NewGame function
    private void Start()
    {
        NewGame();
    }

    // So when the game is over, to play again we click any key on keyboard
    private void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        livesText.text = "Lives: " + lives.ToString();

        if(this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    // We are starting a new game ans seting the score, lives and running the NewRound function
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }
    
    // We are starting a new round and the points are reseted
    private void NewRound()
    {
        foreach(Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }
    
    // The moving objects begins to move anain when we reset the state
    private void ResetState()
    {
        ResetGhostMultiplier();

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(true);
        }
        this.pacman.gameObject.SetActive(true);
    }
    
    // The moving objects are stopped when game is over
    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        this.pacman.gameObject.SetActive(false);
    }
    
    // We are setting the score
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + score.ToString();
    }
    
    // We are setting the lives number
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "Lives: " + lives.ToString();
    }

    // We are setting the activity when the ghost eats pacman
    public void PacmanEaten()
    {
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver();
        }
    }

    // We are adding the score points when pacman eats the ghost
    public void GhostEaten(Ghost ghost)
    {
        int point = ghost.points * this.ghostMultiplier;
        SetScore(this.score + point);
        this.ghostMultiplier++;
    }

    // We are adding the score when pacman eats the pellet
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }

    }

    // When the Power Pellet is eaten then the states of ghosts will change and Pacman will be able to eat them
    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);


    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pelletParent)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }

    internal void Reset()
    {
        pacman.ResetState();
    }
}
