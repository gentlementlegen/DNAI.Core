using JetBrains.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;

// https://forum.unity.com/threads/synchronizationcontext-for-main-thread.433876/
namespace Core.Plugin.Unity.Context
{
    [InitializeOnLoad]
    public static class UnityTasks
    {
        private static readonly TaskScheduler Scheduler;

        static UnityTasks()
        {
            var context = new UnitySynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(context);
            Scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public static Task Run([NotNull] Func<Task> func)
        {
            if (func == null)
                throw new ArgumentNullException("func");

            var task = Task.Factory
                .StartNew(func, CancellationToken.None, TaskCreationOptions.DenyChildAttach, Scheduler)
                .Unwrap();

            return task;
        }

        public static Task<T> Run<T>([NotNull] Func<Task<T>> func)
        {
            if (func == null)
                throw new ArgumentNullException("func");

            var task = Task.Factory
                .StartNew(func, CancellationToken.None, TaskCreationOptions.DenyChildAttach, Scheduler)
                .Unwrap();

            return task;
        }

        public static Task Run([NotNull] Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var task = Task.Factory
                .StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, Scheduler);

            return task;
        }

        public static Task<T> Run<T>([NotNull] Func<T> func)
        {
            if (func == null)
                throw new ArgumentNullException("func");

            var task = Task.Factory
                .StartNew(func, CancellationToken.None, TaskCreationOptions.DenyChildAttach, Scheduler);

            return task;
        }
    }
}