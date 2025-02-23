using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockInfo blockInfo;
    public EndlessMap.ChunkData chunkData;

    public int currentHealth;
    private Coroutine hitCoroutine;

    #region Interaction
    public void SetBlock(string name, EndlessMap.ChunkData chunkData)
    {
        this.name = name;
        this.chunkData = chunkData;

        blockInfo = BlockManager.Instance.GetBlockInfoData(name);
        currentHealth = blockInfo.health;
    }

    public void HitHealth(Controller controller)
    {
        ClientPlayer player = controller.GetPlayer;

        // 피해 간격
        if (hitCoroutine != null) { return; }

        // 피해 감소
        currentHealth -= player.Damage;
        UpdatePlayerInteraction(controller);

        // 체력이 0 이하가 되면 블록 무효화
        if (currentHealth <= 0)
        {
            UpdatePlayerInventory(controller);
            InActive();
        }
        // 피해 간격 코루틴 시작
        else
        {
            hitCoroutine = StartCoroutine(HitCoroutine());
        }
    }

    public bool CreateBlock(string name, Vector3? hitDirection)
    {
        return chunkData.FindChunkForBlock(transform.position, name, (Vector3)hitDirection);
    }

    void UpdatePlayerInteraction(Controller controller) => controller.GetUi.ToggleInteraction(this);
    void UpdatePlayerInventory(Controller controller)
    {
        controller.GetUi.data.AddItem(blockInfo.drop);
        controller.GetUi.UpdateQuickSlot();
    }
    void InActive()
    {
        hitCoroutine = null;
        gameObject.SetActive(false);
        chunkData.RemoveBlock(transform.position);
    }

    IEnumerator HitCoroutine()
    {
        // 피해 간격 대기
        yield return new WaitForSeconds(Define.HIT_INTERVAL);

        hitCoroutine = null;
    }
    #endregion
}