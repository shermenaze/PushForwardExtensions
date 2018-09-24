/*
	AnimatorExtender

	Description: Extends the Animator class.

	Created by: Eran "Sabre Runner" Arbel.
	Last Updated: 2018-08-27
*/

namespace PushForward.Extenders
{
	using System;
	using UnityEngine;
	using UnityEngine.Events;
	using PushForward.ExtensionMethods;

	public class AnimatorExtender : BaseMonoBehaviour
	{
		[SerializeField] Animator animator;

		#region animation events
		/// <summary>A description of an animation event.</summary>
		/// <remarks>Used to avoid just having events named Element 0.</remarks>
		[Serializable]
		public struct AnimationEvent
		{
			public string eventName;
			public UnityEvent eventInvoker;

			public void Invoke()
			{ this.eventInvoker.Invoke(); }
		}

		[Tooltip("Enter events you want triggered here. Then call them from inside the animation.")]
		[SerializeField] AnimationEvent[] animationEventArray = null;

		public void InvokeEvent(int eventNumber)
		{
			if (!eventNumber.Between(0, this.animationEventArray.Length - 1))
			{ return; }

			this.animationEventArray[eventNumber].Invoke();
		}
		#endregion // animation events

		#region engine
		private void OnValidate()
		{
			this.animator = this.GetComponent<Animator>();
		}
		#endregion // engine
	}
}