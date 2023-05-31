using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersController : MonoBehaviour
{
    public GridSystemSO gridSystemSO;
    private CanvasManager canvasManager;
    public MovementManager movementManager;
    public int numberValue, colorValue;
    private string colorName;
    TMPro.TextMeshPro numberText;
    public NumberType numberType;
    private Animator animator;
    public bool isEntryNumber;
    private int column, row;
    private GameObject target;

    #region Variables for Destroy
    private bool isBeingCleared = false;
    public bool IsBeingCleared
    {
        get { return isBeingCleared; }
    }
    #endregion

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
        canvasManager = ObjectManager.CanvasManager;
        animator = GetComponent<Animator>();
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
            gridManager.ClearAllValidMatches();
            gridManager.StartFill();
        }
    }
    #endregion

    private void SetColor()
    {
        if (Type.Equals(NumberType.Normal))
        {
            if (numberValue == 0 || numberValue > gridSystemSO.maxNumber)
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

            if (colorValue == 1)
                colorName = "Red";
            if (colorValue == 2)
                colorName = "Green";
            if (colorValue == 3)
                colorName = "Blue";
            if (colorValue == 4)
                colorName = "Orange";
            if (colorValue == 5)
                colorName = "Pink";
        }

    }

    public void Init(int _column, int _row, GridManager _gridManager, NumberType _type)
    {
        column = _column;
        row = _row;
        gridManager = _gridManager;
        type = _type;
    }

    #region Destroy
    public void DestroyEvent()
    {
        isBeingCleared = true;
        StartCoroutine(WaitDestroyEvent());
    }

    private IEnumerator WaitDestroyEvent()
    {

        if (animator != null)
            animator.SetTrigger("Destroy");
        NumbersController newNumber = gridManager.SpawnNewNumber(Column, Row, NumberType.Empty);
        if (gridManager.isGameStart)
            SetTargetNumber();
        yield return new WaitForSeconds(0.2f);
        Destroy(newNumber.gameObject);
        Destroy(gameObject);
    }
    #endregion

    public void OpenNumber()
    {
        StartCoroutine(WaitOpenNumber());
    }

    IEnumerator WaitOpenNumber()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void SetTargetNumber()
    {
        if (Type.Equals(NumberType.Normal))
        {
            target = GameObject.FindGameObjectWithTag(colorName);
            if (target != null)
            {
                target.GetComponent<GoalController>().SetText(1);

            }
        }
    }

}
