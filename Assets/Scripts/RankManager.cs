using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//This script is used to keep track which rank is selected by the player. Also contains code for handling buttons.
public class RankManager : MonoBehaviour
{
    [Header("UI")]
    public Button rankDButton;
    public Button rankCButton;
    public Button rankBButton;
    public Button rankAButton;
    public Button rankSButton;
    public TextMeshProUGUI rankSelect;

    public enum Rank
    {
        D, C, B, A, S
    }

    public Rank selectedRank;

    public static RankManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);    //Only want one instance of rank manager
            return;
        }

        instance = this;

    }
   

    public void OnRankDButtonClicked()
    {
        Debug.Log("Pressed rank D");
        selectedRank = Rank.D;
        SceneManager.LoadScene("Game");
    }

    public void OnRankCButtonClicked()
    {
        selectedRank = Rank.C;
        SceneManager.LoadScene("Game");
    }

    public void OnRankBButtonClicked()
    {
        selectedRank = Rank.B;
        SceneManager.LoadScene("Game");
    }

    public void OnRankAButtonClicked()
    {
        selectedRank = Rank.A;
        SceneManager.LoadScene("Game");
    }

    public void OnRankSButtonClicked()
    {
        selectedRank = Rank.S;
        SceneManager.LoadScene("Game");
    }
}
