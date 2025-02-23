using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class EndlessMap : MonoBehaviour
{
    // 청크를 업데이트 이동 거리
    const float chunkUpdateThreshold = 5f;
    const float sqrChunkUpdateThreshold = chunkUpdateThreshold * chunkUpdateThreshold;

    // 블록을 업데이트 이동 거리
    const float sqrblockUpdateThreshold = 1f;

    const float blockCreateThreshold = Define.INTERACTION_RANGE;

    const int blockCreatePerLine = (int)(blockCreateThreshold * 2 + 1);
    const int cubeBlockCreate = blockCreatePerLine * blockCreatePerLine * blockCreatePerLine;

    const float maxViewDistance = 50f;

    static MapGenerator mapGenerator;
    public Transform player;
    public GameObject blockPrefab;
    public Transform blockPool;

    // 현재 뷰어 위치
    static Vector2 playerPositionCurrentForChunk;
    static Vector3 playerPositionCurrentForBlock;

    // 이전 뷰어 위치
    Vector2 playerPositionPastForChunk;
    Vector3 playerPositionPastForBlock;

    // 청크의 크기
    static int chunkSize;


    // 시야 거리 내에서 보이는 청크 수
    int chunkVisibleInViewDistance;

    // 청크 목록 저장
    Dictionary<Vector2, ChunkData> chunkDataDictionary = new Dictionary<Vector2, ChunkData>();

    // 보이는 청크 목록 저장
    static List<ChunkData> chunkDataVisibleLastUpdate = new List<ChunkData>();
    GameObject[] blocks;

    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();

        chunkSize = mapGenerator.mapData.chunkSize;
        chunkVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);
        playerPositionCurrentForBlock = new Vector3(player.position.x, player.position.y, player.position.z);

        CreateObjectPoolInBlocks();
        UpdateChunks();
        UpdateBlocks();
    }

    void CreateObjectPoolInBlocks()
    {
        blocks = new GameObject[cubeBlockCreate];

        for (int i = 0; i < cubeBlockCreate; i++)
        {
            GameObject newBlock = Instantiate(blockPrefab);
            blocks[i] = newBlock;
            newBlock.AddComponent<BoxCollider>();
            newBlock.AddComponent<Block>();
            newBlock.transform.parent = blockPool;
            newBlock.SetActive(false);
        }
    }

    void Update()
    {
        if (player != null)
        {
            playerPositionCurrentForChunk = new Vector2(player.position.x, player.position.z);
            playerPositionCurrentForBlock = new Vector3(player.position.x, player.position.y, player.position.z);

            if ((playerPositionPastForChunk - playerPositionCurrentForChunk).sqrMagnitude > sqrChunkUpdateThreshold)
            {
                playerPositionPastForChunk = playerPositionCurrentForChunk;

                UpdateChunks();
            }
            if ((playerPositionPastForBlock - playerPositionCurrentForBlock).sqrMagnitude > sqrblockUpdateThreshold)
            {
                playerPositionPastForBlock = playerPositionCurrentForBlock;

                UpdateBlocks();
            }
        }
    }

    void UpdateChunks()
    {
        for (int i = 0; i < chunkDataVisibleLastUpdate.Count; i++)
        {
            chunkDataVisibleLastUpdate[i].SetVisible(false);
        }

        chunkDataVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(playerPositionCurrentForChunk.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(playerPositionCurrentForChunk.y / chunkSize);

        for (int yOffset = -chunkVisibleInViewDistance; yOffset <= chunkVisibleInViewDistance; yOffset++)
        {
            for (int xOffset = -chunkVisibleInViewDistance; xOffset <= chunkVisibleInViewDistance; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (chunkDataDictionary.ContainsKey(viewedChunkCoord))
                {
                    chunkDataDictionary[viewedChunkCoord].UpdateChunk();
                }
                else
                {
                    chunkDataDictionary.Add(viewedChunkCoord, new ChunkData(this, viewedChunkCoord, chunkSize, transform));
                }

            }
        }
    }

    void UpdateBlocks()
    {
        HashSet<ChunkData> blockCreateThresholdPoints = new HashSet<ChunkData>();

        foreach (GameObject block in blocks)
        {
            block.SetActive(false);
        }

        Vector3Int nearestBlockPosition = MathUtility.CalculateRoundDownToUnitV3(playerPositionCurrentForBlock, 1);
        Vector3Int minusRangeV3 = nearestBlockPosition - (Vector3Int.one * (int)blockCreateThreshold);
        Vector3Int plusRangeV3 = nearestBlockPosition + (Vector3Int.one * (int)blockCreateThreshold);

        int a = MathUtility.CalculateRoundDownToUnit(minusRangeV3.x, chunkSize);
        int b = MathUtility.CalculateRoundDownToUnit(plusRangeV3.x, chunkSize);
        int c = MathUtility.CalculateRoundDownToUnit(minusRangeV3.z, chunkSize);
        int d = MathUtility.CalculateRoundDownToUnit(plusRangeV3.z, chunkSize);

        foreach (ChunkData chunk in chunkDataVisibleLastUpdate)
        {
            if (chunk.positionV2 == new Vector2(a, c) || chunk.positionV2 == new Vector2(a, d) ||
                chunk.positionV2 == new Vector2(b, c) || chunk.positionV2 == new Vector2(b, d))
            {
                blockCreateThresholdPoints.Add(chunk);
            }
        }

        foreach (ChunkData chunk in blockCreateThresholdPoints)
        {
            foreach (Vector3 key in chunk.blockData.Keys)
            {
                Vector3 globalBlockPosition = key + chunk.positionV3;

                if (minusRangeV3.x <= globalBlockPosition.x && globalBlockPosition.x <= plusRangeV3.x &&
                    minusRangeV3.y <= globalBlockPosition.y && globalBlockPosition.y <= plusRangeV3.y &&
                    minusRangeV3.z <= globalBlockPosition.z && globalBlockPosition.z <= plusRangeV3.z)
                {
                    foreach (GameObject block in blocks)
                    {
                        if (!block.activeSelf)
                        {
                            block.SetActive(true);

                            SetBlockLayer(block, chunk.blockData[key]);

                            block.GetComponent<Block>().SetBlock(chunk.blockData[key], chunk);
                            block.transform.position = globalBlockPosition + (Vector3.one * 0.5f);
                            break;
                        }
                    }
                }
            }
        }
    }

    void SetBlockLayer(GameObject block, string value)
    {
        if (value == "Block_Chest")
        {
            block.layer = LayerMask.NameToLayer("Chest");
        }
        else if (value == "Block_Craft")
        {
            block.layer = LayerMask.NameToLayer("Craft");
        }
        else if (value == "Block_Furnace")
        {
            block.layer = LayerMask.NameToLayer("Furnace");
        }
        else
        {
            block.layer = LayerMask.NameToLayer("Block");
        }
    }

    public class ChunkData
    {
        EndlessMap endlessMap;

        GameObject chunkObject;
        public Vector2 positionV2;
        public Vector3 positionV3;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        bool meshDataReceived;

        public NoiseData noiseData;
        public Dictionary<Vector3, string> blockData;
        public MeshData meshData;

        public ChunkData(EndlessMap endlessMap, Vector2 coord, int size, Transform parent)
        {
            this.endlessMap = endlessMap;

            positionV2 = coord * size;
            bounds = new Bounds(positionV2, Vector2.one * size);
            positionV3 = new Vector3(positionV2.x, 0, positionV2.y);

            chunkObject = new GameObject($"Chunk_{positionV3.x}_{positionV3.y}_{positionV3.z}");
            meshRenderer = chunkObject.AddComponent<MeshRenderer>();
            meshFilter = chunkObject.AddComponent<MeshFilter>();
            chunkObject.transform.position = positionV3;
            chunkObject.transform.parent = parent;

            SetVisible(false);

            mapGenerator.RequestNoiseData(positionV2, OnNoiseDataReceived);
        }

        void OnNoiseDataReceived(NoiseData noiseData)
        {
            this.noiseData = noiseData;

            mapGenerator.RequestBlockData(noiseData, OnBlockDataReceived);
        }

        void OnBlockDataReceived(Dictionary<Vector3, string> blockData)
        {
            this.blockData = blockData;

            mapGenerator.RequestMeshData(blockData, OnMeshDataReceived);
        }

        void OnMeshDataReceived(MeshData meshData)
        {
            this.meshData = meshData;

            meshData.CreateMesh();
            meshData.CreateMaterials();

            meshFilter.mesh = meshData.mesh;
            meshRenderer.materials = meshData.materials;

            meshDataReceived = true;

            UpdateChunk();
        }

        public bool FindChunkForBlock(Vector3 position, string name, Vector3 hitDirection)
        {
            Vector3 normalizePosition = position + hitDirection;

            foreach (ChunkData chunk in chunkDataVisibleLastUpdate)
            {
                if (chunk.positionV2.x <= normalizePosition.x && normalizePosition.x < chunk.positionV2.x + chunkSize &&
                    chunk.positionV2.y <= normalizePosition.z && normalizePosition.z < chunk.positionV2.y + chunkSize)
                {
                    chunk.AddBlock(name, normalizePosition);
                    return true;
                }
            }

            return false;
        }

        public void AddBlock(string name, Vector3 key)
        {
            blockData.Add(key - positionV3 - (Vector3.one * 0.5f), name);
            endlessMap.UpdateBlocks();

            mapGenerator.RequestMeshData(blockData, OnMeshDataReceived);
        }

        public void RemoveBlock(Vector3 key)
        {
            blockData.Remove(key - positionV3 - (Vector3.one * 0.5f));
            endlessMap.UpdateBlocks();

            mapGenerator.RequestMeshData(blockData, OnMeshDataReceived);
        }

        public void UpdateChunk()
        {
            if (meshDataReceived)
            {
                float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPositionCurrentForChunk));
                bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;

                if (visible)
                {
                    chunkDataVisibleLastUpdate.Add(this);
                }

                SetVisible(visible);
            }
        }

        public void SetVisible(bool visible)
        {
            chunkObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return chunkObject.activeSelf;
        }
    }
}