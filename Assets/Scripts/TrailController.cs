using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrailController : MonoBehaviour
{
    GridManager gridManager;
    void Start()
    {
        gridManager = ObjectManager.GridManager;
        GetComponent<SpriteRenderer>().color = Color.red;
        GetComponent<TrailRenderer>().startColor = Color.red;
        GetComponent<TrailRenderer>().endColor = Color.red;
    }

    
    void Update()
    {
        
    }

    public void Movement(GameObject target)
    {
        transform.DOMove(Camera.main.ScreenToWorldPoint(target.transform.position), 15).SetEase(Ease.Linear).SetSpeedBased(true).OnStepComplete(() => EndMovement(target));

    }

    private void EndMovement(GameObject target)
    {
        target.GetComponent<GoalController>().SetText(1);
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        gridManager.trailQue.Enqueue(transform.gameObject);
    }
}
