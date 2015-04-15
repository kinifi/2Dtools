using UnityEngine;
using System.Collections;

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

	//TODO: Implement debug calls when this is true
	//Debug the animator
	public bool DebugAnimator = false;

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
	/// Play the specified Animation
	/// </summary>
	/// <param name="_animation">_animation.</param>
	/// <param name="loopAnimation">If set to <c>true</c> loop animation.</param>
	public void PlayMultiple(Sprite[] _animation, bool loopAnimation)
	{
		Loop = loopAnimation;
		currentAnimation = _animation;
		playAnimation = true;
		DebugAnim("Play Multiple Animation Called: " +
		          									  " Frames: " + _animation.Length +
		          									  " Loop Animation: " + loopAnimation);
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
		DebugAnim("Play Single Animation Called: " +
		          " Frames: " + _animation.Length +
		          " Loop Animation: " + loopAnimation);
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
