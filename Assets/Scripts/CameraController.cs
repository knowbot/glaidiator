using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float smoothing = 5f;
	[SerializeField] private Vector3 _offset;

	// Use this for initialization
	void Start() {}

	// Update is called once per frame
	private void LateUpdate()
	{
		Vector3 targetCamPos = target.position + _offset;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}