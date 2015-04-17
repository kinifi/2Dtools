# 2Dtools
a series of 2D tools for use in Unity

###Sprite Animator

####Setup: 

1. Attach the Anim_Sprite.cs script to a GameObject. It will require a Sprite Renderer
2. Make your own script and attach that to the same GameObject as in #1
3. make a class level variable with type Anim_Sprite
4. in Start() set your variable from #3 to GetComponent<Anim_Sprite>();
5. You are all setup! 

Example Code for Setup

```

	private Anim_Sprite _animator;

	// Use this for initialization
	void Start () {

		_animator = GetComponent<Anim_Sprite>();

	}
	
```

*How To Use*

There are two different ways to use this API. Locally and Stored. Both are setup very similar. Code is commented below to show you how this works

```

	private Anim_Sprite _animator;
	//arrays of sprites for each individual animation we want to play
	public Sprite[] runAnimation;
	public Sprite[] secondRun;
	public Sprite[] Idle;
	
	//Horizontal axis float for the arrow keys
	public float hx;

	// Use this for initialization
	void Start () {
    		
    		//get the Anim_Sprite class
		_animator = GetComponent<Anim_Sprite>();
		
		//STORE USAGE: Send your animation Name, Sprite Array, frame rate, and loop type once
		//this is done once in the beginning so you dont have to call ALL INFORMATION again
		//_animator.storeAnimation("Run", runAnimation, 16, true);
		
	}

	private void Update()
	{
	
	//get the axis value of the arrow keys or joystick
		hx = Input.GetAxis("Horizontal");

		if(hx > 0.8f)
		{
			//play the run animation
			//LOCAL USAGE: pass all the parameters you need to play
			_animator.PlayMultiple(runAnimation, true);
			
			//STORE USAGE: Above in the Start Method we already sent our info, now just call [1/2]
			// [2/2] the animation by name
			//_animator.playStoredAnimation("Run2");
		}
		else if(hx < -0.8f)
		{
			//play the other run animation
			_animator.PlayMultiple(secondRun, true);
		}
		else
		{
		//play the idle animation
		//idle animation is only one frame so we use PlaySingle
			_animator.PlaySingle(Idle);
		}

	}

```

*API:*

```

	/// <summary>
	/// Stores an Animation for later use
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="animation">Animation.</param>
	/// <param name="frameRate">Frame rate; default = 12</param>
	/// <param name="loop">If set to <c>true</c> loop.</param>
	public void storeAnimation(string name, Sprite[] animation, int frameRate, bool loop)
	
	/// <summary>
	/// Plays the Stored Animation
	/// </summary>
	/// <param name="animationName">Animation name; Not Case Sensitive </param>
	public void playStoredAnimation(string animationName)

	/// <summary>
	/// Sets the frame rate
	/// </summary>
	/// <param name="_newFrameRate">_new frame rate.</param>
	public void setFrameRate(int _newFrameRate)

	/// <summary>
	/// Play the specified Animation
	/// </summary>
	/// <param name="_animation">_animation.</param>
	/// <param name="loopAnimation">If set to <c>true</c> loop animation.</param>
	public void PlayMultiple(Sprite[] _animation, bool loopAnimation, int frameRate)

	/// <summary>
	/// Play the specified Single Sprite Animation
	/// </summary>
	/// <param name="_animation">_animation.</param>
	public void PlaySingle(Sprite[] _animation)

	/// <summary>
	/// Continues playing the Current Animation
	/// </summary>
	public void ContinuePlaying()

	/// <summary>
	/// Stops playing the Current Animation
	/// </summary>
	public void StopPlaying()

	/// <summary>
	/// Checks if the animation is playing or not
	/// </summary>
	/// <returns><c>true</c>, if playing was ised, <c>false</c> otherwise.</returns>
	public bool isPlaying()

	/// <summary>
	/// Flip the sprite on the x axis
	/// </summary>
	public void FlipX()

	/// <summary>
	/// Flip the sprite on the Y axis
	/// </summary>
	public void FlipY()

```

*Events:*

//Is called on the first frame of the animation that is playing

```

void OnSpriteEnter()

```

//Is called on the last frame the animation is playing

```

void OnSpriteExit()

```

#Q&A

- Looked into Resources loading and this would be find on PC but not on consoles. It loads way to slow. 
- No way in Runtime to change any of the settings you can do from the editor
- Sprite resolution would need to be handled by you manually. 

