using UnityEngine;

public interface ILowerBodyState
{
    void Enter(ClientPlayer player);
    void Update(Controller controller);
}

public class IdleLowerState : ILowerBodyState
{
    public void Enter(ClientPlayer player)
    {
        player.lowerAnimator.SetBool("IsRun", false);
        player.lowerAnimator.SetBool("IsWalk", false);
    }

    public void Update(Controller controller)
    {
        if (controller.IsRunToggleKeyPressed() && controller.IsMoveToggleKeyPressed())
        {
            controller.SetLowerBodyState(new RunLowerState());
        }
        else if (controller.IsMoveToggleKeyPressed())
        {
            controller.SetLowerBodyState(new WalkLowerState());
        }
    }
}

public class WalkLowerState : ILowerBodyState
{
    public void Enter(ClientPlayer player)
    {
        player.lowerAnimator.SetBool("IsRun", false);
        player.lowerAnimator.SetBool("IsWalk", true);
    }

    public void Update(Controller controller)
    {
        if (controller.IsRunToggleKeyPressed())
        {
            controller.SetLowerBodyState(new RunLowerState());
        }
        else if (!controller.IsMoveToggleKeyPressed())
        {
            controller.SetLowerBodyState(new IdleLowerState());
        }
    }
}

public class RunLowerState : ILowerBodyState
{
    public void Enter(ClientPlayer player)
    {
        player.lowerAnimator.SetBool("IsRun", true);
        player.lowerAnimator.SetBool("IsWalk", true);
    }

    public void Update(Controller controller)
    {
        if (!controller.IsRunToggleKeyPressed())
        {
            controller.SetLowerBodyState(new WalkLowerState());
        }
        else if (!controller.IsMoveToggleKeyPressed())
        {
            controller.SetLowerBodyState(new IdleLowerState());
        }
    }
}
