using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GemManager : MonoBehaviour
{
    public int count;
    public int treasures = 2;
    public TMP_Text scoreText;
    public TMP_Text scoreText2;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Gemas Encontradas: " + count.ToString();
        scoreText2.text = "Tesoros Ocultos: " + treasures.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Gemas Encontradas: " + count.ToString();
        scoreText2.text = "Tesoros Ocultos: " + treasures.ToString();
    }
}
