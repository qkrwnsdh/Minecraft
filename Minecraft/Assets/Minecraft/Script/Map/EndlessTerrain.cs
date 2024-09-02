using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour
{
    // 플레이어가 이동한 후 지형을 업데이트하는 임계값(거리)
    const float viewerMoveThresholdForChunkUpdate = 25f;
    // 거리의 제곱 값으로 계산하여 사용함으로써 성능을 최적화
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    // LOD 정보를 담는 배열
    public LODInfo[] detailLevels;
    // 최대 시야 거리 (LOD 배열의 마지막 요소 기준)
    public static float maxViewDst;

    // 플레이어의 Transform 정보
    public Transform viewer;
    // 지형에 사용할 재질
    public Material mapMaterial;

    // 플레이어의 현재 위치 (2D 평면)
    public static Vector2 viewerPosition;
    // 플레이어의 이전 위치 (2D 평면)
    Vector2 viewerPositionOld;
    // 맵 생성기를 정적 변수로 선언하여 모든 지형 조각이 참조 가능하도록 설정
    static GenerateMap mapGenerator;
    // 각 지형 조각(Chunk)의 크기
    int chunkSize;
    // 시야 내에서 보이는 지형 조각의 수
    int chunksVisibleInViewDst;

    // 지형 조각을 좌표를 기준으로 관리하기 위한 Dictionary
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    // 마지막 업데이트 시점에서 시야 내에 있던 지형 조각들을 관리하는 리스트
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start()
    {
        // GenerateMap 클래스를 찾아서 mapGenerator에 할당
        mapGenerator = FindObjectOfType<GenerateMap>();

        // 시야 거리 계산 (LOD 배열의 마지막 요소의 visibleDstThreshold 값으로 설정)
        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        // 지형 조각의 크기 설정 (MAP_CHUNK_SIZE에서 1을 뺀 값)
        chunkSize = Define.MAP_CHUNK_SIZE - 1;
        // 시야 내에서 보이는 지형 조각의 수 계산
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize) + 1;

        // 초기 지형 조각 업데이트 호출
        UpdateVisibleChunks();
    }

    void Update()
    {
        // 플레이어의 현재 위치를 갱신
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        // 플레이어가 일정 거리 이상 이동했을 때 지형 조각 업데이트
        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    // 시야 내에서 보이는 지형 조각들을 업데이트하는 함수
    void UpdateVisibleChunks()
    {
        // 이전 프레임에서 시야에 있던 지형 조각들을 모두 비활성화
        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        // 현재 플레이어가 위치한 지형 조각의 좌표를 계산
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        // 시야 내 모든 지형 조각을 순회
        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                // 현재 시야 내의 지형 조각 좌표를 계산
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                // 해당 좌표의 지형 조각이 이미 존재하는 경우 업데이트
                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                }
                else
                {
                    // 해당 좌표의 지형 조각이 존재하지 않는 경우 새로 생성하여 Dictionary에 추가
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
                }

            }
        }
    }

    // 지형 조각을 나타내는 클래스
    public class TerrainChunk
    {
        // 지형 조각의 게임 오브젝트
        GameObject meshObject;
        // 지형 조각의 위치 (2D 평면에서의 좌표)
        Vector2 position;
        // 지형 조각의 경계 (Bounds)
        Bounds bounds;

        // 메쉬 렌더러와 메쉬 필터 컴포넌트
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        // LOD 관련 정보
        LODInfo[] detailLevels;
        // 각 LOD에 대응하는 메쉬 데이터를 저장
        LODMesh[] lodMeshes;

        // 생성된 맵 데이터
        MapData mapData;
        // 맵 데이터 수신 여부
        bool mapDataReceived;
        // 이전 프레임에서의 LOD 인덱스
        int previousLODIndex = -1;

        // TerrainChunk 생성자
        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material)
        {
            this.detailLevels = detailLevels;

            // 좌표를 기준으로 지형 조각의 위치와 경계 설정
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            // 지형 조각에 해당하는 게임 오브젝트 생성
            meshObject = new GameObject($"Chunk_{position.x},{position.y}");
            // 메쉬 렌더러와 메쉬 필터 컴포넌트를 추가
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            // 게임 오브젝트의 위치와 부모 객체, 스케일 설정
            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one;

            // 처음에는 비활성화
            SetVisible(false);

            // LOD 메쉬 초기화
            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }

            // 맵 데이터를 요청
            mapGenerator.RequestMapData(position, OnMapDataReceived);
        }

        // 맵 데이터를 수신했을 때 호출되는 콜백 함수
        void OnMapDataReceived(MapData mapData)
        {
            this.mapData = mapData;
            mapDataReceived = true;

            // 수신한 컬러 맵 데이터를 텍스처로 변환하여 메쉬 렌더러에 적용
            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, Define.MAP_CHUNK_SIZE, Define.MAP_CHUNK_SIZE);
            meshRenderer.material.mainTexture = texture;

            // 지형 조각 업데이트
            UpdateTerrainChunk();
        }

        // 지형 조각을 업데이트하는 함수
        public void UpdateTerrainChunk()
        {
            if (mapDataReceived)
            {
                // 플레이어와 가장 가까운 지형 조각의 경계에서의 거리 계산
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                // 지형 조각이 플레이어의 시야 내에 있는지 확인
                bool visible = viewerDstFromNearestEdge <= maxViewDst;

                if (visible)
                {
                    int lodIndex = 0;

                    // LOD 레벨을 결정
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // LOD 레벨이 이전 프레임과 다르면 메쉬를 갱신
                    if (lodIndex != previousLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }

                    // 현재 프레임에서 시야에 보이는 지형 조각으로 추가
                    terrainChunksVisibleLastUpdate.Add(this);
                }

                // 지형 조각을 활성화하거나 비활성화
                SetVisible(visible);
            }
        }

        // 지형 조각을 활성화하거나 비활성화하는 함수
        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        // 지형 조각의 활성화 상태를 반환하는 함수
        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }

    }

    // LOD 메쉬를 관리하는 클래스
    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;

        // LODMesh 생성자
        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        // 메쉬 데이터를 수신했을 때 호출되는 콜백 함수
        void OnMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallback();
        }

        // 메쉬 데이터를 요청하는 함수
        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
        }
    }

    // LOD 정보를 담는 구조체
    [System.Serializable]
    public struct LODInfo
    {
        public int lod; // LOD 레벨 (0이 가장 고해상도)
        public float visibleDstThreshold; // 해당 LOD 레벨의 지형 조각이 보이는 최대 거리
    }
}
