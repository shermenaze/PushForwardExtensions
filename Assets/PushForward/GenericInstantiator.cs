
namespace PushForward
{
	using UnityEngine;

	public class GenericInstantiator : BaseMonoBehaviour
	{
		public enum Lineage { Child, Sibling, Parent, Root }

		#pragma warning disable IDE0044 // Add readonly modifier
		[Tooltip("The Prefab to instantiate.")]
		[SerializeField] private GameObject prefab;
		[Tooltip("Where in the hierarchy to put the new object?")]
		[SerializeField] private Lineage lineage;
		[Tooltip("What is the position offset to add?")]
		[SerializeField] private Vector3 positionOffset = Vector3.zero;
		[Tooltip("What is the rotation offset to add?")]
		[SerializeField] private Vector3 rotationOffset = Vector3.zero;
		#pragma warning restore IDE0044 // Add readonly modifier

		public void Instantiate()
		{
			// instantiate according to lineage
			GameObject newObject = Instantiate(this.prefab,
											   this.lineage == Lineage.Child ? this.transform
												: this.lineage == Lineage.Sibling
													|| this.lineage == Lineage.Parent ? this.transform.parent : null);

			// add required offsets
			newObject.transform.localPosition += this.positionOffset
													+ (newObject.transform.parent == null ? this.transform.position : Vector3.zero);
			newObject.transform.localRotation *= Quaternion.Euler(this.rotationOffset)
													* (newObject.transform.parent == null ? this.transform.rotation : Quaternion.identity);
		}
	}
}
