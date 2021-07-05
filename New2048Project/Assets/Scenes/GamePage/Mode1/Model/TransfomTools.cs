using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfomTools : MonoBehaviour
{
    public static IEnumerator moveToPosition(Transform transform,Vector3 targetPosition,float moveSpeed)
    {
        float timeCounter = 0f;
        while (transform.position != targetPosition)
        {
            timeCounter += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (timeCounter >= 0.5f)
            {
                transform.position = targetPosition;
                break;
            }
            yield return 0;
        }
    }
    public static IEnumerator Scale(Transform transform, Vector3 targetScale, float scaleSpeed)
    {
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return 0;
        }
    }
}
