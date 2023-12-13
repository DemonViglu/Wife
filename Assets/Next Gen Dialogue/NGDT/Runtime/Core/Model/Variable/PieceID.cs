using System;
namespace Kurisu.NGDT
{
    [Serializable]
    public class PieceID : SharedVariable<string>
    {
        public PieceID()
        {
            IsShared = true;
        }

        public void Bind(PieceID other)
        {
            base.Bind(other);
        }

        public override object Clone()
        {
            return new PieceID() { Value = value, Name = Name, IsShared = IsShared, IsGlobal = IsGlobal };
        }
    }
}
