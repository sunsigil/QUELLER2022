using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MachineState Template
/*void State(StateSignal signal)
{
	switch(signal)
	{
		case StateSignal.ENTER:

		break;

		case StateSignal.TICK:

		break;

		case StateSignal.FIXED_TICK:

		break;

		case StateSignal.EXIT:

		break;
	}
}*/
// LACK OF STATESIGNAL CONTROL FLOW WILL RESULT IN INFINITE LOOPS

public class Machine : MonoBehaviour
{
	public delegate void MachineState(StateSignal signal);

	MachineState state;

	public bool InState(MachineState state){ return this.state != null && this.state.Equals(state); }
	public void Transition(MachineState state)
	{
		if(this.state != null){ this.state(StateSignal.EXIT); }

		this.state = state;

		if(this.state != null){ this.state(StateSignal.ENTER); }
	}

	void Update(){ if(state != null){ state(StateSignal.TICK); } }
	void FixedUpdate(){ if(state != null){ state(StateSignal.FIXED_TICK); } }
}
