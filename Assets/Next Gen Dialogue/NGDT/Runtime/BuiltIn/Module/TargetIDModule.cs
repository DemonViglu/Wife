using Kurisu.NGDS;
using UnityEngine;
namespace Kurisu.NGDT
{
    [AkiInfo("Module : TargetID Module is used to definite option's target dialogue piece id.")]
    [ModuleOf(typeof(Option))]
    public class TargetIDModule : CustomModule
    {
#if UNITY_EDITOR
        [SerializeField]
        private bool useReference;
#endif
        [SerializeField, AkiLabel("Target ID"), Tooltip("The target dialogue piece's PieceID"), ReferencePieceID]
        private PieceID targetID;
        public PieceID TargetID
        {
            get => targetID;
            set => targetID = value;
        }
        public override void Awake()
        {
            InitVariable(targetID);
        }
        protected sealed override IDialogueModule GetModule()
        {
            return new NGDS.TargetIDModule(targetID.Value);
        }
    }
}