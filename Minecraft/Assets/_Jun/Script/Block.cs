using UnityEngine;

public class Block : BlockMain
{
    private BlockInfo blockInfo;

    protected override void Start()
    {
        string id = transform.name.Substring(0, 8);

        blockInfo = BlockData.Instance.GetBlock(id);
        blockRenderer = GetComponent<Renderer>();

        blockRenderer.material.mainTexture = Resources.Load<Texture2D>
                (Define.PATH_BLOCK_TEXTURE + blockInfo.name + "/" + blockInfo.texture);

        base.Start();
    }
}