using UnityEngine;

public class GridTransform : MonoBehaviour
{
	public int x;
	public int y;
	public Vector2 position;
	public float horizontalMoveSpeed = 12f;

	// Update is called once per frame
	public virtual void Update()
	{
		position = new Vector2(x, y);
		transform.localPosition = Vector2.MoveTowards(transform.localPosition, position, horizontalMoveSpeed * Time.deltaTime);
	}
	
	public void SetPosition( int newX, int newY, bool instant = false )
	{
		x = newX;
		y = newY;
		if ( instant )
		{
			transform.localPosition = new Vector2( newX, newY );
		}
	}
}
