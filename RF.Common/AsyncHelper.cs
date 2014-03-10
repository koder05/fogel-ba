using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using RF.Common.UI;

namespace RF.Common
{
    public static class AsyncHelper
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post((async) =>
            {
                try
                {
                    task().Start();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post((async) =>
            {
                try
                {
                    ret = task().Result;
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        /// <summary>
        /// Do "stitch" between two threads
        /// </summary>
        public static void Stitch(Action acyncTask, Action syncCallbackTask)
        {
            UIWaitOverdoor.On();
            Task.Factory.StartNew(acyncTask)
            .ContinueWith((t) =>
            {
                try
                {
                    if (t.IsFaulted)
                        //throw t.Exception.InnerException;
                        throw t.Exception;
                    if (syncCallbackTask != null)
                        syncCallbackTask.Invoke();
                }
                catch (Exception ex)
                {
                    UIErrorOverdoor.Show(ex);
                }
                finally
                {
                    UIWaitOverdoor.Off();
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Do "stitch" between two threads
        /// </summary>
        /// <param name="acyncTask">task in background</param>
        /// <param name="syncCallbackTask">task in main</param>
        /// <param name="commonParam">param to link second task</param>
        public static void Stitch(Action<object> acyncTask, Action<object> syncCallbackTask, object commonParam)
        {
            UIWaitOverdoor.On();
            Task.Factory.StartNew(acyncTask, commonParam)
            .ContinueWith((t) =>
            {
                try
                {
                    if (t.IsFaulted)
                        throw t.Exception.InnerException;
                    if (syncCallbackTask != null)
                        syncCallbackTask.Invoke(t.AsyncState);
                }
                catch (Exception ex)
                {
                    UIErrorOverdoor.Show(ex);
                }
                finally
                {
                    UIWaitOverdoor.Off();
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static Task<T> Stitch<T>(Func<object, T> acyncTask, Action<object, T> syncCallbackTask, object commonParam)
        {
            UIWaitOverdoor.On();
            var task = Task.Factory.StartNew<T>(acyncTask, commonParam);
            task.ContinueWith((t) =>
            {
                try
                {
                    if (t.IsFaulted)
                        throw t.Exception.InnerException;
                    if (syncCallbackTask != null)
                        syncCallbackTask.Invoke(t.AsyncState, t.Result);
                }
                catch (Exception ex)
                {
                    UIErrorOverdoor.Show(ex);
                }
                finally
                {
                    UIWaitOverdoor.Off();
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

            return task;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}
