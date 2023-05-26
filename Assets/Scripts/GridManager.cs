using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Transform matchedObjects;
    #region Create Grid
    private CanvasManager canvasManager; //------------
    public GridSystemSO gridSystemSO;//-------------
    #endregion
    private int matchedCount, matchedColor;


    private void Awake()
    {
        ObjectManager.GridManager = this; //-------------
    }

    void Start()
    {
        CreateGrid();
    }

    #region Create Grid
    private void CreateGrid()
    {
        //Add numberPrefabs to dictionary
        gridSystemSO.numberPrefabDict = new Dictionary<NumberType, GameObject>();
        for (int i = 0; i < gridSystemSO.numberPrefabs.Length; i++)
        {
            if (!gridSystemSO.numberPrefabDict.ContainsKey(gridSystemSO.numberPrefabs[i].type))
            {
                gridSystemSO.numberPrefabDict.Add(gridSystemSO.numberPrefabs[i].type, gridSystemSO.numberPrefabs[i].prefab);
            }
        }
        //Create Grids
        for (int i = 0; i < gridSystemSO.column; i++)
        {
            for (int j = 0; j < gridSystemSO.row; j++)
            {
                GameObject grid = Instantiate(gridSystemSO.gridPrefab, GetWorldPosition(i, j), Quaternion.identity);
                grid.transform.SetParent(transform);
                grid.transform.localScale = Vector3.one * gridSystemSO.gridScale;
            }
        }


        //Create Numbers
        gridSystemSO.numbers = new NumbersController[gridSystemSO.column, gridSystemSO.row];
        for (int i = 0; i < gridSystemSO.column; i++)
        {
            for (int j = 0; j < gridSystemSO.row; j++)
            {
                SpawnNewNumber(i, j, NumberType.Empty);
                //GameObject newNumber = Instantiate(gridSystemSO.numberPrefabDict[NumberType.Normal], Vector3.zero, Quaternion.identity);
                //newNumber.name = "Number(" + i + "," + j + ")";
                //newNumber.transform.SetParent(transform);
                //newNumber.transform.localScale = Vector3.one * gridSystemSO.gridScale;
                //gridSystemSO.numbers[i, j] = newNumber.GetComponent<NumbersController>();
                //gridSystemSO.numbers[i, j].Init(i, j, this, NumberType.Normal);

                //gridSystemSO.numbers[i, j].GetComponent<MovementManager>().Movement(i, j);
            }
        }
        StartCoroutine(Fill());
    }

    //Set Grids Positions
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2((transform.position.x - gridSystemSO.column / 2 + x) * gridSystemSO.gridScale, (transform.position.y + gridSystemSO.row / 2 - y) * gridSystemSO.gridScale);
    }

    public NumbersController SpawnNewNumber(int column, int row, NumberType type)
    {
        GameObject newNumber = Instantiate(gridSystemSO.numberPrefabDict[type], GetWorldPosition(column, row), Quaternion.identity);
        newNumber.transform.SetParent(transform);

        gridSystemSO.numbers[column, row] = newNumber.GetComponent<NumbersController>();
        gridSystemSO.numbers[column, row].Init(column, row, this, type);
        gridSystemSO.numbers[column, row].numberType = type;

        return gridSystemSO.numbers[column, row];
    }
    #endregion

    #region FillGridsWithNumbers

    public IEnumerator Fill()
    {
        while (FillStep())
        {
            yield return new WaitForSeconds(gridSystemSO.fillTime);
        }
    }

    public bool FillStep()
    {
        bool movedPiece = false;
        for (int j = gridSystemSO.row - 2; j >= 0; j--)
        {
            for (int i = 0; i < gridSystemSO.column; i++)
            {
                NumbersController number = gridSystemSO.numbers[i, j];

                NumbersController numberBelow = gridSystemSO.numbers[i, j + 1];
                if (numberBelow.Type == NumberType.Empty)
                {
                    Destroy(numberBelow.gameObject);
                    number.movementManager.Movement(i, j + 1, gridSystemSO.fillTime);
                    gridSystemSO.numbers[i, j + 1] = number;
                    SpawnNewNumber(i, j, NumberType.Empty);
                    movedPiece = true;

                }
            }
        }

        for (int i = 0; i < gridSystemSO.column; i++)
        {
            NumbersController numberBelow = gridSystemSO.numbers[i, 0];
            if (numberBelow.Type == NumberType.Empty)
            {
                Destroy(numberBelow.gameObject);
                GameObject newNumber = Instantiate(gridSystemSO.numberPrefabDict[NumberType.Normal], GetWorldPosition(i, -1), Quaternion.identity);
                newNumber.transform.SetParent(transform);

                gridSystemSO.numbers[i, 0] = newNumber.GetComponent<NumbersController>();
                gridSystemSO.numbers[i, 0].Init(i, -1, this, NumberType.Normal);
                gridSystemSO.numbers[i, 0].GetComponent<MovementManager>().Movement(i, 0, gridSystemSO.fillTime);
                gridSystemSO.numbers[i, 0].numberType = NumberType.Normal;
                movedPiece = true;
            }
        }
        return movedPiece;
    }

    #endregion

    #region Match
    public List<NumbersController> GetMatch(NumbersController number, int newColumn, int newRow)
    {
        List<NumbersController> horizontalNumbers = new List<NumbersController>();
        List<NumbersController> verticalNumbers = new List<NumbersController>();
        List<NumbersController> matchingNumbers = new List<NumbersController>();

        //Check Horizontal Numbers
        horizontalNumbers.Add(number);
        for (int dir = 0; dir <= 1; dir++)
        {
            for (int xOffset = 1; xOffset < gridSystemSO.column; xOffset++)
            {
                int x;
                if (dir == 0) //Left
                    x = newColumn - xOffset;
                else //Right
                    x = newColumn + xOffset;

                if (x < 0 || x >= gridSystemSO.column)
                    break;

                if (gridSystemSO.numbers[x, newRow].numberValue == number.numberValue)
                    horizontalNumbers.Add(gridSystemSO.numbers[x, newRow]);
                else
                    break;
            }
        }
        if (horizontalNumbers.Count >= gridSystemSO.matchNumber)
        {
            for (int i = 0; i < horizontalNumbers.Count; i++)
            {
                matchingNumbers.Add(horizontalNumbers[i]);
            }
        }
        //if (matchingNumbers.Count >= gridSystemSO.matchNumber)
        //{
        //    return matchingNumbers;
        //}

        //Check Vertical Numbers
        verticalNumbers.Add(number);
        for (int dir = 0; dir <= 1; dir++)
        {
            for (int yOffset = 1; yOffset < gridSystemSO.row; yOffset++)
            {
                int y;
                if (dir == 0) //Up
                    y = newRow - yOffset;
                else //Down
                    y = newRow + yOffset;

                if (y < 0 || y >= gridSystemSO.row)
                    break;

                if (gridSystemSO.numbers[newColumn, y].numberValue == number.numberValue)
                    verticalNumbers.Add(gridSystemSO.numbers[newColumn, y]);
                else
                    break;
            }
        }
        if (verticalNumbers.Count >= gridSystemSO.matchNumber)
        {
            for (int i = 0; i < verticalNumbers.Count; i++)
            {
                matchingNumbers.Add(verticalNumbers[i]);
            }
        }
        if (matchingNumbers.Count >= gridSystemSO.matchNumber)
        {
            return matchingNumbers;
        }
        return null;
    }

    public void DestroyMathed(NumbersController destroyNumber, int destroyColumn, int destroyRow)
    {
        if (GetMatch(destroyNumber, destroyColumn, destroyRow) != null)
        {
            foreach (var item in GetMatch(destroyNumber, destroyColumn, destroyRow))
            {
                item.transform.SetParent(matchedObjects);
            }
            matchedCount = matchedObjects.childCount;
            print("Matched Count: " + matchedCount);
            StartCoroutine(DestroyMatchedNumbers(destroyNumber, destroyColumn, destroyRow));
        }


    }
    IEnumerator DestroyMatchedNumbers(NumbersController destroyNumber, int destroyColumn, int destroyRow)
    {
        for (int i = 0; i < matchedCount; i++)
        {
            Destroy(matchedObjects.GetChild(0).gameObject);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(Fill());
    }
    #endregion


}
