using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
namespace Kurisu.NGDT.Editor
{
    public interface IDialogueTreeView : IVariableSource
    {
        EditorWindow EditorWindow { get; }
        GraphView View { get; }
        IControlGroupBlock GroupBlockController { get; }
        IDialogueNode DuplicateNode(IDialogueNode node);
        ContextualMenuController ContextualMenuController { get; }
        IBlackBoard BlackBoard { get; }
        Action<IDialogueNode> OnSelectAction { get; }
        void BakeDialogue();
    }
}
