
namespace PushForward
{
	using UnityEngine;
	using UnityEngine.Events;
	using PushForward.ExtensionMethods;

	public class TriggerAt : BaseMonoBehaviour
	{
		public enum TriggerPoint { Never = 0, Awake, Start, Enabled, Disabled, Destroyed }
		public enum TriggerPlatform { All = 0, Standalone, Android, iOS }

		[SerializeField] private TriggerPoint triggerPoint;
		[SerializeField] private TriggerPlatform triggerPlatform;
		[SerializeField] private float triggerDelayInSeconds;
#if DEBUG
		[SerializeField] private bool debugLog;
#endif
		[SerializeField] private UnityEvent triggerEvent;

		[ContextMenu("Trigger")]
		public void Trigger()
		{
#if UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE
#if DEBUG
			if (this.debugLog)
			{ this.Temp("Trigger", "Platform: " + Application.platform); }
#endif

			switch (this.triggerPlatform)
			{
				case TriggerPlatform.Standalone:
					if (Application.platform != RuntimePlatform.LinuxPlayer
						&& Application.platform != RuntimePlatform.OSXPlayer
						&& Application.platform != RuntimePlatform.WindowsPlayer)
					{ return; }
					break;
				case TriggerPlatform.Android:
					if (Application.platform != RuntimePlatform.Android
						&& Application.platform != RuntimePlatform.WindowsEditor)
					{ return; }
					break;
				case TriggerPlatform.iOS:
					if (Application.platform != RuntimePlatform.IPhonePlayer
						&& Application.platform != RuntimePlatform.OSXEditor)
					{ return; }
					break;
			}
#endif
#if DEBUG
			if (this.debugLog)
			{ this.Temp("Trigger", "Triggering " + this.triggerEvent.GetPersistentMethodName(0) + " in " + this.triggerDelayInSeconds + " seconds."); }
#endif

			if (this.triggerDelayInSeconds.FloatEqual(0))
			{ this.triggerEvent.Invoke(); }
			else { this.ActionInSeconds(this.triggerEvent.Invoke, this.triggerDelayInSeconds); }
		}

		public void AbortTrigger()
		{
			this.StopAllCoroutines();
		}

#region engine
		void Awake()
		{
			if (this.triggerPoint == TriggerPoint.Awake)
			{ this.Trigger(); }
		}

		void Start()
		{
			if (this.triggerPoint == TriggerPoint.Start)
			{ this.Trigger(); }
		}

		void OnEnable()
		{
			if (this.triggerPoint == TriggerPoint.Enabled)
			{ this.Trigger(); }
		}

		void OnDisable()
		{
			if (this.triggerPoint == TriggerPoint.Disabled)
			{ this.Trigger(); }
		}

		private void OnDestroy()
		{
			if (this.triggerPoint == TriggerPoint.Destroyed)
			{ this.Trigger(); }
		}
#endregion // engine
	}
}
