using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NumbersController : MonoBehaviour
{
    public GridSystemSO gridSystemSO;
    private CanvasManager canvasManager;
    public MovementManager movementManager;
    public int numberValue, colorValue;
    private string colorName;
    TMPro.TextMeshPro numberText;
    [HideInInspector] public NumberType numberType;
    private Animator animator;
    public bool isEntryNumber;
    private int column, row;
    private GameObject target;
    private Vector3 startScale;

    #region Variables for Destroy
    private bool isBeingCleared = false, isDestroyed;
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
        transform.localScale = Vector3.one * gridSystemSO.gridScale;
    }

    #region Input System
    private void OnMouseDown()
    {
        if (!canvasManager.isGameOver)
        {
            DOTween.Complete(21);
            transform.DOScale(Vector3.one * gridSystemSO.gridScale * 0.5f, 0.1f).SetEase(Ease.Linear).SetId(20);
        }
    }

    private void OnMouseUp()
    {
        DOTween.Complete(20);
        transform.DOScale(Vector3.one * gridSystemSO.gridScale, 0.1f).SetEase(Ease.Linear).SetId(21);
        if (numberValue != gridSystemSO.maxNumber && !isDestroyed && !canvasManager.isGameOver)
        {
            canvasManager.SetMoveCount(1);
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
        if (!isDestroyed)
        {
            isBeingCleared = true;
            StartCoroutine(WaitDestroyEvent());
            isDestroyed = true;
        }
    }

    private IEnumerator WaitDestroyEvent()
    {

        if (animator != null)
        {
            animator.enabled = true;
             animator.SetTrigger("Destroy");
        }
        NumbersController newNumber = gridManager.SpawnNewNumber(Column, Row, NumberType.Empty);
        if (gridManager.isGameStart && !isDestroyed)
        {
            SetTargetNumber();
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(newNumber.gameObject);
        Destroy(gameObject);
    }

    private void MoveTrail(GameObject newtarget)
    {
        if (gridManager.trailQue.Count <= 0)
            gridManager.CreateQueue();
        GameObject newTrail = gridManager.trailQue.Dequeue();
        newTrail.transform.position = transform.position;
        newTrail.SetActive(true);
        newTrail.GetComponent<TrailController>().Movement(newtarget,this);
    }

    private void SetExpFx()
    {
        if(gridManager.expFxQue.Count <= 0)
            gridManager.CreateFxQueue();
        GameObject newFx = gridManager.expFxQue.Dequeue();
        newFx.transform.position = transform.position;
        newFx.SetActive(true);
        newFx.GetComponent<ExpFxController>().SetColor(this);
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
        if (Type.Equals(NumberType.Normal) && colorName != null)
        {
            target = GameObject.FindGameObjectWithTag(colorName);
            SetExpFx();
            if (target != null)
            {
                MoveTrail(target);
            }
        }
    }

}
