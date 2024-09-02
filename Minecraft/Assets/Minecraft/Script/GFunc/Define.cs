public static class Define
{
    public const string PATH_BLOCK_INFO = "Data/BlockInfo";
    public const string PATH_BLOCK_OFFSET = "Data/BlockOffset";
    public const string PATH_BLOCK_TEXTURE = "Texture/Block/";
    public const string PATH_BLOCK_PREFAB = "Prefabs/Block";

    public const float BIOME_DESERT_AMPLITUDE = 5f;
    public const float BIOME_DESERT_VIORATION = 5f;

    public const float BIOME_OCEAN_AMPLITUDE = 5f;
    public const float BIOME_OCEAN_VIORATION = 5f;

    public const float BIOME_VELD_AMPLITUDE = 5f;
    public const float BIOME_VELD_VIORATION = 5f;

    public const int BIOME_SMOOTH_MAX = 5;
    public const int BIOME_SMOOTH_MIN = 3;

    public const int REGION_IN_BLOCK_DIM = 20;
    public const int REGION_DIM = 2;

    #region Player Setting
    // 마우스 민감도
    public const float MOUSE_SENSITIVITY = 2.0f;
    // 지면 체크를 위한 레이 길이
    public const float GROUND_CHECK_RANGE = 0.1f;
    // 상호작용 레이 길이
    public const float INTERACTION_RANGE = 5.0f;
    // 걷기 속도
    public const float SPEED_WALK = 5.0f;
    // 달리기 속도
    public const float SPEED_RUN = 7.0f;
    // 점프 파워
    public const float FORCE_JUMP = 5.0f;
    #endregion

    public const float HIT_INTERVAL = 1.0f;

    #region Map Setting
    public const int MAP_CHUNK_SIZE = 51;
    public const int MAP_OCTAVES = 5;
    public const int MAP_NOISE_SCALE = 100;
    public const float MAP_PERSISTANCE = 0.5f;
    public const float MAP_LACUNARITY = 2.0f;
    #endregion
}