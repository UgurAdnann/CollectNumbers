using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public GridSystemSO gridSystemSo;
    public GameObject goalPrefab;
    public Transform goals;
    private Image goalsBg;
    [HideInInspector] public bool isWin, isGameOver;
    public GameObject winPanel;
    public GameObject[] confetties;


    private void Awake()
    {
        ObjectManager.CanvasManager = this;
    }
    void Start()
    {
        goalsBg = goals.GetChild(0).GetComponent<Image>();
        SetGoals();
        winPanel = GameObject.FindGameObjectWithTag("WinPanel");
    }

    #region Goals Setting
    private void SetGoals()
    {
        float posX = 0, distance = 80;
        int count = 0;
        goalsBg.rectTransform.sizeDelta = new Vector2(gridSystemSo.goals.Length * 100, 100);
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
                    print(posX);
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

    #region Win&Fail Statement
    public void WinState()
    {
        StartCoroutine(WaitForWinState());
    }

    IEnumerator WaitForWinState()
    {
        isWin = true;
        isGameOver = true;
        print("Confetti");
        for (int i = 0; i < confetties.Length; i++)
        {
            confetties[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2);
        winPanel.SetActive(true);
        winPanel.transform.DOScale(Vector3.one, 1);
    }

    public void ReloadScene()
    {
        DOTween.CompleteAll();
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }
    #endregion
}
