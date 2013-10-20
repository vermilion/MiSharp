using System;
using Caliburn.Micro;

namespace MiSharp
{
    public abstract class Result : IResult
    {
        public virtual void Execute(ActionExecutionContext context)
        {
            OnCompleted(this, new ResultCompletionEventArgs());
        }

        public event EventHandler<ResultCompletionEventArgs> Completed;

        protected virtual void OnCompleted(object sender, ResultCompletionEventArgs e)
        {
            if (Completed != null)
                Completed(sender, e);
        }
    }
}