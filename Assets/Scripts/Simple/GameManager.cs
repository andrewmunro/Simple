using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Component.State;
using Assets.Scripts.Simple.Components.Npc;
using UnityEngine;

namespace Assets.Scripts.Simple
{
    [RequireComponent(typeof(ServerManager))]
    public class GameManager : AbstractGameManager
    {
        public new static GameManager Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            Components.Add(new NpcComponent());
        }
    }
}
