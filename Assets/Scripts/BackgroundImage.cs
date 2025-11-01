using UnityEngine;

public class BackgroundImage : MonoBehaviour
{

	[SerializeField] Camera mainCamera;
	private float imageHeightInUnits;

	private SpriteRenderer sr;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		// Screen aspect ratio
		// Big thanks to https://discussions.unity.com/t/convert-screen-width-to-distance-in-world-space/832150
		float aspectRatio = (float)Screen.width / Screen.height;
		float screenHeightInUnits = mainCamera.orthographicSize * 2;
		float screenWidthInUnits = screenHeightInUnits * aspectRatio;

		transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 5);

		sr = GetComponent<SpriteRenderer>();
		// float imageWidthInUnits = sr.sprite.rect.width / sr.sprite.pixelsPerUnit;
		imageHeightInUnits = sr.sprite.rect.height / sr.sprite.pixelsPerUnit;

		sr.drawMode = SpriteDrawMode.Tiled;
		sr.size = new Vector2(screenWidthInUnits, Mathf.Ceil(screenHeightInUnits) + 2 * imageHeightInUnits);
	}

	// Update is called once per frame
	void Update()
	{
		float newY = transform.position.y + (Time.deltaTime * .5f);
		if (newY >= imageHeightInUnits + mainCamera.transform.position.y)
		{
			newY -= imageHeightInUnits;
		}
		transform.position = new Vector3(transform.position.x, newY, transform.position.y);
	}
}
