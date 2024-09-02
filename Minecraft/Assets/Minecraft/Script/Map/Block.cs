using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockData data;

    private MeshRenderer render;
    private Material[] materials;

    private int currentHealth;
    private Coroutine hitCoroutine;

    #region Initialization
    private void Start()
    {
        InitializationComponents();
        InitializationSetups();
    }

    private void InitializationComponents()
    {
        render = GetComponent<MeshRenderer>();
    }

    private void InitializationSetups()
    {
        data = BlockManager.Instance.blockDatas[name];
        materials = render.materials;
        currentHealth = data.health;
    }
    #endregion

    #region Interaction
    public void SetOffset(int value)
    {
        if (data.offsets.Count < value + 1)
        {
            Debug.LogError($"{name} is not found offsets {value}");

            return;
        }

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetTextureOffset("_MainTex", data.offsets[value].offset[i]);
        }
    }

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
    #endregion
}