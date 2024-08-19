using UnityEngine;

public interface IPlayerCommand
{
    void Execute(Player player);
}

public class MoveCommand : IPlayerCommand
{
    public void Execute(Player player)
    {
        Rigidbody rigidbody = player.rigidbody; 
        Transform cameraTransform = player.playerCamera;

        // 카메라의 좌우 방향과 앞뒤 방향을 기준으로 이동 벡터를 계산
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // y축의 컴포넌트를 0으로 설정하여 수평 방향만 고려
        forward.y = 0f;
        right.y = 0f;

        // 정규화하여 방향 벡터의 크기를 1로 설정
        forward.Normalize();
        right.Normalize();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 이동 벡터를 카메라의 방향에 맞춰 조정
        Vector3 movement = (forward * moveVertical) + (right * moveHorizontal);

        // 이동 속도 설정
        float speed = Input.GetKey(KeyCode.LeftShift) ? player.forceRun : player.forceWalk;
        // 이동 벡터를 속도에 맞게 조정
        Vector3 velocity = movement.normalized * speed;

        // 현재 속도에 이동 벡터를 적용
        rigidbody.velocity = new Vector3(velocity.x, rigidbody.velocity.y, velocity.z);
    }
}

public class JumpCommand : IPlayerCommand
{
    public void Execute(Player player)
    {
        Rigidbody rigidbody = player.rigidbody;

        rigidbody.AddForce(Vector3.up * player.forceJump, ForceMode.Impulse);
    }
}