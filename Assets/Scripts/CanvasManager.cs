using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    #region Variables for General
    public GridSystemSO gridSystemSo;
    #endregion

    #region Variables for Goals
    public GameObject goalPrefab;
    public Transform goals;
    private Image goalsBg;
    #endregion

    #region Variables for Move
    public GameObject moves;
    private TMPro.TextMeshProUGUI moveText;
    private int moveNum;
    private bool isHasMove = true;
    #endregion

    #region Variables for Win&Fail
    [HideInInspector] public bool isWin, isGameOver;
    public GameObject winPanel, failPanel;
    public GameObject[] confetties;
    #endregion

    private void Awake()
    {
        ObjectManager.CanvasManager = this;
    }

    void Start()
    {
        goalsBg = goals.GetChild(0).GetComponent<Image>();
        SetGoals();

        moveNum = gridSystemSo.MaxMove;
        moves = GameObject.FindGameObjectWithTag("Moves");
        moveText = moves.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        SetMoveCount(0);
    }

    #region Goals Setting
    private void SetGoals()
    {
        float posX = 0, distance = 120;
        int count = 0;
       
        goalsBg.rectTransform.sizeDelta = new Vector2(gridSystemSo.goals.Length *Screen.width/8 , 200);
        for (int i = 0; i < gridSystemSo.goals.Length; i++)
        {
            GameObject newGoal = Instantiate(goalPrefab);
            newGoal.transform.SetParent(goalsBg.transform);
            newGoal.name = gridSystemSo.goals[i].color.ToString();
            newGoal.GetComponent<GoalController>().colorType = gridSystemSo.goals[i].color;
            newGoal.GetComponent<GoalController>().value = gridSystemSo.goals[i].value;
            newGoal.tag = newGoal.GetComponent<GoalController>().colorType.ToString();
           
            if (gridSystemSo.goals.Length % 2 == 0)
            {
                if (i == 0)
                {
                    count++;
                    posX = count * distance * 0.5f;
                    newGoal.transform.localPosition = new Vector3(posX, 20, 0);
                }
                else if (i == 1)
                {
                    newGoal.transform.localPosition = new Vector3(-posX, 20, 0);
                }
                else if (i % 2 == 0)
                {
                    count++;
                    posX = (count * distance) - (distance * 0.5f);
                    newGoal.transform.localPosition = new Vector3(posX, 20, 0);
                }
                else
                    newGoal.transform.localPosition = new Vector3(-posX, 20, 0);
            }
            else
            {
                if (i == 0)
                    newGoal.transform.localPosition = new Vector3(0, 20, 0);
                else
                {
                    if (i % 2 != 0)
                    {
                        count++;
                        posX = count * distance;
                        newGoal.transform.localPosition = new Vector3(posX, 20, 0);
                    }
                    else
                        newGoal.transform.localPosition = new Vector3(-posX, 20, 0);
                }
            }
        }
    }
    #endregion

    #region MovesSetting
    public void SetMoveCount(int value)
    {
        moveNum -= value;
        moveText.text = moveNum.ToString();
        if (moveNum <= 0 && !isWin && isHasMove)
        {
            isHasMove = false;
            isGameOver = true;
            StartCoroutine(WaitForFailState());
        }
    }
    #endregion

    #region Win&Fail Statement
    public void WinState()
    {
        StartCoroutine(WaitForWinState());
    }

    IEnumerator WaitForWinState()
    {
        isWin = true;
        isGameOver = true;
        for (int i = 0; i < confetties.Length; i++)
        {
            confetties[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2);
        winPanel.SetActive(true);
        winPanel.transform.DOScale(Vector3.one, 1);
    }

    IEnumerator WaitForFailState()
    {
        yield return new WaitForSeconds(2);
        if (!isWin)
        {
            failPanel.SetActive(true);
            failPanel.transform.DOScale(Vector3.one, 1);
        }
    }

    public void ReloadScene()
    {
        DOTween.CompleteAll();
        DOTween.Clear();
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
    #endregion
}
