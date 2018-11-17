using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshPro[] ScoreTexts;
    public int[] Scores;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerScored(int playerIndex)
    {
        Scores[playerIndex]++;
        ScoreTexts[playerIndex].text = Scores[playerIndex].ToString();
    }
}
