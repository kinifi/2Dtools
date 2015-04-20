using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (SpriteRenderer))]
public class Anim_Sprite : MonoBehaviour {

	//The current animation playing
	private Sprite[] currentAnimation;

	//we also want to multiply the counter by a number to get a specific framerate
	private float frameRate = 12.0f;
	
	//Set a counter so the animation can be based on time
	private float counter = 0.0f;

	//The current frame counter that isn't time based but sprite based
	//Example: 0 = first sprite
	private int currentFrameCounter = 0;

	//Is the animation playing
	private bool playAnimation = false;

	//Should we loop the animation
	private bool Loop = false;

	//the sprite render attached to this object
	private SpriteRenderer _rend = null;
	
	//Debug the animator
	public bool DebugAnimator = false;

	/// <summary>
	/// List of the Animations Class stored in one area
	/// </summary>
	public List<Animations> animationsToPlay = new List<Animations>();

	
	/// <summary>
	/// Animations Class to store all the animations needed
	/// </summary>
	public class Animations 
	{
		public string name;
		public Sprite[] animation;
		public int frameRate;
		public bool loop;
	}

	#region event bools

	#endregion

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start()
	{
		_rend = GetComponent<SpriteRenderer>();
		DebugAnim("Getting SpriteRenderer");
	}

	// Update is called once per frame
	private void Update () {

		if(playAnimation)
		{
			//keeping track of time with counter
			counter += Time.deltaTime*frameRate;

			//change the frames and calls all Event Methods
			animationControl();

			//Check to see if the animation is done so we can loop it
			frameCompleteCheck();

		}
	
	}

	#region Public API for consumption and manipulation


	/// <summary>
	/// Stores an Animation for later use
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="animation">Animation.</param>
	/// <param name="frameRate">Frame rate; default = 12</param>
	/// <param name="loop">If set to <c>true</c> loop.</param>
	public void storeAnimation(string name, Sprite[] animation, int frameRate, bool loop)
	{
		Animations _anim = new Animations();
		_anim.animation = animation;
		_anim.name = name;
		_anim.frameRate = frameRate;
		_anim.loop = loop;

		animationsToPlay.Add(_anim);
		DebugAnim("Stored Animation: " + _anim.name);
	}

	/// <summary>
	/// Plays the Stored Animation
	/// </summary>
	/// <param name="animationName">Animation name; Not Case Sensitive </param>
	public void playStoredAnimation(string animationName)
	{
		DebugAnim("Searching for animation in list");
		//search through the stored animations and see if it exists
		for (int i = 0; i < animationsToPlay.Count; i++) 
		{
			//lower case all the animation names so we dont have to worry about case sensitivity
			if(animationName.ToLower() == animationsToPlay[i].name.ToLower())
			{
				DebugAnim("Animation Found: " + animationsToPlay[i].name);
				//check to see if we have a single sprite animation or a multiple
				if(animationsToPlay[i].animation.Length == 1)
				{
					//play single
					PlaySingle(animationsToPlay[i].animation);
				}
				else
				{
					//play multiple
					PlayMultiple(animationsToPlay[i].animation, animationsToPlay[i].loop, animationsToPlay[i].frameRate);
				}
			}

		}


	}


	/// <summary>
	/// Sets the frame rate
	/// </summary>
	/// <param name="_newFrameRate">_new frame rate.</param>
	public void setFrameRate(int _newFrameRate)
	{
		frameRate = _newFrameRate;
		DebugAnim("Set new frameRate: " + _newFrameRate);
	}

	/// <summary>
	/// Play the specified Animation
	/// </summary>
	/// <param name="_animation">_animation.</param>
	/// <param name="loopAnimation">If set to <c>true</c> loop animation.</param>
	public void PlayMultiple(Sprite[] _animation, bool loopAnimation, int frameRate)
	{
		setFrameRate(frameRate);
		Loop = loopAnimation;
		currentAnimation = _animation;
		playAnimation = true;
		DebugAnim("Play Multiple Animation Called: " + " Frames: " + _animation.Length + " Loop Animation: " + loopAnimation);
	}

	/// <summary>
	/// Play the specified Single Sprite Animation
	/// </summary>
	/// <param name="_animation">_animation.</param>
	public void PlaySingle(Sprite[] _animation)
	{
		Loop = true;
		currentAnimation = _animation;
		playAnimation = true;
		DebugAnim("Play Single Animation Called: " + " Frames: " + _animation.Length);
	}

	/// <summary>
	/// Continues playing the Current Animation
	/// </summary>
	public void ContinuePlaying()
	{
		playAnimation = true;
		DebugAnim("Continuing Play");
	}

	/// <summary>
	/// Stops playing the Current Animation
	/// </summary>
	public void StopPlaying()
	{
		playAnimation = false;
		DebugAnim("Animation Stopped");
	}

	/// <summary>
	/// Checks if the animation is playing or not
	/// </summary>
	/// <returns><c>true</c>, if playing was ised, <c>false</c> otherwise.</returns>
	public bool isPlaying()
	{
		DebugAnim("isPlaying Value: " + playAnimation);
		return playAnimation;
	}

	/// <summary>
	/// Flip the sprite on the Y axis
	/// </summary>
	public void FlipY() {
		
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
		DebugAnim("Flipping Sprite Y Axis");
	}


	/// <summary>
	/// Flip the sprite on the x axis
	/// </summary>
	public void FlipX() {

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		DebugAnim("Flipping Sprite X Axis");
	}

	#endregion


	#region Animation System Methods

	/// <summary>
	/// Animates the GameObject using the Sprite Renderer attached
	/// </summary>
	private void animationControl()
	{
		
		//check to see if we have any frames left to play
		if(counter > currentFrameCounter && currentFrameCounter < currentAnimation.Length)
		{
			//check to see if this is the start of the animation
			if(currentFrameCounter == 0)
			{
				onAnimationEnter();
			}
			else if(counter > currentAnimation.Length - 1)
			{
				onAnimationComplete();
			}

			//change the sprite render to reflect the 
			_rend.sprite = currentAnimation[currentFrameCounter];
			//increment the frame count so we can continue the animation and change frames 
			currentFrameCounter += 1;



		}
	}

	/// <summary>
	/// checks to see if the animation is on the last frame and loops it if it needs to
	/// </summary>
	private void frameCompleteCheck()
	{
		//If animation finishes, we destroy the object
		if(counter > currentAnimation.Length)
		{
			if(Loop)
			{
				DebugAnim("Resetting frames, starting Animation loop over");
				currentFrameCounter = 0;
				counter = 0.0f;
			}
			
		}
	}

	#endregion

	#region Event Methods

	/// <summary>
	/// Calls the OnEnter Animation to let your method know it has started
	/// </summary>
	public void onAnimationEnter()
	{
		DebugAnim("OnSpriteEnter is being called");
		SendMessage("OnSpriteEnter", SendMessageOptions.DontRequireReceiver);
	}

	/// <summary>
	/// Calls the OnComplete Animation to let your method know its done
	/// </summary>
	public void onAnimationComplete()
	{
		DebugAnim("OnSpriteExit is being called");
		SendMessage("OnSpriteExit", SendMessageOptions.DontRequireReceiver);
	}

	#endregion


	private void DebugAnim(string log)
	{
		if(DebugAnimator)
		{
			Debug.LogWarning(log);
		}
	}


}
