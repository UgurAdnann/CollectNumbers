using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersController : MonoBehaviour
{
    public GridSystemSO gridSystemSO;
    public MovementManager movementManager;
    public int numberValue, colorValue;
    TMPro.TextMeshPro numberText;
    public NumberType numberType;

    
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

    private void Awake()
    {
        if (Type.Equals(NumberType.Normal) || Type.Equals(NumberType.Empty))
            movementManager = GetComponent<MovementManager>();
    }
    void Start()
    {
        numberText = transform.GetChild(0).GetComponent<TMPro.TextMeshPro>();
        SetColor();
    }

    #region Input System
    private void OnMouseDown()
    {
        print("Anim");
    }

    private void OnMouseUp()
    {
        if (numberValue != gridSystemSO.maxNumber)
        {
            numberValue++;
            SetColor();
            gridManager.DestroyMathed(this, Column, Row);
        }
    }
    #endregion

    private void SetColor()
    {
        if (Type.Equals(NumberType.Normal))
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

    }

    public void Init(int _column, int _row, GridManager _gridManager, NumberType _type)
    {
        column = _column;
        row = _row;
        gridManager = _gridManager;
        type = _type;
    }

    
}
