using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public string blockId { get; private set; }              // 블록 ID
    public Vector3 blockPosition { get; private set; }       // 블록 Position
    public bool blockVisible { get; private set; }         // 블록 Visible
    public List<Block> blockAdjacents; /*{ get; private set; }*/  // 블록 주위 블록
    public int currentHealth { get; private set; }          // 블록 현재 체력

    private string name;
    private int health;                               // 블록 최대 체력
    private string drop;                              // 드랍 아이템
    private Texture2D texture;                        // 블록 Texture

    private Renderer blockRenderer;                   // 블록 Renderer

    public void Initialization(string id, Vector3 position)
    {
        InitializationValue(id, position);
        InitializationInstance();
        InitializationComponent();
        InitializationGetBlockInfo();
        InitializationSetups();
    }

    private void InitializationValue(string id, Vector3 position)
    {
        blockId = id;
        blockPosition = position;
    }

    private void InitializationInstance()
    {
        blockAdjacents = new List<Block>();
    }

    private void InitializationComponent()
    {
        blockRenderer = GetComponent<Renderer>();
    }

    private void InitializationGetBlockInfo()
    {
        BlockInfo blockInfo = BlockManager.Instance.GetBlockInfoData(blockId);

        name = blockInfo.name;
        health = blockInfo.health;
        drop = blockInfo.drop;
        texture = Resources.Load<Texture2D>
            (Define.PATH_BLOCK_TEXTURE + name + "/" + blockInfo.texture);
    }

    private void InitializationSetups()
    {
        transform.name = name;
        currentHealth = health;
        blockRenderer.material.mainTexture = texture;
        blockVisible = false;
    }

    public void AddAdjacent(Block block)
    {
        blockAdjacents.Add(block);
    }

    public void RemoveAdjacent(Block block)
    {
        blockAdjacents.Remove(block);
    }

    public void SetHealth(int damage)
    {
        currentHealth -= damage;
        UpdateBlockState();
    }

    private void UpdateBlockState()
    {
        // 체력바로 표시하기
    }
}