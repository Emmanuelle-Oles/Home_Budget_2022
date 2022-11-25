using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Budget;
using Microsoft.Win32;
using System.IO;


namespace HomeBudget888WPF
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml. Displays the options the user has
    /// to create a new home budget file, open an existing home budget file or to select
    /// one of the recent files that user has been working on. This page also allows
    /// for a color selection that will carry through the rest of the application.
    /// </summary>
    public partial class WelcomePage : Window, ViewInterfaceWelcomePage, ShowRecentInterface
    {
        WelcomePageWindowPresenter presenter;
        RecentItemsPresenter recentItemsPresenter;

        //private readonly List<TextBox> userinputs;
        //private readonly List<ListBoxItem> recentFilesSaved;
        public static string DEFAULT_STORAGE_FOLDER = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents/Budgets");

        /// <summary>
        /// Instantiates a new Welcome Page calls the <see cref="WelcomePageWindowPresenter"> ViewInterfaceWelcomePresenter</see>
        /// constructor passing in the current welcome window. This constructor also
        /// creates the default storage folders.
        /// </summary>
        public WelcomePage()
        {
            CreateBudgetDirectory(DEFAULT_STORAGE_FOLDER);

            InitializeComponent();

            recentItemsPresenter = new RecentItemsPresenter(this);
            presenter = new WelcomePageWindowPresenter(this, recentItemsPresenter);
            //recentItemsPresenter.GetRecentFiles();
        }

        private void CreateBudgetDirectory(string folder)
        {
            bool folderCreatedSuccessfully = false;

            try
            {
                if (!Directory.Exists(folder))
                {
                    // creating the default folder
                    Directory.CreateDirectory(folder);
                    folderCreatedSuccessfully = true;
                    DEFAULT_STORAGE_FOLDER = folder;
                }
                else
                {
                    folderCreatedSuccessfully = true;
                }

            }
            catch
            {
                folderCreatedSuccessfully = false; // unecessary
            }

            while (!folderCreatedSuccessfully)
            {
                try
                {
                    if (!Directory.Exists(folder))
                    {
                        // creating the default folder
                        Directory.CreateDirectory(folder);
                        folderCreatedSuccessfully = true;
                        DEFAULT_STORAGE_FOLDER = folder;
                        break;
                    }
                    else
                    {
                        break;
                    }

                }
                catch
                {
                    folderCreatedSuccessfully = false;
                    MessageBoxResult messageBoxResult = MessageBox.Show($"Cannot create Budget folder at path {folder}. Would you like to select another location for the budget folder?",
                        "Permission Denied", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Folders |\n";

                        if (openFileDialog.ShowDialog() == true)
                        {
                            string path = openFileDialog.FileName + "/Budgets";
                            folder = path;
                        }
                    }
                }
            }          
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenExistingBudgetFile();
        }

        /// <summary>
        /// Prompts a standard dialog box in the default storage folder the user can use to navigate
        /// to open an existing home budget file.
        /// </summary>
        public void OpenExistingBudgetFile()
        {

            OpenFileDialog openExistingFileWindow = new OpenFileDialog();

            openExistingFileWindow.InitialDirectory = System.IO.Path.GetFullPath(DEFAULT_STORAGE_FOLDER);

            openExistingFileWindow.Filter = "DB Files | *.db";

            if (openExistingFileWindow.ShowDialog() == true)
            {
                presenter.OpenExistingHomeBudget(openExistingFileWindow.FileName);
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            CreateNewBudgetFile();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            //no functionality yet -- waiting to discuss with client
        }

        private void DefaultMode_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).ChangeSkin(Skin.Default);
        }

        private void LightMode_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).ChangeSkin(Skin.Light);
        }

        private void DarkMode_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).ChangeSkin(Skin.Dark);
        }

        private void OtherMode_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).ChangeSkin(Skin.Other);
        }
        
        /// <summary>
        /// Prompts a standard dialog box in the default storage folder for the
        /// user to select a location where they would like to save their home budget file.
        /// </summary>
        public void CreateNewBudgetFile()
        {
            SaveFileDialog newBudgetFileWindow = new SaveFileDialog();

            newBudgetFileWindow.InitialDirectory = System.IO.Path.GetFullPath(DEFAULT_STORAGE_FOLDER);

            newBudgetFileWindow.Filter = "DB Files | *.db";

            if (newBudgetFileWindow.ShowDialog() == true)
            {
                presenter.CreateNewBudget(newBudgetFileWindow.FileName);
            }
        }

        private void lbRecentFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            String fileName = lbRecentFiles.SelectedItem as String;

            if (fileName != null)
            {
                presenter.OpenExistingHomeBudget(fileName);
                this.Close();
            }
        }


        /// <summary>
        /// Outputs the files in the directory having an extendion .db. This file
        /// contains the last five home budget files open by user.
        /// </summary>
        /// <param name="recentFilesSaved"> The collection of all the recent saved files.</param>
        public void ShowRecent(string[] recentFilesSaved)
        {
            for (int i = recentFilesSaved.Length - 1; i >= 0; i--)
            {
                lbRecentFiles.Items.Add(recentFilesSaved[i]);
                lbRecentFiles.FontSize = 18;
            }
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Prompts a message box that advises the user that the current existing file
        /// they selected is empty and if they would like to use it. This is to avoid 
        /// any confusion that could be caused by having an existing home budget file
        /// but no data inside.
        /// </summary>
        /// <param name="fileName">The specified file location of the current home 
        /// budget database file.</param>
        /// <returns>True, if the user wants to continue with the empty file. False, if they don't
        /// wish to continue with the empty file.</returns>
        public bool DatabaseEmpty(string fileName)
        {
            MessageBoxResult result =
            MessageBox.Show("This is an empty database. Would like to use this file? ", "Empty database", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return (result == MessageBoxResult.Yes);
        }

        /// <summary>
        /// Opens a new <see cref="MainWindow"> Main Window</see> and closes the Welcome Page.
        /// </summary>
        /// <param name="saveFileLocation">The specified file location of the current home 
        /// budget database file.</param>
        /// <param name="newOrExisting">Whether or not the specified current home budget 
        /// database file is new or not.</param>
        public void OpenMainWindow(string saveFileLocation, bool newOrExisting)
        {
            MainWindow mainWindow = new MainWindow(saveFileLocation, newOrExisting);
            this.Close();
            mainWindow.ShowDialog();
        }
    }
}