using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBalloon : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sr;
    LineRenderer lr;
    float offsetY = 4f;
    float moveTime = 0.5f;
    float randomOffsetRadiusX = 2f;
    float randomOffsetRadiusY = 0.5f;
    float rotationTime = 0.75f;
    Vector3 rotationSmoothing;
    Vector3 randomOffset;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();
        randomOffset = new Vector3(Random.Range(-randomOffsetRadiusX, randomOffsetRadiusX), Random.Range(-randomOffsetRadiusY, randomOffsetRadiusY), 0f);
    }

    private void Update()
    {
        transform.up = Vector3.SmoothDamp(transform.up, transform.position - Player.Instance.transform.position, ref rotationSmoothing, rotationTime);

        Vector3 targetPos = Player.Instance.transform.position + offsetY * Vector3.up + randomOffset;
        float distance = Vector3.Distance(transform.position, targetPos);
        float t = (distance / 2f) * Time.deltaTime / moveTime;
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
    }

    private void LateUpdate()
    {
        
        lr.SetPosition(0, transform.position - transform.up * sr.sprite.bounds.size.y);
        lr.SetPosition(1, Player.Instance.transform.position);
    }

    public void AttachToPlayer()
    {
        Player.Instance.RegisterBalloon(this);
    }

    public void Pop(bool immediate)
    {
        Player.Instance.DeregisterBalloon(this);
        if (immediate) TriggerPopAnimation();
        else Invoke(nameof(TriggerPopAnimation), Random.Range(0f, 0.1f));
    }

    void TriggerPopAnimation()
    {
        AudioManager.Instance.PlayBalloonPop();
        animator.SetTrigger("Pop");
    }

    void EndPop()
    {
        Destroy(gameObject);
    }
}
