using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersController : MonoBehaviour
{
    public GridSystemSO gridSystemSO;
    public int numberValue, colorValue;
    TMPro.TextMeshPro numberText;

    private int column, row;

    public int Column
    {
        get { return column; }
        set { column = value; }
    }

    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    private NumberType type;

    public NumberType Type
    {
        get { return type; }
    }

    private GridManager gridManager;

    public GridManager GridRef
    {
        get { return gridManager; }
    }

    void Start()
    {
        numberText = transform.GetChild(0).GetComponent<TMPro.TextMeshPro>();
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetColor()
    {
        if (numberValue == 0)
        {
            colorValue = Random.Range(1, gridSystemSO.useableMaxNumber + 1);
            numberValue = colorValue;
        }
        else
        {
            colorValue = numberValue;
        }
        GetComponent<SpriteRenderer>().color = gridSystemSO.colors[colorValue];
        numberText.text = numberValue.ToString();
    }

    public void Init(int _column, int _row, GridManager _gridManager, NumberType _type)
    {
        column = _column;
        row = _row;
        gridManager = _gridManager;
        type = _type;
    }
}
