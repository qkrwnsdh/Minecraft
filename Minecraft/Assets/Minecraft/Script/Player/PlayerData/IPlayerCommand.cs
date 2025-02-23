using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public interface IPlayerCommand
{
    void Execute(Controller controller, string num = null);
}

public class MoveCommand : IPlayerCommand
{
    private readonly float speedRun = Define.SPEED_RUN;
    private readonly float speedWalk = Define.SPEED_WALK;

    public void Execute(Controller controller, string num = null)
    {
        ClientPlayer player = controller.GetPlayer;

        Transform playerTransform = controller.transform;
        Vector3 forward = playerTransform.forward;
        Vector3 right = playerTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = (forward * moveVertical) + (right * moveHorizontal);

        float speed = controller.IsRunToggleKeyPressed() ? speedRun : speedWalk;

        Vector3 velocity = movement.normalized * speed;

        playerTransform.position += velocity * Time.deltaTime;
    }
}

public class JumpCommand : IPlayerCommand
{
    private readonly float forceJump = Define.FORCE_JUMP;
    private readonly float checkRange = Define.GROUND_CHECK_RANGE;

    private bool isJumping = false;

    public void Execute(Controller controller, string num = null)
    {
        ClientPlayer player = controller.GetPlayer;
        Rigidbody rb = player.rb;

        IsCheckGroundContect(player.groundChecks);

        if (!isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector3.up * (forceJump * 0.5f), ForceMode.Impulse);
        }
    }

    bool IsCheckGroundContect(Transform[] transformChecks)
    {
        RaycastHit hit;

        foreach (Transform groundCheck in transformChecks)
        {
            if (Physics.Raycast(groundCheck.position, Vector2.down, out hit, checkRange))
            {
                if (hit.collider.CompareTag("Block"))
                {
                    isJumping = false;
                    return true;
                }
            }
        }

        return false;
    }
}

public class AttackCommand : IPlayerCommand
{
    private readonly float interactionRange = Define.INTERACTION_RANGE;

    public void Execute(Controller controller, string num = null)
    {
        ClientPlayer player = controller.GetPlayer;

        Vector3 position = player.playerCamera.position;
        Vector3 direction = player.playerCamera.forward;

        RaycastHit hit;

        if (Physics.Raycast(position, direction, out hit, interactionRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyInteraction(hit, controller);
            }
            else if (hit.collider.CompareTag("Block"))
            {
                BlockInteraction(hit, controller);
            }
        }
    }

    void EnemyInteraction(RaycastHit hit, Controller controller)
    {
        Enemy enemy = hit.collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.HitHealth(controller);
        }
    }

    void BlockInteraction(RaycastHit hit, Controller controller)
    {
        Block block = hit.collider.GetComponent<Block>();

        if (block != null)
        {
            block.HitHealth(controller);
        }
    }
}

public class InteractionCommand : IPlayerCommand
{
    private readonly float interactionRange = Define.INTERACTION_RANGE;

    private readonly int furnaceLayer = LayerMask.NameToLayer("Furnace");
    private readonly int craftLayer = LayerMask.NameToLayer("Craft");
    private readonly int chestLayer = LayerMask.NameToLayer("Chest");

    public void Execute(Controller controller, string num = null)
    {
        ClientPlayer player = controller.GetPlayer;

        Vector3 position = player.playerCamera.position;
        Vector3 direction = player.playerCamera.forward;

        RaycastHit hit;

        if (Physics.Raycast(position, direction, out hit, interactionRange))
        {
            switch (hit.collider.gameObject.layer)
            {
                case int layer when layer == furnaceLayer:
                    FurnaceInteraction(hit, controller);
                    break;
                case int layer when layer == craftLayer:
                    CraftInteraction(hit, controller);
                    break;
                case int layer when layer == chestLayer:
                    ChestInteraction(hit, controller);
                    break;
            }
        }
    }

    void FurnaceInteraction(RaycastHit hit, Controller controller)
    {
        Furnace furnace = hit.collider.GetComponent<Furnace>();

        if (furnace != null)
        {
            controller.GetUi.ToggleManager(Window.FURNACE, furnace);
        }
    }

    void CraftInteraction(RaycastHit hit, Controller controller)
    {
        Craft craft = hit.collider.GetComponent<Craft>();

        if (craft != null)
        {
            controller.GetUi.ToggleManager(Window.CRAFT, craft);
        }
    }

    void ChestInteraction(RaycastHit hit, Controller controller)
    {
        Chest chest = hit.collider.GetComponent<Chest>();

        if (chest != null)
        {
            controller.GetUi.ToggleManager(Window.CHEST, chest);
        }
    }
}

public class EscapeCommand : IPlayerCommand
{
    public void Execute(Controller controller, string num = null)
    {
        controller.GetUi.ToggleManager(Window.SETTING);
    }
}

public class InventoryCommand : IPlayerCommand
{
    public void Execute(Controller controller, string num = null)
    {
        controller.GetUi.ToggleManager(Window.INVENTORY, controller.GetInventory);
    }
}

public class QuickCommand : IPlayerCommand
{
    private readonly float interactionRange = Define.INTERACTION_RANGE;

    public void Execute(Controller controller, string num = null)
    {
        ClientPlayer player = controller.GetPlayer;

        Vector3 position = player.playerCamera.position;
        Vector3 direction = player.playerCamera.forward;

        RaycastHit hit;

        if (Physics.Raycast(position, direction, out hit, interactionRange))
        {
            if (hit.collider.CompareTag("Block"))
            {
                Block block = hit.collider.GetComponent<Block>();
                Vector3 hitDirection = IdentifyHitFace(hit);

                controller.GetUi.ToggleItem(controller.GetPlayer, num, block, hitDirection);
            }
        }
        else
        {
            controller.GetUi.ToggleItem(controller.GetPlayer, num);
        }

    }

    Vector3 IdentifyHitFace(RaycastHit hit)
    {
        Vector3 hitNormal = hit.normal;

        if (Mathf.Abs(hitNormal.y) > Mathf.Abs(hitNormal.x) && Mathf.Abs(hitNormal.y) > Mathf.Abs(hitNormal.z))
        {
            // Y 값이 가장 큰 경우 윗면 또는 아랫면
            if (hitNormal.y > 0)
            {
                return Vector3.up;
            }
            else
            {
                return Vector3.down;
            }
        }
        else if (Mathf.Abs(hitNormal.x) > Mathf.Abs(hitNormal.y) && Mathf.Abs(hitNormal.x) > Mathf.Abs(hitNormal.z))
        {
            // X 값이 가장 큰 경우 왼쪽면 또는 오른쪽면
            if (hitNormal.x > 0)
            {
                return Vector3.right;
            }
            else
            {
                return Vector3.left;
            }
        }
        else
        {
            // Z 값이 가장 큰 경우 앞면 또는 뒷면
            if (hitNormal.z > 0)
            {
                return Vector3.forward;
            }
            else
            {
                return Vector3.back;
            }
        }
    }
}

public class CameraCommand : IPlayerCommand
{
    private readonly float sensitivity = Define.MOUSE_SENSITIVITY;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public void Execute(Controller controller, string num = null)
    {
        ClientPlayer player = controller.GetPlayer;

        // 마우스 이동 입력
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        // 카메라의 회전
        player.playerCamera.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // 플레이어 몸체의 회전
        player.playerHead.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}