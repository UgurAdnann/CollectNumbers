using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    #region variables for General
    public GridSystemSO gridSystemSO;
    private NumbersController numberController;
    private IEnumerator moveCoroutine;
    #endregion

    private void Awake()
    {
        numberController = GetComponent<NumbersController>();
    }

    public void Movement(int newColumn, int newRow, float time)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = MoveCoroutine(newColumn, newRow, time);
        StartCoroutine(moveCoroutine);

        numberController.Column = newColumn;
        numberController.Row = newRow;

        Vector3 endPos = numberController.GridRef.GetWorldPosition(newColumn, newRow);

        numberController.transform.position = endPos;
    }

    private IEnumerator MoveCoroutine(int newColumn, int newRow, float time)
    {
        numberController.Column = newColumn;
        numberController.Row = newRow;

        Vector3 startPos = transform.position;
        Vector3 endPos = numberController.GridRef.GetWorldPosition(newColumn, newRow);

        for (float t = 0; t <= 1 * time; t += Time.deltaTime)
        {
            numberController.transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;
        }

        numberController.transform.position = endPos; //Loopta tam hedefe gitmeyebilir diye yazýldý
    }
}
