using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
 {
	public Animator anim;
	public bool moving;
	public bool startCharge;
	public bool hit;
	
	void Update()
	{
		// we need to get all necessary values from other scripts firts
		if(GetComponent<CharacterControls>().GetMoving())
			moving = true;
		else
			moving = false;
		
		if(GamepadInput.GetRightTrigger(GetComponent<CharacterOwner>().GetOwner().GetGamepad()))
			startCharge = true;
		else
			startCharge = false;
		
		if(GamepadInput.GetRightTrigger(GetComponent<CharacterOwner>().GetOwner().GetGamepad()))
			hit = false;
		else
			hit = true;
		
		// And then use it for animation data
		anim.SetBool("Moving", moving);
		anim.SetBool("StartCharge", startCharge);
		anim.SetBool("Hit", hit);
	}
}
