using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scoremanager : MonoBehaviour
{
    [SerializeField]
    private TextMesh effect;

    private TextMesh scoreText;

    private List<double> predictHits = new List<double>();
    private int score = 0;
    private float effectTime;

    void Start()
    {
        scoreText = GetComponent<TextMesh>();
    }

    void Update()
    {
        ComputeScore();
        predictHits.Clear();

        if (Time.realtimeSinceStartup - effectTime > 1)
        {
            effect.gameObject.SetActive(false);
        }

        scoreText.text = score.ToString();
    }

    void ComputeScore()
    {
        int tmp = 0;
        predictHits.ForEach(p => tmp += ComputeSingleScore(p));

        string str = "+" + tmp.ToString();

        if (predictHits.Count > 1)
        {
            float rate = 1f + ((float)predictHits.Count) / 10;
            tmp = (int)(((float)tmp) * rate);

            str += "*" + rate.ToString() + " = " + tmp.ToString();
        }

        score += tmp;
        if (tmp > 0)
        {
            effect.text = str;
            effect.gameObject.SetActive(true);
            effectTime = Time.realtimeSinceStartup;
        }
    }

    int ComputeSingleScore(double time)
    {
        if (time > 3)
        {
            return 10;
        }
        else if ( time > 2)
        {
            return 20;
        }
        else if ( time > 1)
        {
            return 30;
        }
        else if (time > 0.5)
        {
            return 50;
        }
        else if (time > 0)
        {
            return 100;
        }

        return 500;
    }

    public void AddPredictHit(double time)
    {
        predictHits.Add(time);
    }
}
