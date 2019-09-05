
namespace PushForward.EventSystem
{
	using Base;
	using UnityEngine;

	public class GameEventIntListener : GameEventListenerBase
    {
		/// <summary>This listener's event is an event with a number.</summary>
		[SerializeField] private GameEventInt gameEventInt;
		protected override GameEvent GameEvent => this.gameEventInt;
		/// <summary>This listener's event gets an integer.</summary>
		[SerializeField] private IntEvent intResponse;

		protected override void OnEventRaised()
		{ this.intResponse?.Invoke(this.gameEventInt.integer); }
	}
}
