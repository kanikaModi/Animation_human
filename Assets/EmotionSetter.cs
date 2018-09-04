using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionSetter : MonoBehaviour {

    Mesh thisMesh;
    SkinnedMeshRenderer smr;
    int lefteye;
    int righteye;

    float[] happy;
    float[] sad;
    float[] disgust;
    float[] fear;
    float[] surprise;
    float[] anger;
    float[] neutral;

    public Transform lEye;
    public Transform rEye;

    Vector3 gazeTargetSmall;
    Vector3 gazeTargetLarge;
    Vector3 gazeTargetHome;
    float eyeSpeed = 20.0f;

    private void OnMouseDown()
    {
        SetEmotion(sad);
    }

    // Use this for initialization
	void Start () {
        smr = this.GetComponent<SkinnedMeshRenderer>();
        thisMesh = smr.sharedMesh;

        lefteye = thisMesh.GetBlendShapeIndex("Blink_Left");
        righteye = thisMesh.GetBlendShapeIndex("Blink_Right");

        neutral = new float[thisMesh.blendShapeCount];
        happy = new float[thisMesh.blendShapeCount];
        sad = new float[thisMesh.blendShapeCount];
        disgust = new float[thisMesh.blendShapeCount];
        fear = new float[thisMesh.blendShapeCount];
        surprise = new float[thisMesh.blendShapeCount];
        anger = new float[thisMesh.blendShapeCount];

        happy[thisMesh.GetBlendShapeIndex("Smile_Left")] = 100;
        happy[thisMesh.GetBlendShapeIndex("Smile_Right")] = 100;

        sad[thisMesh.GetBlendShapeIndex("Frown_Left")] = 100;
        sad[thisMesh.GetBlendShapeIndex("Frown_Right")] = 100;

        disgust[thisMesh.GetBlendShapeIndex("BrowsIn_Left")] = 100;
        disgust[thisMesh.GetBlendShapeIndex("BrowsIn_Right")] = 100;
        disgust[thisMesh.GetBlendShapeIndex("LowerLipIn")] = 100;
        disgust[thisMesh.GetBlendShapeIndex("MouthUp")] = 100;
        disgust[thisMesh.GetBlendShapeIndex("NoseScrunch_Left")] = 100;
        disgust[thisMesh.GetBlendShapeIndex("NoseScrunch_Right")] = 100;
        disgust[thisMesh.GetBlendShapeIndex("Squint_Left")] = 50;
        disgust[thisMesh.GetBlendShapeIndex("Squint_Right")] = 50;
        disgust[thisMesh.GetBlendShapeIndex("UpperLipOut")] = 100;

        fear[thisMesh.GetBlendShapeIndex("BrowsUp_Left")] = 100;
        fear[thisMesh.GetBlendShapeIndex("BrowsUp_Right")] = 100;
        fear[thisMesh.GetBlendShapeIndex("EyesWide_Left")] = 100;
        fear[thisMesh.GetBlendShapeIndex("EyesWide_Right")] = 100;
        fear[thisMesh.GetBlendShapeIndex("MouthOpen")] = 20;

        surprise[thisMesh.GetBlendShapeIndex("BrowsUp_Left")] = 100;
        surprise[thisMesh.GetBlendShapeIndex("BrowsUp_Right")] = 100;
        surprise[thisMesh.GetBlendShapeIndex("EyesWide_Left")] = 100;
        surprise[thisMesh.GetBlendShapeIndex("EyesWide_Right")] = 100;
        surprise[thisMesh.GetBlendShapeIndex("MouthNarrow_Left")] = 20;
        surprise[thisMesh.GetBlendShapeIndex("MouthNarrow_Right")] = 20;
        surprise[thisMesh.GetBlendShapeIndex("MouthOpen")] = 100;

        anger[thisMesh.GetBlendShapeIndex("Blink_Left")] = 50;
        anger[thisMesh.GetBlendShapeIndex("Blink_Right")] = 50;
        anger[thisMesh.GetBlendShapeIndex("BrowsDown_Left")] = 100;
        anger[thisMesh.GetBlendShapeIndex("BrowsDown_Right")] = 100;
        anger[thisMesh.GetBlendShapeIndex("Frown_Left")] = 50;
        anger[thisMesh.GetBlendShapeIndex("Frown_Right")] = 50;
        anger[thisMesh.GetBlendShapeIndex("LowerLipIn")] = 100;
        anger[thisMesh.GetBlendShapeIndex("NoseScrunch_Left")] = 100;
        anger[thisMesh.GetBlendShapeIndex("NoseScrunch_Right")] = 100;
        anger[thisMesh.GetBlendShapeIndex("UpperLipIn")] = 100;

        SetDefault();
        InvokeRepeating("DecayEmotion", 1, 0.5f);
        Invoke("Blink", 1);
        InvokeRepeating("Gaze", 0.5f, 0.1f);
        InvokeRepeating("UpdateGazeTarget", 0.5f, 0.5f);

        gazeTargetHome = rEye.position + this.transform.forward * 2;
        gazeTargetSmall = gazeTargetHome;
        gazeTargetLarge = gazeTargetHome;
	}

    void Gaze()
    {
        Vector3 direction = gazeTargetSmall - lEye.transform.position;
        lEye.transform.rotation = Quaternion.Slerp(lEye.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * eyeSpeed);

        direction = gazeTargetSmall - rEye.transform.position;
        rEye.transform.rotation = Quaternion.Slerp(rEye.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * eyeSpeed);
    }

    void UpdateGazeTarget()
    {
        gazeTargetSmall = gazeTargetLarge + this.transform.right * Random.Range(-0.03f, 0.03f);
        gazeTargetSmall = gazeTargetSmall + this.transform.up * Random.Range(-0.03f, 0.03f);

        if (Random.Range(0, 1000) < 200)
        {
            gazeTargetLarge = gazeTargetHome + this.transform.right * Random.Range(-0.2f, 0.2f);
            gazeTargetLarge = gazeTargetLarge + this.transform.up * Random.Range(-0.2f, 0.2f);
        }
    }

    void Blink()
    {
        smr.SetBlendShapeWeight(lefteye, 100);
        smr.SetBlendShapeWeight(righteye, 100);
        float nextBlink = Random.Range(0.5f, 4f);
        Invoke("Blink", nextBlink);
        Invoke("StopBlink", Random.Range(0.1f, 0.5f));
    }

    void StopBlink()
    {
        smr.SetBlendShapeWeight(lefteye, 0);
        smr.SetBlendShapeWeight(righteye, 0);
    }


    void DecayEmotion()
    {
        SetEmotion(neutral);
    }

    void BlendEmotion(int blendShape, float target, float speed)
    {
        float amount = Mathf.Lerp(smr.GetBlendShapeWeight(blendShape), target,
                                       Time.deltaTime * speed);
        smr.SetBlendShapeWeight(blendShape, amount);
    }

    void SetEmotion(float[] emotion)
    {
        for (int i = 0; i < emotion.Length; i++)
            BlendEmotion(i, emotion[i], 5.0f);
    }

    void SetDefault()
    {
        for (int i = 0; i < thisMesh.blendShapeCount; i++)
            smr.SetBlendShapeWeight(i, 0);
    }

    void SetHappy()
    {
        for (int i = 0; i < thisMesh.blendShapeCount; i++)
            smr.SetBlendShapeWeight(i, happy[i]);
    }

    void SetSad()
    {
        for (int i = 0; i < thisMesh.blendShapeCount; i++)
            smr.SetBlendShapeWeight(i, sad[i]);
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.Space))
            SetDefault();

        if (Input.GetKey(KeyCode.Alpha1))
            SetEmotion(happy);

        if (Input.GetKey(KeyCode.Alpha2))
            SetEmotion(sad);

        if (Input.GetKey(KeyCode.Alpha3))
            SetEmotion(disgust);

        if (Input.GetKey(KeyCode.Alpha4))
            SetEmotion(fear);

        if (Input.GetKey(KeyCode.Alpha5))
            SetEmotion(surprise);

        if (Input.GetKey(KeyCode.Alpha6))
            SetEmotion(anger);

	}
}
