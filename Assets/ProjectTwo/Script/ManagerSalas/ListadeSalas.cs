using UnityEngine;
using UnityEngine.UI;

public class ListadeSalas : MonoBehaviour
{
    [SerializeField]
	private Text _roomNameText;
	private Text RoomNameText
	{
		get { return _roomNameText;}
	}

	public string RoomName { get; private set;}

	public bool Updated{ get; set;}	

	private void Start ()
	{
		GameObject mangeCanvasObj = ManageCanvas.instance.gameObject;
		if(mangeCanvasObj == null)
			return;

		ManageCanvas manageCanvas = mangeCanvasObj.GetComponent<ManageCanvas> ();

		Button button = GetComponent<Button> ();
		button.onClick.AddListener(() => manageCanvas.OnClickJoinRoom(RoomNameText.text));

	}

	private void OnDestroy()
	{
		Button button = GetComponent<Button>();
		button.onClick.RemoveAllListeners ();
	}

	public void SetRoonNameText(string text)
	{
		RoomName = text;
		RoomNameText.text = text;
	}
}
