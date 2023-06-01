using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Transform matchedObjects;
    public bool isGameStart;
    #region Create Grid
    private CanvasManager canvasManager; //------------
    public GridSystemSO gridSystemSO;//-------------
    #endregion
    public int matchedCount, matchedColor;
    public List<GameObject> destroyedList = new List<GameObject>();
    public Queue<GameObject> trailQue = new Queue<GameObject>();
    private GameObject trails;
    [HideInInspector] public int doneGoals;

    private void Awake()
    {
        ObjectManager.GridManager = this; //-------------
    }

    void Start()
    {
        canvasManager = ObjectManager.CanvasManager;
        CreateGrid();
        CreateQueue();
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
        gridSystemSO.grids = new GameObject[gridSystemSO.column, gridSystemSO.row];
        for (int i = 0; i < gridSystemSO.column; i++)
        {
            for (int j = 0; j < gridSystemSO.row; j++)
            {
                GameObject grid = Instantiate(gridSystemSO.gridPrefab, GetWorldPosition(i, j), Quaternion.identity);
                grid.transform.SetParent(transform);
                grid.transform.localScale = Vector3.one * gridSystemSO.gridScale;
                gridSystemSO.grids[i, j] = grid;
            }
        }


        //Create Numbers
        gridSystemSO.numbers = new NumbersController[gridSystemSO.column, gridSystemSO.row];
        for (int i = 0; i < gridSystemSO.column; i++)
        {
            for (int j = 0; j < gridSystemSO.row; j++)
            {
                if (gridSystemSO.isSmootCreateStart)
                    SpawnNewNumber(i, j, NumberType.Empty);
                else
                {
                    GameObject newNumber = Instantiate(gridSystemSO.numberPrefabDict[NumberType.Normal], Vector3.zero, Quaternion.identity);
                    newNumber.name = "Number(" + i + "," + j + ")";
                    newNumber.transform.SetParent(transform);
                    newNumber.transform.localScale = Vector3.one * gridSystemSO.gridScale;
                    CloseNumbers(newNumber);
                    gridSystemSO.numbers[i, j] = newNumber.GetComponent<NumbersController>();
                    gridSystemSO.numbers[i, j].transform.position = gridSystemSO.grids[i, j].transform.position;
                    gridSystemSO.numbers[i, j].Init(i, j, this, NumberType.Normal);
                }


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
        if (!gridSystemSO.isSmootCreateStart && !isGameStart)
            CloseNumbers(newNumber);
        return gridSystemSO.numbers[column, row];
    }

    private void CloseNumbers(GameObject number)
    {
        number.GetComponent<SpriteRenderer>().enabled = false;
        number.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OpenNumbers(GameObject number)
    {
        number.GetComponent<NumbersController>().OpenNumber();
        //number.GetComponent<SpriteRenderer>().enabled = true;
        //number.transform.GetChild(0).gameObject.SetActive(true);
    }
    #endregion

    #region FillGridsWithNumbers

    public void StartFill()
    {
        StartCoroutine(Fill());
    }

    private IEnumerator Fill()
    {
        bool needsRefill = true;
        while (needsRefill)
        {

            float time = isGameStart ? gridSystemSO.fillTime : 0;
            yield return new WaitForSeconds(time);
            while (FillStep())
            {
                yield return new WaitForSeconds(time);
            }
            if (!isGameStart)
                SetEntryNumbers();
            needsRefill = ClearAllValidMatches();
        }

        //EndFill Events
        if (!needsRefill)
        {
            //Check Destroyed Numbers
            if (isGameStart)
            {
                destroyedList.Clear();
            }

            if (!isGameStart)
            {
                isGameStart = true;
                destroyedList.Clear();
                //OpenNumbers
                if (!gridSystemSO.isSmootCreateStart)
                {
                    for (int i = 0; i <= gridSystemSO.numbers.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= gridSystemSO.numbers.GetUpperBound(1); j++)
                        {
                            OpenNumbers(gridSystemSO.numbers[i, j].gameObject);
                        }
                    }
                }
            }
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
                GameObject newNumber = (GameObject)Instantiate(gridSystemSO.numberPrefabDict[NumberType.Normal], GetWorldPosition(i, -1), Quaternion.identity);
                newNumber.transform.SetParent(transform);

                //CloseStartNumbers if instantly refill
                if (!gridSystemSO.isSmootCreateStart && !isGameStart)
                    CloseNumbers(newNumber);

                gridSystemSO.numbers[i, 0] = newNumber.GetComponent<NumbersController>();
                gridSystemSO.numbers[i, 0].Init(i, -1, this, NumberType.Normal);
                gridSystemSO.numbers[i, 0].GetComponent<MovementManager>().Movement(i, 0, gridSystemSO.fillTime);
                gridSystemSO.numbers[i, 0].numberType = NumberType.Normal;
                movedPiece = true;
            }
        }
        return movedPiece;
    }


    public bool ClearAllValidMatches()
    {
        bool isNeedsRefill = false;
        for (int j = 0; j < gridSystemSO.row; j++)
        {
            for (int i = 0; i < gridSystemSO.column; i++)
            {
                if (gridSystemSO.numbers[i, j] == null)
                    print("null");
                List<NumbersController> match = GetMatch(gridSystemSO.numbers[i, j], i, j);
                if (match != null)
                {
                    for (int l = 0; l < match.Count; l++)
                    {
                        if (ClearPiece(match[l].Column, match[l].Row))
                        {
                            isNeedsRefill = true;
                        }

                    }
                }
            }
        }
        return isNeedsRefill;
    }
    public bool ClearPiece(int column, int row)
    {
        if (!gridSystemSO.numbers[column, row].IsBeingCleared)
        {

            if (!destroyedList.Contains(gridSystemSO.numbers[column, row].gameObject) && gridSystemSO.numbers[column, row].Type.Equals(NumberType.Normal))
                destroyedList.Add(gridSystemSO.numbers[column, row].gameObject);

            gridSystemSO.numbers[column, row].DestroyEvent();
            SpawnNewNumber(column, row, NumberType.Empty);
            return true;
        }
        return false;
    }

    public void SetEntryNumbers()
    {
        if (gridSystemSO.entryNumbers.Length > 0)
        {

            for (int i = 0; i < gridSystemSO.entryNumbers.Length; i++)
            {
                Destroy(gridSystemSO.numbers[gridSystemSO.entryNumbers[i].row, gridSystemSO.entryNumbers[i].column].gameObject);

                GameObject newNumber = Instantiate(gridSystemSO.numberPrefabDict[NumberType.Normal], Vector3.zero, Quaternion.identity);
                newNumber.GetComponent<NumbersController>().isEntryNumber = true;
                newNumber.GetComponent<NumbersController>().numberValue = gridSystemSO.entryNumbers[i].value;
                newNumber.name = "Number(" + gridSystemSO.entryNumbers[i].row + "," + gridSystemSO.entryNumbers[i].column + ")";
                newNumber.transform.SetParent(transform);
                newNumber.transform.localScale = Vector3.one * gridSystemSO.gridScale;
                if (!gridSystemSO.isSmootCreateStart && !isGameStart)
                    CloseNumbers(newNumber);
                gridSystemSO.numbers[gridSystemSO.entryNumbers[i].row, gridSystemSO.entryNumbers[i].column] = newNumber.GetComponent<NumbersController>();
                gridSystemSO.numbers[gridSystemSO.entryNumbers[i].row, gridSystemSO.entryNumbers[i].column].transform.position = gridSystemSO.grids[gridSystemSO.entryNumbers[i].row, gridSystemSO.entryNumbers[i].column].transform.position;
                gridSystemSO.numbers[gridSystemSO.entryNumbers[i].row, gridSystemSO.entryNumbers[i].column].Init(gridSystemSO.entryNumbers[i].row, gridSystemSO.entryNumbers[i].column, this, NumberType.Normal);
            }
        }
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
                if (!destroyedList.Contains(gridSystemSO.numbers[item.Column, item.Row].gameObject) && gridSystemSO.numbers[item.Column, item.Row].Type.Equals(NumberType.Normal))
                    destroyedList.Add(gridSystemSO.numbers[item.Column, item.Row].gameObject);
                item.DestroyEvent();
            }
            //StartCoroutine(Fill());
            //ClearAllValidMatches();
        }
    }
    #endregion

    #region Trail
    public void CreateQueue()
    {
        trails = GameObject.FindGameObjectWithTag("Trails");
        for (int i = 0; i < gridSystemSO.trailCount; i++)
        {
            GameObject trailTemp = Instantiate(gridSystemSO.trailPrefab);
            trailTemp.transform.SetParent(trails.transform);
            trailTemp.transform.localPosition = Vector3.zero;
            trailQue.Enqueue(trailTemp);
        }
    }
    #endregion

    #region End Game Statements
    public void CheckWin()
    {
        doneGoals++;
        if (doneGoals == gridSystemSO.goals.Length)
            canvasManager.WinState();
    }

    
    #endregion

}
