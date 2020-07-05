#define DEBUG_AGENT

using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Windows.Phone.Speech.Synthesis;

namespace TileUpdater
{

    public class ScheduledAgent : ScheduledTaskAgent
    {

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            FlipTileData primaryTileData = new FlipTileData();
            primaryTileData.BackContent = "";
            primaryTileData.WideBackContent = primaryTileData.BackContent;
            primaryTileData.BackTitle = "";
            
            int tileID = (new Random()).Next(0, 25);
            string fileName = String.Format("_{0:D4}_Tip-{1}.png", tileID - 1, tileID);

            primaryTileData.WideBackBackgroundImage = new Uri("Assets/LiveTiles/Large/" + fileName, UriKind.Relative);
            primaryTileData.BackBackgroundImage = new Uri("Assets/LiveTiles/Medium/" + fileName, UriKind.Relative);

            ShellTile primaryTile = ShellTile.ActiveTiles.FirstOrDefault();
            primaryTile.Update(primaryTileData);

            #warning "Remove this on final release"
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(15));

            NotifyComplete();
        }
    }
}