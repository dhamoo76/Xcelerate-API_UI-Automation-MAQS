using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSM.Xcelerate.Test.Shared
{
    public static class Utility
    {
        /// <summary>
        /// Retry the <paramref name="waitFor"/> function has returned true until the <paramref name="timeout"/> has elapsed.
        /// If the <paramref name="waitFor"/> has not completed before the <paramref name="retryTime"/> has elapsed, the
        /// result of the function (including exceptions) will be forgotten, and the <paramref name="waitFor"/> will be
        /// called again.
        /// </summary>
        /// <param name="waitFor">The wait for function</param>
        /// <param name="retryTime">How long do we wait before retrying the wait for true function</param>
        /// <param name="timeout">Max timeout for the check</param>
        /// <param name="throwException">If the last check failed because of an exception should we throw the exception</param>
        /// <returns>True if the wait for true function returned true before timing out</returns>
        /// <remarks>This function could potentially be promoted to AutomationCore</remarks>
        public static async Task<bool> WaitAsync(Func<Task<bool>> waitForTrue, TimeSpan retryTime, TimeSpan timeout, bool throwException)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;
            Exception exception = null;

            do
            {
                try
                {
                    // Clear out old exception
                    exception = null;

                    // Check if the function returns true
                    if (await waitForTrue())
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    // Save of the exception if we want to throw exceptions
                    if (throwException)
                    {
                        exception = e;
                    }
                }

                // Give the system a second before checking if the page is updating
                Thread.Sleep(retryTime);
            }
            while ((DateTime.Now - start) < timeout);

            // Check if we had an exceptions
            if (throwException && exception != null)
            {
                throw exception;
            }

            // We timed out waiting for the function to return true
            return false;
        }


    }
}
