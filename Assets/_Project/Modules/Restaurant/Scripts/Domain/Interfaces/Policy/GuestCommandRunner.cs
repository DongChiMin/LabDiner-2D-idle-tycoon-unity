using System.Collections;
using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.Flow.Guest
{
    public class GuestCommandRunner : MonoBehaviour
    {
        Coroutine _routine;
        GuestCommandRuntime _runtime;

        public void Run(GuestContext ctx, IEnumerable<IGuestCommand> commands)
        {
            Stop();
            _runtime = new GuestCommandRuntime();
            _routine = StartCoroutine(RunRoutine(ctx, commands, _runtime));
        }

        public void Stop()
        {
            if (_runtime != null) _runtime.Cancel();
            if (_routine != null) StopCoroutine(_routine);
            _routine = null;
            _runtime = null;
        }

        IEnumerator RunRoutine(GuestContext ctx, IEnumerable<IGuestCommand> commands, GuestCommandRuntime runtime)
        {
            foreach (var cmd in commands)
            {
                if (runtime.IsCancelled) yield break;
                if (cmd == null) continue;
                yield return cmd.Execute(ctx, runtime);
            }
        }
    }
}