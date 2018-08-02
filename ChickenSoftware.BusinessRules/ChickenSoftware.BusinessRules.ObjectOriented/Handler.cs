using System;
namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public abstract class Handler
    {
        public abstract void Process();
        public abstract void SetSuccessor(Handler handler);
    }
}
