using UnityEngine;

public static class GenerateNoiseMap
{
    // 노이즈 맵 생성
    public static float[,] Generate(int mapScale, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        // 생성할 노이즈 맵을 저장할 2차원 배열을 생성
        float[,] noiseMap = new float[mapScale, mapScale];

        // 랜덤 시드를 사용하여 난수 생성기(prng)를 초기화
        System.Random prng = new System.Random(seed);

        // 각 옥타브의 오프셋을 저장할 배열을 생성
        Vector2[] octaveOffsets = new Vector2[octaves];

        // 최대 높이를 계산하기 위한 변수
        float maxHeight = 0;
        // 옥타브의 진폭을 초기화
        float amplitude = 1;
        // 옥타브의 주파수를 초기화
        float frequency = 1;

        // 옥타브 수만큼 반복하여 각 옥타브의 오프셋을 설정
        for (int i = 0; i < octaves; i++)
        {
            // 무작위로 오프셋을 생성하고, 입력받은 오프셋 추가
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            // 최대 높이를 계산하기 위해 진폭을 추가
            maxHeight += amplitude;
            // 진폭을 업데이트
            amplitude *= persistance;
        }

        // 노이즈의 최대 및 최소 높이를 초기화합니다.
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // 맵의 중심을 계산하여 샘플링 오프셋을 정렬합니다.
        float halfWidth = mapScale / 2f;
        float halfHeight = mapScale / 2f;

        // 맵의 각 좌표에 대해 노이즈 값을 계산합니다.
        for (int y = 0; y < mapScale; y++)
        {
            for (int x = 0; x < mapScale; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                // 각 옥타브에 대해 노이즈 값을 계산합니다.
                for (int i = 0; i < octaves; i++)
                {
                    // 현재 좌표와 오프셋을 사용하여 샘플링 좌표를 계산합니다.
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                    // Perlin 노이즈 값을 계산하고 -1과 1 사이의 범위로 조정합니다.
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    // 계산된 노이즈 값을 높이에 추가합니다.
                    noiseHeight += perlinValue * amplitude;

                    // 진폭과 주파수를 업데이트합니다.
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // 현재 높이 값을 기준으로 최대 및 최소 높이를 업데이트합니다.
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                // 계산된 높이 값을 노이즈 맵에 저장합니다.
                noiseMap[x, y] = noiseHeight;
            }
        }

        // 노이즈 값을 0과 1 사이로 정규화합니다.
        for (int y = 0; y < mapScale; y++)
        {
            for (int x = 0; x < mapScale; x++)
            {
                // 노이즈 값을 정규화하여 [0, 1] 범위로 조정합니다.
                float normalizedHeight = (noiseMap[x, y] + 1) / maxHeight;
                // 정규화된 값을 클램프하여 [0, 1] 범위로 제한합니다.
                noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
            }
        }

        // 생성된 노이즈 맵을 반환합니다.
        return noiseMap;
    }
}