using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CandiesColumn : MonoBehaviour
{
    [SerializeField]
    private Candy candyPrefab;

    public List<Candy> candyColumn = new List<Candy>();

    private float candyHeight;
    private float endRollTime = 0.3f;
    private float rollAnimToler = 100;
    private Roller roller;

    private void Start()
    {
        roller = GetComponentInParent<Roller>();
    }

    public void InitStartCandies(CandyParams[] candyParams)
    {
        candyColumn.AddRange(GetComponentsInChildren<Candy>());
        candyHeight = candyColumn[0].GetComponent<RectTransform>().sizeDelta.y;
        foreach (var candy in candyColumn)
        {
            candy.Init(candyParams[Random.Range(0, candyParams.Length)]);
        }
    }

    public void SetElement(int index, CandyParams candyParam)
    {
        candyColumn[index].Init(candyParam);
    }

    public void AddRandomElements(CandyParams[] candyParams)
    {
        int rollValue = Random.Range(20, 40);
        for (int i = 1; i <= rollValue; i++)
        {
            candyColumn.Insert(0, Instantiate(candyPrefab, transform));
            candyColumn[0].Init(candyParams[Random.Range(0, candyParams.Length)]);
            candyColumn[0].rt.anchoredPosition = new Vector2(0, candyHeight * i);
        }
    }

    public void RollLine()
    {
        float rollTime = Random.Range(0.6f, 3f);
        Sequence roll = DOTween.Sequence();

        roll.PrependCallback(() =>
        {
            foreach (var candy in candyColumn)
            {
                candy.Blur();
            }
        });

        for (int i = 0; i < candyColumn.Count; i++)
        {
            roll.Join(candyColumn[i].rt.DOAnchorPosY(i * -candyHeight - rollAnimToler, rollTime));
        }

        roll.AppendCallback(() =>
        {
            foreach (var candy in candyColumn)
            {
                candy.UnBlur();
            }
        });

        roll.Append(candyColumn[0].rt.DOAnchorPosY(0, endRollTime));
        for (int i = 1; i < candyColumn.Count; i++)
        {
            roll.Join(candyColumn[i].rt.DOAnchorPosY(i * -candyHeight, endRollTime));
        }

        roll.AppendCallback(() =>
        {
            for (int i = candyColumn.Count - 1; i >= StaticParams.MaxVertical; i--)
            {
                Destroy(candyColumn[i].gameObject);
                candyColumn.RemoveAt(i);
            }
            roller.LineFinished();
        });
        roll.Restart();
    }
}
