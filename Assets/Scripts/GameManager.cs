using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Setting variables
     public Ghost[] ghosts;
     public Pacman pacman;
     public Transform pellets;
     public int score { get; private set; }
    public int lives { get; private set; }

    // Starting the game and running the NewGame function
    private void Start()
    {
        NewGame();
    }

    // So when the game is over, to play again we click any key on keyboard
    private void Update()
    {
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
    }
    
    // We are setting the lives number
    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    // We are adding the score points when pacman eats the ghost
    public void GhostEaten(Ghost ghost)
    {
        SetScore(this.score + ghost.points);
    }
    
    // We are setting the activity when the ghost eats pacman
    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

}
