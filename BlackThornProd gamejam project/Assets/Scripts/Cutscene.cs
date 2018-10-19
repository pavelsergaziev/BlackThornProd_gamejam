using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using System.

public class Cutscene : MonoBehaviour {

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float[] _timeStamps;

    private float _timer;
    private int _nextStateIndex;

	// Use this for initialization
	void Start () {
        
	}

    void Update()
    {
        if (_nextStateIndex < _timeStamps.Length - 1)
        {
            if (_timer > _timeStamps[_nextStateIndex])
            {
                _animator.SetInteger("switchStateTo", ++_nextStateIndex);
                _timer = 0;
            }

            _timer += Time.deltaTime;
        }
    }
	

    //вариант с корутиной
    private IEnumerator TimeLoop()
    {

        yield return new WaitForEndOfFrame();

        while (_nextStateIndex < _timeStamps.Length - 1)
        {            

            if (_timer > _timeStamps[_nextStateIndex])
            {
                _animator.SetInteger("switchStateTo", ++_nextStateIndex);
                _timer = 0;
            }

            _timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
            
            
        }

    }

}
