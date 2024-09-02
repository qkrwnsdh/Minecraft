using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int currentHealth;
    private Coroutine hitCoroutine;

    public void HitHealth(Player player, int damage)
    {
        // 피해 간격
        if (hitCoroutine != null) { return; }

        // 피해 감소
        currentHealth -= damage;
        UpdateHealth(player);

        // 체력이 0 이하가 되면 블록 무효화
        if (currentHealth <= 0)
        {
            InvalidBlock(player);
        }
        // 피해 간격 코루틴 시작
        else
        {
            hitCoroutine = StartCoroutine(HitCoroutine(damage));
        }
    }

    void UpdateHealth(Player player)
    {
        // 체력바로 표시하기
    }

    void InvalidBlock(Player player)
    {

    }

    IEnumerator HitCoroutine(int damage)
    {
        // 피해 간격 대기
        yield return new WaitForSeconds(Define.HIT_INTERVAL);

        hitCoroutine = null;
    }
}
