
using UnityEngine;

public class ColorCube : MonoBehaviour {
	PlayerHolder holder;
	   
	void Start() {
		holder = GameObject.Find("PlayerHolder").GetComponent<PlayerHolder>();
    }

    // Update is called once per frame
    void Update() {    
		foreach(Player player in holder.GetPlayers())
			if(player != null)
				if(player.GetCursor().CursorOverObject(this.gameObject))
					GetComponent<Renderer>().material.color = player.GetColor();
    }
}
