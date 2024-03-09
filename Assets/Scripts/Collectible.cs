using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update

    public static Collectible instance;

    public TMP_Text scoreText;
    public TMP_Text scoreTex2;
    int score = 0;
    int tree = 2;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreText.text = "Gemas Encontradas: " + score.ToString();
        scoreTex2.text = "Tesoros por Encontrar: " + tree.ToString();
    }

    // Update is called once per frame


    public void AddPoint()
    {
        score += 1;
        scoreText.text = "Gemas Encontradas: " + score.ToString();

    }

    public void RemoveT()
    {
        tree -= 1;
        scoreTex2.text = "Tesoros por Encontrar: " + tree.ToString();
    }
}
