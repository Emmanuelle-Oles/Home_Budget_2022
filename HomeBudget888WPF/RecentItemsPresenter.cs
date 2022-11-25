using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HomeBudget888WPF
{
    public class RecentItemsPresenter
    {
        private ShowRecentInterface view;
        private string[] recentFilesSavedInDirectory;
        private const int NUMBER_OF_RECENTS_TO_DISPLAY = 5;
        private int currentFileIndexInRecents;
        private readonly string DEFAULT_INI_FILE_LOCATION = WelcomePage.DEFAULT_STORAGE_FOLDER + "/recentFiles.ini";
        private readonly string INIFILE;
        private const int FILE_NOT_FOUND_IN_RECENTS = -1;
        public RecentItemsPresenter(ShowRecentInterface view, string iniFileLocation = "default")
        {
            this.view = view;

            // this is done for a testing file
            if (iniFileLocation == "default")
            {
                INIFILE = DEFAULT_INI_FILE_LOCATION;
            }
            else
            {
                INIFILE = iniFileLocation; 
            }

            recentFilesSavedInDirectory = new string[NUMBER_OF_RECENTS_TO_DISPLAY];
            GetRecentFiles();
        }

        //public RecentItemsPresenter(ShowRecentInterface view, RecentItemsPresenter presenterToCopy)
        //{
        //    this.view = view;
        //    this.
        //}

        private void ShiftAllRecentFiles(int shiftIndex)
        {
            for (int i = shiftIndex; i < NUMBER_OF_RECENTS_TO_DISPLAY - 1; i++)
            {
                recentFilesSavedInDirectory[i] = recentFilesSavedInDirectory[i + 1];
            }
        }

        private int GetIndexOfFileInRecents(string filename)
        {

            for (int i = 0; i < recentFilesSavedInDirectory.Length; i++)
            {
                if (recentFilesSavedInDirectory[i] == filename)
                {
                    return i;
                }
            }

            return FILE_NOT_FOUND_IN_RECENTS;
        }
        public void AddToIniFile(string file)
        {
            int indexOfFileInRecents = GetIndexOfFileInRecents(file);
            if (indexOfFileInRecents != FILE_NOT_FOUND_IN_RECENTS)
            {
                ShiftAllRecentFiles(indexOfFileInRecents);
                currentFileIndexInRecents--;
            }

            if (currentFileIndexInRecents == NUMBER_OF_RECENTS_TO_DISPLAY)
            {
                currentFileIndexInRecents = NUMBER_OF_RECENTS_TO_DISPLAY - 1;
                ShiftAllRecentFiles(0);
            }

            recentFilesSavedInDirectory[currentFileIndexInRecents] = file;
            currentFileIndexInRecents++;

            try
            {
                File.WriteAllLines(INIFILE, recentFilesSavedInDirectory);
            }
            catch (Exception error)
            {
                // dont know if we should do anything here
            }

        }
        private void ReadINI()
        {
            if (File.Exists(INIFILE))
            {
                try
                {
                    string[] iniFileLines = File.ReadAllLines(INIFILE);
                    int counter = 0;

                    for (int i = 0; i < iniFileLines.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(iniFileLines[i]))
                        {
                            recentFilesSavedInDirectory[i] = iniFileLines[i];
                            counter++;
                        }
                    }

                    currentFileIndexInRecents = counter;
                }
                catch (Exception e)
                {

                }
            }
        }

        private string[] GetRecentsWithoutEmptySpacesInArray()
        {
            string[] spacelessArray = new string[currentFileIndexInRecents];

            for (int i = 0; i < recentFilesSavedInDirectory.Length; i++)
            {
                {
                    if (!string.IsNullOrEmpty(recentFilesSavedInDirectory[i]))
                    {
                        spacelessArray[i] = recentFilesSavedInDirectory[i];
                    }
                }
            }

            return spacelessArray;
        }
        private void GetRecentFiles()
        {
            ReadINI();

            if (recentFilesSavedInDirectory != null)
            {
                if (recentFilesSavedInDirectory.Length > 0)
                {
                    view.ShowRecent(GetRecentsWithoutEmptySpacesInArray());
                }
            }
        }
    }
}
