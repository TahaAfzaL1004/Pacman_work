using UnityEngine;

public class PacmanDeath : MonoBehaviour
{
    public void Die()
{Debug.Log("Pacman was eaten!");

GameManager.Instance.PacmanDied();}
}