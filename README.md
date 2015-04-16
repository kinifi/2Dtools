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

	}

	private void Update()
	{
	
	//get the axis value of the arrow keys or joystick
		hx = Input.GetAxis("Horizontal");

		if(hx > 0.8f)
		{
		//play the run animation
			_animator.PlayMultiple(runAnimation, true);
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
	public void Flip()


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

