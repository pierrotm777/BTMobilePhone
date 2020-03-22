//
//
//
#if NETCF
//
using System;

namespace PutGuiCs
{
    delegate void delegatePutFromNtiCaller(System.IO.Stream source, String name, String type, Int64 length);

    /// <summary>
    /// Replacing <c>delegate void PutFromNtiCaller(Stream source, String name, String type, Int64 length)</c> for the NETCF.
    /// </summary>
    class PutFromNtiCaller
    {
        //--------------------------------------------------------------
        String m_name;
        delegatePutFromNtiCaller m_method;
        BeginInvokeAsyncResult m_asyncResult;
        Exception m_error;
        //----

        //--------------------------------------------------------------
        public PutFromNtiCaller(delegatePutFromNtiCaller method)
        {
            m_method = method;
        }

        //--------------------------------------------------------------
        /// <summary>
        /// Sets the name of the task for diagnostic reasons.  The name is used to 
        /// name the IAsyncResult object produced too.
        /// </summary>
        public String DebugName {
            set {
                // delegate's Method property doesn't exist in the NETCF. :-(
                // Otherwise we'd do method.Method.Name in the constructor...
                m_name = value;
            }
        }

        //--------------------------------------------------------------
        WorkArguments m_args;
        struct WorkArguments {
            public System.IO.Stream source;
            public String name;
            public String type;
            public Int64 length;

            public WorkArguments(System.IO.Stream source, String name, String type, Int64 length)
            {
                this.source = source;
                this.name = name;
                this.type = type;
                this.length = length;
            }
        }

        public IAsyncResult BeginInvoke(System.IO.Stream source, String name, String type, Int64 length,
            AsyncCallback callback, object state)
        {
            m_asyncResult = new BeginInvokeAsyncResult(m_name, callback, state);
            //
            System.Threading.WaitCallback wcbk = new System.Threading.WaitCallback(WorkItem);
            m_args=new WorkArguments(source, name, type, length);
            System.Threading.ThreadPool.QueueUserWorkItem(wcbk);
            //
            return m_asyncResult;
        }

        public void EndInvoke(IAsyncResult ar)
        {
            if (!Object.ReferenceEquals(ar, m_asyncResult)) {
                throw new ArgumentException("Not same IAsyncResult object", "ar");
            }
            //
            // Note this really need to be this.m_asyncResult.WaitCompletion();
            // However we PutGuiCs calls EndInvoke from inside the callback, so by
            // definition IsCompleted is true...
            // TODO ! this.m_asyncResult.WaitCompletion();
            System.Diagnostics.Debug.Assert(this.m_asyncResult.IsCompleted);
            //
            if (m_error != null) {
                throw m_error;
            }
        }

        void WorkItem(object argument)
        {
            System.Diagnostics.Debug.Assert(System.Threading.Thread.CurrentThread.IsBackground);
            WorkArguments args = m_args;
            try {
                m_method.Invoke(args.source, args.name, args.type, args.length);
            } catch (Exception ex) {
                m_error = ex;
            } finally {
                m_asyncResult.SetCompleted();
                if (m_asyncResult.m_callback != null) {
                    m_asyncResult.m_callback.Invoke(m_asyncResult);
                }
            }
        }

        //--------------------------------------------------------------


    }//class


    class BeginInvokeAsyncResult : IAsyncResult
    {
        String m_name;
        readonly internal AsyncCallback m_callback;
        readonly Object m_state;
        //
        bool m_isCompleted;

        internal BeginInvokeAsyncResult(String name)
            :this(name, null, null)
        { }

        internal BeginInvokeAsyncResult(String name, AsyncCallback callback, Object state)
        {
            m_name = name;
            m_callback = callback;
            m_state = state;
        }

        internal void SetCompleted()
        {
            m_isCompleted = true;
        }

        #region IAsyncResult Members

        public object AsyncState
        {
            get { return m_state; }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool CompletedSynchronously
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsCompleted
        {
            get { return m_isCompleted; }
        }


        #endregion
    }//class

}//namespace

#endif // NETCF
//EOF
