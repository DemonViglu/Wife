using System.Collections;
using System.Threading;
using Kurisu.NGDS.AI;
using UnityEngine;
namespace Kurisu.NGDS
{
    public readonly struct AIGenerateModule : IDialogueModule, IProcessable
    {
        private static readonly CancellationTokenSource token = new();
        private readonly string characterName;
        public string CharacterName => characterName;
        public AIGenerateModule(string characterName)
        {
            this.characterName = characterName;
        }

        public IEnumerator Process(IObjectResolver resolver)
        {
            var promptBuilder = resolver.Resolve<AIPromptBuilder>();
            var task = promptBuilder.Generate(CharacterName, token);
            yield return new WaitUntil(() => task.IsCompleted);
            var response = task.Result;
            if (response.Status) resolver.Resolve<IContent>().Content = response.Response;
            promptBuilder.Append(CharacterName, response.Response);
        }
    }
}
