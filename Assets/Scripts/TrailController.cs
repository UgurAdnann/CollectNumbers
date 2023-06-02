using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrailController : MonoBehaviour
{
    GridManager gridManager;
  public  GridSystemSO gridSystemSO;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        
    }
    void Start()
    {
        gridManager = ObjectManager.GridManager;
    }

    public void Movement(GameObject target)
    {
        SetColor(target);
        transform.DOMove(Camera.main.ScreenToWorldPoint(target.transform.position), 15).SetEase(Ease.Linear).SetSpeedBased(true).OnStepComplete(() => EndMovement(target));
    }

    private void EndMovement(GameObject target)
    {
        target.GetComponent<GoalController>().SetText(1);
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        gridManager.trailQue.Enqueue(transform.gameObject);
    }

    private void SetColor(GameObject target)
    {
        if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Red))
        {
            spriteRenderer.color = gridSystemSO.colors[1];
            trailRenderer.startColor = gridSystemSO.colors[1];
            trailRenderer.endColor = gridSystemSO.colors[1];
        }
        if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Green))
        {
            spriteRenderer.color = gridSystemSO.colors[2];
            trailRenderer.startColor = gridSystemSO.colors[2];
            trailRenderer.endColor = gridSystemSO.colors[2];
        }
        if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Blue))
        {
            spriteRenderer.color = gridSystemSO.colors[3];
            trailRenderer.startColor = gridSystemSO.colors[3];
            trailRenderer.endColor = gridSystemSO.colors[3];
        }
        if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Orange))
        {
            spriteRenderer.color = gridSystemSO.colors[4];
            trailRenderer.startColor = gridSystemSO.colors[4];
            trailRenderer.endColor = gridSystemSO.colors[4];
        }
        if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Pink))
        {
            spriteRenderer.color = gridSystemSO.colors[5];
            trailRenderer.startColor = gridSystemSO.colors[5];
            trailRenderer.endColor = gridSystemSO.colors[5];
        }
       
       
    }
}
