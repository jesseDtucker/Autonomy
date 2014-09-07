using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Autonomy.Model.Utility
{
    class SimpleTaskProcessor
    {
        #region Protected Members

        // is this needed?
        protected const int POLLING_INTERVAL = 1000; 

        protected int m_numThreads;
        protected bool m_isRunning = true;

        protected ConcurrentQueue<Action> m_tasks = new ConcurrentQueue<Action>();
        protected AutoResetEvent m_threadControl = new AutoResetEvent(true);

        protected int m_busyThreads = 0;
        protected ManualResetEvent m_externalWaitControl = new ManualResetEvent(false);

        #endregion

        #region Init

        public SimpleTaskProcessor()
        {
            m_numThreads = System.Environment.ProcessorCount;

            for (int i = 0; i < m_numThreads; ++i)
            {
                Task.Factory.StartNew(ProcessWork);
            }
        }

        #endregion

        #region Public Methods

        public void EnqueueWork(Action actionToExecute)
        {
            m_tasks.Enqueue(actionToExecute);
            m_threadControl.Set();
        }

        public void Shutdown()
        {
            m_isRunning = false;
            for (int i = 0; i < m_numThreads; ++i)
            {
                m_threadControl.Set();
            }
        }

        public async Task AwaitAllWorkCompleted()
        {
            await Task.Factory.StartNew(() =>
            {
                m_externalWaitControl.WaitOne();
            });
        }

        #endregion

        #region Processing

        protected void ProcessWork()
        {
            while (m_isRunning)
            {
                m_externalWaitControl.Reset();
                Interlocked.Increment(ref m_busyThreads);

                Action nextTask = null;
                while (m_isRunning && m_tasks.TryDequeue(out nextTask))
                {
                    nextTask();
                }
                
                Interlocked.Decrement(ref m_busyThreads);

                if (m_busyThreads == 0)
                {
                    m_externalWaitControl.Set();
                }

                if (m_isRunning)
                {
                    m_threadControl.WaitOne(POLLING_INTERVAL);
                }
            }
        }

        #endregion
    }
}
