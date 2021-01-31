using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	public bool isPaused = true;

	private float progress;
	private bool goingForward = true;

	private void Update () {
		if (isPaused)
			return;

		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop) {
					progress -= 1f;
				}
				else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		}
		else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		Vector3 position = spline.GetPoint(progress);
		transform.localPosition = position;
		if (lookForward) {
			Quaternion lookOnLook = Quaternion.LookRotation(spline.GetDirection(progress));
			transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);
		}
	}

	void StartNewPath(BezierSpline newPath, bool startPaused) {
		spline = newPath;
		progress = 0;
		isPaused = startPaused;
    }
}