using GameMessage;
using SuperBilliard;
using GameFramework.Network;

public class SCRigisterHandler : PacketHandlerBase
{
    public override int Id => SuperBilliard.Constant.PacketTypeId.Rigister;

    public override void Handle(object sender, Packet packet)
    {
        SCRigister sCRigister = packet as SCRigister;
        switch (sCRigister.Result)
        {
            case ReturnResult.Success:
                SuperBilliard.GameEntry.Event.Fire(this, RigisterSuccessEventArgs.Create());
                break;
            case ReturnResult.Failure:
                SuperBilliard.GameEntry.Event.Fire(this, RigisterFailureEventArgs.Create());
                break;
        }
    }
}
