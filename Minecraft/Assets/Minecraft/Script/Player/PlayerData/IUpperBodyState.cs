using UnityEngine;

public interface IUpperBodyState
{
    void Enter(ClientPlayer player);
    void Update(Controller controller);
}

public class IdleUpperState : IUpperBodyState
{
    public void Enter(ClientPlayer player)
    {
        player.upperAnimator.SetBool("IsAttack", false);
        player.upperAnimator.SetBool("IsRun", false);
        player.upperAnimator.SetBool("IsWalk", false);
    }

    public void Update(Controller controller)
    {
        if (controller.IsAttackToggleKeyPressed())
        {
            controller.SetUpperBodyState(new AttackUpperState());
        }
        else if (controller.IsRunToggleKeyPressed() && controller.IsMoveToggleKeyPressed())
        {
            controller.SetUpperBodyState(new RunUpperState());
        }
        else if (controller.IsMoveToggleKeyPressed())
        {
            controller.SetUpperBodyState(new WalkUpperState());
        }
    }
}

public class AttackUpperState : IUpperBodyState
{
    public void Enter(ClientPlayer player)
    {
        player.upperAnimator.SetBool("IsAttack", true);
    }

    public void Update(Controller controller)
    {
        if (!controller.IsAttackToggleKeyPressed() && controller.IsRunToggleKeyPressed() && controller.IsMoveToggleKeyPressed())
        {
            controller.SetUpperBodyState(new RunUpperState());
        }
        else if (!controller.IsAttackToggleKeyPressed() && controller.IsMoveToggleKeyPressed())
        {
            controller.SetUpperBodyState(new WalkUpperState());
        }
        else if (!controller.IsAttackToggleKeyPressed())
        {
            controller.SetUpperBodyState(new IdleUpperState());
        }
    }
}

public class WalkUpperState : IUpperBodyState
{
    public void Enter(ClientPlayer player)
    {
        player.upperAnimator.SetBool("IsAttack", false);
        player.upperAnimator.SetBool("IsRun", false);
        player.upperAnimator.SetBool("IsWalk", true);
    }

    public void Update(Controller controller)
    {
        if (controller.IsAttackToggleKeyPressed())
        {
            controller.SetUpperBodyState(new AttackUpperState());
        }
        else if (controller.IsRunToggleKeyPressed())
        {
            controller.SetUpperBodyState(new RunUpperState());
        }
        else if (!controller.IsMoveToggleKeyPressed())
        {
            controller.SetUpperBodyState(new IdleUpperState());
        }
    }
}

public class RunUpperState : IUpperBodyState
{
    public void Enter(ClientPlayer player)
    {
        player.upperAnimator.SetBool("IsAttack", false);
        player.upperAnimator.SetBool("IsRun", true);
        player.upperAnimator.SetBool("IsWalk", true);
    }

    public void Update(Controller controller)
    {
        if (controller.IsAttackToggleKeyPressed())
        {
            controller.SetUpperBodyState(new AttackUpperState());
        }
        else if (!controller.IsRunToggleKeyPressed())
        {
            controller.SetUpperBodyState(new WalkUpperState());
        }
        else if (!controller.IsMoveToggleKeyPressed())
        {
            controller.SetUpperBodyState(new IdleUpperState());
        }
    }
}