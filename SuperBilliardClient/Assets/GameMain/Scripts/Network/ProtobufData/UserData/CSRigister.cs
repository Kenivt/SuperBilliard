using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class CSRigister : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.Rigister;
        public static CSRigister Create(string username, string password)
        {
            CSRigister cSRigister = ReferencePool.Acquire<CSRigister>();
            cSRigister.Username = username;
            cSRigister.Password = password;
            return cSRigister;
        }
        public override void Clear()
        {
            username_ = string.Empty;
            password_ = string.Empty;
        }
    }
    public partial class SCRigister : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.Rigister;

        public override void Clear()
        {
            result_ = ReturnResult.None;
        }
        public static SCRigister Create(string username, string password)
        {
            SCRigister cSRigister = ReferencePool.Acquire<SCRigister>();
            cSRigister.Result = ReturnResult.Success;
            return cSRigister;
        }
    }
}
