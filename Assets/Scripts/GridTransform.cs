using UnityEngine;

public class GridTransform : MonoBehaviour
{
	public int x;
	public int y;
	public Vector2 position;
	public float horizontalMoveSpeed = 12f;

	// Update is called once per frame
	void Update() {
		position = new Vector2(x, y);
		transform.position = Vector2.MoveTowards(transform.position, position, horizontalMoveSpeed * Time.deltaTime );
    }
}
