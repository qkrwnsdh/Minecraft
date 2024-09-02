using UnityEngine;

public interface IPlayerCommand
{
    void Execute(Player player);
}

public class VisionCommand : IPlayerCommand
{
    private float sensitivity;
    private float xRotation = 0f;
    private float yRotation = 0f;

    public VisionCommand(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }

    public void Execute(Player player)
    {
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

public class MoveCommand : IPlayerCommand
{
    private Rigidbody rigidbody;
    private Transform playerTransform;
    private float speedWalk;
    private float speedRun;

    public MoveCommand(Rigidbody rigidbody, Transform playerTransform, float speedWalk, float speedRun)
    {
        this.rigidbody = rigidbody;
        this.playerTransform = playerTransform;
        this.speedWalk = speedWalk;
        this.speedRun = speedRun;
    }

    public void Execute(Player player)
    {
        Vector3 forward = playerTransform.forward;
        Vector3 right = playerTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = (forward * moveVertical) + (right * moveHorizontal);

        float speed = player.IsLeftShiftKeyPressed() ? speedRun : speedWalk;

        Vector3 velocity = movement.normalized * speed;

        rigidbody.velocity = new Vector3(velocity.x, rigidbody.velocity.y, velocity.z);
    }
}

public class JumpCommand : IPlayerCommand
{
    private float forceJump;
    private Rigidbody rigidbody;
    private Transform[] groundChecks;
    private float checkLength;
    private int blockLayer;

    public JumpCommand(float forceJump, Rigidbody rigidbody, Transform[] groundChecks, float checkLength, int blockLayer)
    {
        this.forceJump = forceJump;
        this.rigidbody = rigidbody;
        this.groundChecks = groundChecks;
        this.checkLength = checkLength;
        this.blockLayer = blockLayer;
    }

    public void Execute(Player player)
    {
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics.Raycast(groundCheck.position, Vector3.down, checkLength, blockLayer))
            {
                rigidbody.AddForce(Vector3.up * forceJump, ForceMode.Impulse);
                break;
            }
        }
    }
}

public class AttackCommand : IPlayerCommand
{
    private float interactionRange;
    Transform cameraTransform;

    public AttackCommand(float interactionRange, Transform cameraTransform)
    {
        this.interactionRange = interactionRange;
        this.cameraTransform = cameraTransform;
    }

    public void Execute(Player player)
    {
        int damage = player.damage;

        Vector3 direction = cameraTransform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, direction, out hit, interactionRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.HitHealth(player, damage);
                }
            }
            else if (hit.collider.CompareTag("Block"))
            {
                Block block = hit.collider.GetComponent<Block>();
                if (block != null)
                {
                    block.HitHealth(player, damage);
                }
            }
        }
    }
}

public class InteractionCommand : IPlayerCommand
{
    private float interactionRange;
    Transform cameraTransform;

    public InteractionCommand(float interactionRange, Transform cameraTransform)
    {
        this.interactionRange = interactionRange;
        this.cameraTransform = cameraTransform;
    }

    public void Execute(Player player)
    {
        Vector3 direction = cameraTransform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, direction, out hit, interactionRange))
        {
            // 상호작용 로직
        }
    }
}