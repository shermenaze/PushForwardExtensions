
namespace PushForward
{
	using UnityEngine;

	public class ApplicationPublicAccess : BaseMonoBehaviour
	{
		[SerializeField] private bool quitOnAndroidBackButton;

		public void OpenURL(string url)
		{
			Application.OpenURL(url);
		}

		public void Quit(float delayInSeconds = 0)
		{
			this.ActionInSeconds(() => Application.Quit(), delayInSeconds);
		}

		private void Update()
		{
			if (this.quitOnAndroidBackButton && Input.GetKey(KeyCode.Escape))
			{ this.Quit(); }
		}
	}
}
