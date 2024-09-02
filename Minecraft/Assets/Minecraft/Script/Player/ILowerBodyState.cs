public interface ILowerBodyState
{
    void Enter(Player player);
    void Update(Player player);
}

public class IdleLowerState : ILowerBodyState
{
    public void Enter(Player player)
    {
        player.lowerAnimator.SetBool("IsWalk", false);
        player.lowerAnimator.SetBool("IsRun", false);
    }

    public void Update(Player player)
    {
        // 달리기 상태로 전환
        if (player.IsLeftShiftKeyPressed() && player.IsMovementKeyPressed())
        {
            player.SetLowerBodyState(new RunLowerState());
        }
        // 걷기 상태로 전환
        else if (player.IsMovementKeyPressed())
        {
            player.SetLowerBodyState(new WalkLowerState());
        }
    }
}

public class WalkLowerState : ILowerBodyState
{
    public void Enter(Player player)
    {
        player.lowerAnimator.SetBool("IsWalk", true);
    }

    public void Update(Player player)
    {
        // 달리기 상태로 전환
        if (player.IsLeftShiftKeyPressed())
        {
            player.SetLowerBodyState(new RunLowerState());
        }
        // 기본 상태로 전환
        else if (!player.IsMovementKeyPressed())
        {
            player.lowerAnimator.SetBool("IsWalk", false);

            player.SetLowerBodyState(new IdleLowerState());
        }
    }
}

public class RunLowerState : ILowerBodyState
{
    public void Enter(Player player)
    {
        player.lowerAnimator.SetBool("IsWalk", true);
        player.lowerAnimator.SetBool("IsRun", true);
    }

    public void Update(Player player)
    {
        // 걷기 상태로 전환
        if (!player.IsLeftShiftKeyPressed())
        {
            player.lowerAnimator.SetBool("IsRun", false);

            player.SetLowerBodyState(new WalkLowerState());
        }
        // 기본 상태로 전환
        else if (!player.IsMovementKeyPressed())
        {
            player.lowerAnimator.SetBool("IsWalk", false);
            player.lowerAnimator.SetBool("IsRun", false);

            player.SetLowerBodyState(new IdleLowerState());
        }
    }
}
