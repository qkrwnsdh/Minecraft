public interface IUpperBodyState
{
    void Enter(Player player);
    void Update(Player player);
}

public class IdleUpperState : IUpperBodyState
{
    public void Enter(Player player)
    {
        player.upperAnimator.SetBool("IsAttack", false);
        player.upperAnimator.SetBool("IsWalk", false);
        player.upperAnimator.SetBool("IsRun", false);
    }

    public void Update(Player player)
    {
        // 공격 상태로 전환
        if (player.IsLeftMouseKeyPressed())
        {
            player.SetUpperBodyState(new AttackUpperState());
        }
        // 달리기 상태로 전환
        else if (player.IsLeftShiftKeyPressed() && player.IsMovementKeyPressed())
        {
            player.SetUpperBodyState(new RunUpperState());
        }
        // 걷기 상태로 전환
        else if (player.IsMovementKeyPressed())
        {
            player.SetUpperBodyState(new WalkUpperState());
        }
    }
}

public class AttackUpperState : IUpperBodyState
{
    public void Enter(Player player)
    {
        player.upperAnimator.SetBool("IsAttack", true);
    }

    public void Update(Player player)
    {
        // 공격키를 떼면 상태 전환
        if (!player.IsLeftMouseKeyPressed())
        {
            player.upperAnimator.SetBool("IsAttack", false);

            // 달리기 상태로 전환
            if (player.IsLeftShiftKeyPressed() && player.IsMovementKeyPressed())
            {
                player.SetUpperBodyState(new RunUpperState());
            }
            // 걷기 상태로 전환
            else if (player.IsMovementKeyPressed())
            {
                player.SetUpperBodyState(new WalkUpperState());
            }
            // 기본 상태로 전환
            else
            {
                player.SetUpperBodyState(new IdleUpperState());
            }
        }
    }
}

public class WalkUpperState : IUpperBodyState
{
    public void Enter(Player player)
    {
        player.upperAnimator.SetBool("IsWalk", true);
    }

    public void Update(Player player)
    {
        // 공격 상태로 전환
        if (player.IsLeftMouseKeyPressed())
        {
            player.SetUpperBodyState(new AttackUpperState());
        }
        // 달리기 상태로 전환
        else if (player.IsLeftShiftKeyPressed())
        {
            player.SetUpperBodyState(new RunUpperState());
        }
        // 기본 상태로 전환
        else if (!player.IsMovementKeyPressed())
        {
            player.upperAnimator.SetBool("IsWalk", false);

            player.SetUpperBodyState(new IdleUpperState());
        }
    }
}

public class RunUpperState : IUpperBodyState
{
    public void Enter(Player player)
    {
        player.upperAnimator.SetBool("IsWalk", true);
        player.upperAnimator.SetBool("IsRun", true);
    }

    public void Update(Player player)
    {
        // 공격 상태로 전환
        if (player.IsLeftMouseKeyPressed())
        {
            player.SetUpperBodyState(new AttackUpperState());
        }
        // 걷기 상태로 전환
        else if (!player.IsLeftShiftKeyPressed())
        {
            player.upperAnimator.SetBool("IsRun", false);

            player.SetUpperBodyState(new WalkUpperState());
        }
        // 기본 상태로 전환
        else if (!player.IsMovementKeyPressed())
        {
            player.upperAnimator.SetBool("IsWalk", false);
            player.upperAnimator.SetBool("IsRun", false);

            player.SetUpperBodyState(new IdleUpperState());
        }
    }
}