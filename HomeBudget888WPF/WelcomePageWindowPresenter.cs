using System;
using System.Collections.Generic;
using System.Text;
using Budget;
using System.IO;

namespace HomeBudget888WPF
{
    /// <summary>
    /// Interacts with the Welcome Page and the main Window. Manipulates mainly information
    /// regarding the storage of the home budget files.
    /// </summary>
    public class WelcomePageWindowPresenter
    {
        private HomeBudget budget;
        private ViewInterfaceWelcomePage view;

        private RecentItemsPresenter recentItemsPresenter;

        private string[] recentFilesSavedInDirectory;
        private string locationSavedFile = string.Empty;
        private const int NUMBER_OF_RECENTS_TO_DISPLAY = 5;
        private int currentFileIndexInRecents;
        private readonly string INIFILE = WelcomePage.DEFAULT_STORAGE_FOLDER + "/recentFiles.ini";
        private const int FILE_NOT_FOUND_IN_RECENTS = -1;

        /// <summary>
        /// Instatiates a new instance of the presenter of the welcome page with the
        /// <see cref="ViewInterfaceHomeBudgetPage"> ViewInterfaceWelcomePage</see> interface 
        /// .Also, displays up to five of the most recent files open by the user if any.
        /// </summary>
        /// <param name="view">The view that the presenter is communicating with to display 
        /// the welcome page.</param>
        public WelcomePageWindowPresenter(ViewInterfaceWelcomePage view, RecentItemsPresenter recentItemsPresenter)
        {
            this.view = view;
            this.recentItemsPresenter = recentItemsPresenter;
        }

       

        /// <summary>
        /// Opens the main window with the specified file name. If the database file is empty,
        /// the user is prompted a message box that with warning and asked if they would like to
        /// proceed with the empty. If they want to proceed with the file, the main window pops up and 
        /// the welcome page closes. If they don't want to proceed with the file, the user stays on the
        /// welcome page.
        /// </summary>
        /// <param name="fileName">The name of the existing file selected by the user.</param>
        public void OpenExistingHomeBudget(string fileName)
        {
            locationSavedFile = fileName;

            try
            {
                recentItemsPresenter.AddToIniFile(fileName);
                view.OpenMainWindow(locationSavedFile, false);

            }
            catch (Exception)
            {
                if (view.DatabaseEmpty(fileName))
                {
                    locationSavedFile = fileName;

                    recentItemsPresenter.AddToIniFile(fileName);

                    view.OpenMainWindow(locationSavedFile, true);
                }
            }
        }

        /// <summary>
        /// Opens the main window in the specified home budget file name.
        /// Also, adds the file name to the ini file that tracks the recent files opened.
        /// </summary>
        /// <param name="fileName">The name of the new file created by the user.</param>
        public void CreateNewBudget(string fileName)
        {
            locationSavedFile = fileName;

            recentItemsPresenter.AddToIniFile(fileName);
            view.OpenMainWindow(locationSavedFile, true);
        }
    }
}
