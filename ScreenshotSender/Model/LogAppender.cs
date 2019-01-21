using log4net.Appender;
using log4net.Core;
using System;

namespace ScreenshotSender.Model
{
    public class LogAppender : AppenderSkeleton
    {
        public Action<string> LogAdded { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            LogAdded?.Invoke(loggingEvent.RenderedMessage);
        }
    }
}