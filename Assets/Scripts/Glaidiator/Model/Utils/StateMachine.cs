using System;
using System.Collections.Generic;

namespace Glaidiator.Model.Utils
{
    public abstract class StateMachine
    {
	    public class State
		{
			public Action<float> Tick = TickDef;
			public Action Enter = EnterExitDef;
			public Action Exit = EnterExitDef;

			public Enum current;
		}

		public State state = new State();

		public Enum CurrentState
		{
			get => state.current;
			set {
				if (Equals(state.current, value)) { return; }
				state.current = value;
				Transition();
			}
		}
		
		
		/// <summary>
		/// Runs the exit method for the previous state. Updates all method delegates to the new
		/// state, and then runs the enter method for the new state.
		/// </summary>
		private void Transition()
		{
			state.Exit?.Invoke(); // call old state exit method
			
			state.Tick = ConfigureDelegate<Action<float>>("Tick", TickDef);
			state.Enter = ConfigureDelegate<Action>("Enter", EnterExitDef);
			state.Exit = ConfigureDelegate<Action>("Exit", EnterExitDef);
			
			state.Enter?.Invoke(); // call new state enter method
		}

		private readonly Dictionary<Enum, Dictionary<string, Delegate>> _cache = new Dictionary<Enum, Dictionary<string, Delegate>>();

		/// <summary>
		/// Retrieves the specific state method for the provided method root.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="methodRoot">Based method name that is appended to the state name by an underscore, in the form of X_methodRoot where X is a state name.</param>
		/// <param name="default"></param>
		/// <returns>The state specific method as a delegate or Default if it does not exist.</returns>
		private T ConfigureDelegate<T>(string methodRoot, T @default) where T : class
		{
			if (!_cache.TryGetValue(state.current, out Dictionary<string, Delegate> lookup))
			{ _cache[state.current] = lookup = new Dictionary<string, Delegate>(); }

			if (!lookup.TryGetValue(methodRoot, out Delegate returnValue)) {
				System.Reflection.MethodInfo mtd = GetType().GetMethod(state.current + "_" + methodRoot, System.Reflection.BindingFlags.Instance
					| System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);

				if (mtd != null) { returnValue = Delegate.CreateDelegate(typeof(T), this, mtd); }
				else { returnValue = @default as Delegate; }

				lookup[methodRoot] = returnValue;
			}

			return returnValue as T;

		}
		
		public abstract void Tick(float deltaTime);
		private static void TickDef(float f) { }
		private static void EnterExitDef() { }
    }
}