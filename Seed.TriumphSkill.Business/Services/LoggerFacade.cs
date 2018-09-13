using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class LoggerFacade<T> : ILoggerFacade<T> where T : class
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(T));
        private readonly string correlationId;

        public LoggerFacade(string correlationId)
        {
            this.correlationId = correlationId;
        }

        public void Debug(object message)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Debug(message);
            }
        }

        public void Debug(object message, Exception exception)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Debug(message, exception);
            }
        }

        public void Error(object message)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Error(message);
            }
        }

        public void Error(object message, Exception exception)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Error(message, exception);
            }
        }

        public void Fatal(object message)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Fatal(message);
            }
        }

        public void Fatal(object message, Exception exception)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Fatal(message, exception);
            }
        }

        public void Info(object message)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Info(message);
            }
        }

        public void Info(object message, Exception exception)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Info(message, exception);
            }
        }

        public void Warn(object message)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Warn(message);
            }
        }

        public void Warn(object message, Exception exception)
        {
            using (log4net.LogicalThreadContext.Stacks["correlationId"].Push(correlationId))
            {
                logger.Warn(message, exception);
            }
        }
    }

}
