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

    public void Movement(GameObject target, NumbersController number)
    {
        SetColor(target,number);
        transform.DOMove(Camera.main.ScreenToWorldPoint(target.transform.position), 15).SetEase(Ease.Linear).SetSpeedBased(true).OnStepComplete(() => EndMovement(target));
    }

    private void EndMovement(GameObject target)
    {
        target.GetComponent<GoalController>().SetText(1);
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        gridManager.trailQue.Enqueue(transform.gameObject);
    }

    private void SetColor(GameObject target,NumbersController number)
    {
        spriteRenderer.color = gridSystemSO.colors[number.colorValue];
        trailRenderer.startColor = gridSystemSO.colors[number.colorValue];
        trailRenderer.endColor = gridSystemSO.colors[number.colorValue];
    }
}
