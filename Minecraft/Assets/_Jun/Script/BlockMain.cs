using UnityEngine;

public class BlockMain : MonoBehaviour
{
    private int currentHealth;

    protected int maxHealth;
    protected Texture2D[] blockTextures;
    protected Renderer blockRenderer;

    protected virtual void Start()
    {
        InitializationSetups();
    }

    private void InitializationSetups()
    {
        currentHealth = maxHealth;

        UpdateBlockState();
    }

    public void SetHealth(int damage)
    {
        currentHealth -= damage;
        UpdateBlockState();
    }

    private void UpdateBlockState()
    {
        // ü�¹ٷ� ǥ���ϱ�
    }
}
