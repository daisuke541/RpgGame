using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] float moveSpeed;

    bool isMoving;
    Vector2 input;

    Animator animator;

    //壁判定
    [SerializeField] LayerMask solidObjects;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 動いていない時
        if (!isMoving)
        {
            // キーボード入力を受け付ける
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // 斜め移動禁止:横方向の入力があれば, 縦は0にする
            if (input.x != 0)
            {
                input.y = 0;
            }
            // 入力があったら
            if (input != Vector2.zero)
            {
                //向きを変える
                animator.SetFloat("moveX",input.x);
                animator.SetFloat("moveY",input.y);
                // 入力分を追加
                Vector2 targetPos = transform.position;
                targetPos += input;
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        // targetPosと現在のpisitionの差がある間は、MoveTowardsでtargetPosに近く
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    //targetPosに移動可能か調べる
    bool IsWalkable(Vector2 targetPos)
    {
        bool hit = Physics2D.OverlapCircle(targetPos, 0.2f, solidObjects);
        return !hit;
    }

}