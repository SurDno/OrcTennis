using UnityEngine;

// TODO: Once players are able to die, GetCentralPointBetweenGameObjects function should take that into account and ignore them if they're dead.
public class CameraControl : MonoBehaviour {
	[Header("Settings")]
    [SerializeField] private  GameObject[] objectsToFollow;
    [SerializeField] private float moveSpeed = 4f;
	[SerializeField] [Range(0.0f, 6.0f)] private float minYValue = 4f;
	[SerializeField] [Range(6.0f, 12.0f)] private float maxYValue = 9f;
	[SerializeField] [Range(45.0f, 85.0f)] private float minAngle = 45f;
	[SerializeField] [Range(85.0f, 110.0f)] private float maxAngle = 90f;
	[SerializeField] [Range(0f, 5.0f)] private float incrementY = 5f;
	[SerializeField] [Range(0f, 10.0f)] private float incrementZ = 6f;
	const float minPos = -6.5f;
	const float maxPos = 6.5f;
	
	[Header("Current Values")]
	private bool follow = true;
	private Vector3 overwrittenPosition;
	private float overwrittenAngle;
	private float savedMoveSpeed;
	
    void Update() {
		Vector3 desiredPosition;
		float desiredAngle;
		
		if(follow) {
			Vector3 center = GetCentralPointBetweenGameObjects(objectsToFollow);

			// Calculate the distance from the center to the farthest object.
			float distance = 0f;
			foreach (GameObject obj in objectsToFollow)
				if(obj != null)
					distance = Mathf.Max(Vector3.Distance(center, new Vector3(obj.transform.position.x, 0, obj.transform.position.z)), distance);
			
			// Adjust the camera position based on the distance and center.
			desiredPosition = center - transform.forward * distance;
			desiredPosition.y += incrementY;
			desiredPosition.y = Mathf.Max(desiredPosition.y, minYValue);
			desiredPosition.y = Mathf.Min(desiredPosition.y, maxYValue);
			
			// Calculate the lowest possible angle depending on how far we are from the highest possible point, adjust z position since we're now covering less screen area.
			desiredAngle = minAngle + (maxAngle - minAngle) * ((maxPos - center.z) / (maxPos - minPos));
			desiredPosition.z -= incrementZ * ((maxPos - center.z) / (maxPos - minPos));
		} else {
			desiredPosition = overwrittenPosition;
			desiredAngle = overwrittenAngle;
		}

        // Move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
		transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(desiredAngle, 0, 0), moveSpeed * Time.deltaTime);
    }
	
	// Gets the point that lies right between all the given objects by adding their positions together and then dividing by number of objects.
	Vector3 GetCentralPointBetweenGameObjects(GameObject[] gameObjectArray) {
		Vector3 center = Vector3.zero;
		int actualObjs = 0;
		
        foreach (GameObject obj in gameObjectArray) {
			if(obj == null)
				continue;
			
			center += obj.transform.position;
			actualObjs++;
		}
		
        center /= actualObjs;
		center.y = 0;
		
		return center;
	}
	
	public void OverwritePosition(Vector3 newPosition, float newAngle, float transitionSpeed) {
		overwrittenPosition = newPosition;
		overwrittenAngle = newAngle;
		follow = false;
		savedMoveSpeed = moveSpeed;
		moveSpeed = transitionSpeed;
	}
	
	public void ContinueFollowing() {
		follow = true;
		moveSpeed = savedMoveSpeed;
	}
}