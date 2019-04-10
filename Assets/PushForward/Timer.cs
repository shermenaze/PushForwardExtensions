/*
 * Timer
 *
 * Description: General purpose timer class.
 *
 * Created by: Eran "Sabre Runner" Arbel.
 *
 * Last Updated: 2018-11-15
*/

namespace PushForward
{
	#region using
	using System;
	using UnityEngine;
	using PushForward.Base;
	using PushForward.ExtensionMethods;
	#endregion // using

	public class Timer : BaseMonoBehaviour
	{
		public enum DisplayMode { HHMMSS, HHMMSStt, MMSS, MMSStt, SS, SStt }

		#region Timer Action
		[Serializable]
		public class TimerEvent
		{
			[SerializeField] private double triggerTimeInSeconds;
			[SerializeField] private TimeSpanEvent timeSpanEvent;

			private TimeSpan triggerTime = new TimeSpan(-1);

			public bool Triggered { get; set; }
			public TimeSpan TriggerTime => this.triggerTime < TimeSpan.FromTicks(0)
											? this.triggerTime = TimeSpan.FromSeconds(this.triggerTimeInSeconds)
												: this.triggerTime;
			private TimeSpanEvent EventToTrigger => this.timeSpanEvent;

			private TimerEvent() { }

			public void Trigger()
			{ this.EventToTrigger?.Invoke(this.TriggerTime); }

			public static TimerEvent Create(TimeSpan triggerTime, TimeSpanEvent actionToTrigger)
			{ return new TimerEvent() { triggerTime = triggerTime, timeSpanEvent = actionToTrigger }; }

			public static TimerEvent Create(TimeSpanEvent actionToTrigger)
			{ return Create(TimeSpan.FromTicks(0), actionToTrigger); }
		}
		#endregion // timer action

		#region Fields
#pragma warning disable IDE0044 // Add readonly modifier
		[SerializeField] private double timerInSeconds;
		[SerializeField] private DisplayMode displayMode;
		[SerializeField] private UnityEngine.UI.Text outputText;
		[SerializeField] private TimerEvent[] timerActions;
		[SerializeField] private TimerEvent[] timerOverActions;
		#pragma warning restore IDE0044 // Add readonly modifier

		private TimeSpan time;
		#endregion // fields

		#region Setup
		/// <summary>Set Timer parameters.</summary>
		/// <param name="timerSpan">The time.</param>
		/// <param name="displayMode">How to display it.</param>
		/// <param name="timerActions">Actions to take in specific times.</param>
		/// <param name="timerOverActions">Actions to take when timer over.</param>
		public void Set(TimeSpan timerSpan, DisplayMode displayMode, TimerEvent[] timerActions, TimerEvent[] timerOverActions)
		{
			this.time = timerSpan;
			this.displayMode = displayMode;
			this.timerActions = timerActions;
			this.timerOverActions = timerOverActions;
		}
		#endregion // setup

		#region Timer
		private void OutputToText()
		{
			string output = string.Empty;

			switch (this.displayMode)
			{
				case DisplayMode.HHMMSS:
					output = this.time.Hours.ToString() + ":"
								+ this.time.Minutes.ToString("D2") + ":"
								+ this.time.Seconds.ToString("D2");
					break;
				default:
				case DisplayMode.HHMMSStt:
					output = this.time.Hours.ToString() + ":"
								+ this.time.Minutes.ToString("D2") + ":"
								+ this.time.Seconds.ToString("D2")
								+ "." + this.time.Milliseconds.ToString("D3").Remove(2);
					break;
				case DisplayMode.MMSS:
					output = this.time.Minutes.ToString() + ":" + this.time.Seconds.ToString("D2");
					break;
				case DisplayMode.MMSStt:
					output = this.time.Minutes.ToString() + ":" + this.time.Seconds.ToString("D2") + ":"
								+ this.time.Milliseconds.ToString("D3").Remove(2);
					break;
				case DisplayMode.SS: output = ((int)this.time.TotalSeconds).ToString(); break;
				case DisplayMode.SStt: output = this.time.TotalSeconds.ToString("N3"); break;
			}

			this.outputText.text = output;
		}

		/// <summary>Timer Running method.</summary>
		private void RunTimer()
		{
			// update timer
			this.time = this.time.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
			this.time = this.time.Max(0);

			// show
			this.OutputToText();

			// activate actions if available
			this.timerActions?.DoForEach(
				timerAction =>
				{
					if (!timerAction.Triggered && timerAction.TriggerTime >= this.time)
					{ timerAction.Trigger(); }
				});
		}

		private void TimerOverActions()
		{
			this.timerOverActions?.DoForEach(timerAction => { timerAction.Trigger(); });
		}

		[ContextMenu("Start Timer")]
		public void StartTimer()
		{
			Func<bool> predicate = () => this.time.Ticks > 0;
			this.ActionEachFrameWhilePredicate(this.RunTimer, predicate);
			this.ActionWhenPredicate(this.TimerOverActions, () => !predicate());
		}

		[ContextMenu("Pause Timer")]
		public void PauseTimer()
		{
			this.StopAllCoroutines();
		}
		#endregion // timer

		#region Engine
		private void Awake()
		{
			// set up timer from inspector
			this.time = TimeSpan.FromSeconds(this.timerInSeconds);
		}
		#endregion // engine
	}
}
