using System;
using Xunit;
using HomeBudget888WPF;
using Budget;
using System.Collections.Generic;
using System.IO;

namespace PresenterTests
{
    public class TestView : ShowRecentInterface
    {
        public bool calledShowRecent;
        public string[] recents;
        public void ShowRecent(string[] data)
        {
            calledShowRecent = true;
            recents = data;
        }
    }

    [Collection("Sequential")]
    public class TestRecentFilesPresenter
    {
        const int NUMBER_OF_FILES_DISPLAYED = 5;
        readonly string TESTING_INI_FILE = WelcomePage.DEFAULT_STORAGE_FOLDER + "/testIni.ini";

        private void ClearTestingIniFile()
        {
            try
            {
                File.WriteAllText(TESTING_INI_FILE, string.Empty);
            }
            catch (Exception error)
            {
                // dont know if we should do anything here
            }
        }

        [Fact] 
        public void TestConstructor()
        {
            TestView view = new TestView();
            view.calledShowRecent = false;

            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            Assert.IsType<RecentItemsPresenter>(recentItemsPresenter);
            Assert.True(view.calledShowRecent);
        }

        [Fact]
        public void TestAddToIniFile_FiveItems()
        {
            TestView view = new TestView();
            ClearTestingIniFile();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            string[] filesToAdd = { "test1.db", "test2.db", "test3.db", "test4.db", "test5.db" };

            for (int i = 0; i < filesToAdd.Length; i++)
            {
                string currentFile = filesToAdd[i];
                recentItemsPresenter.AddToIniFile(currentFile);
                HomeBudget budget = new HomeBudget(currentFile, true);
            }

            // redoing this so that GetRecentFiles is called again
            recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            Assert.True(view.recents.Length == filesToAdd.Length);

            for (int i = 0; i < view.recents.Length; i++)
            {
                string currentFile = filesToAdd[i];
                Assert.True(view.recents[i] == currentFile);
            }
        }

        [Fact]
        public void TestAddToIniFile_SixItems()
        {
            TestView view = new TestView();
            ClearTestingIniFile();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            string[] filesToAdd = { "test1.db", "test2.db", "test3.db", "test4.db", "test5.db", "test6.db" };

            for (int i = 0; i < filesToAdd.Length; i++)
            {
                string currentFile = filesToAdd[i];
                recentItemsPresenter.AddToIniFile(currentFile);
                HomeBudget budget = new HomeBudget(currentFile, true);
            }

            // redoing this so that GetRecentFiles is called again
            recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            Assert.True(view.recents.Length == NUMBER_OF_FILES_DISPLAYED);

            for (int i = 0; i < view.recents.Length; i++)
            {
                string currentFile = filesToAdd[i + 1];
                Assert.True(view.recents[i] == currentFile);
            }
        }

        [Fact]
        public void TestRecentOrder()
        {
            TestView view = new TestView();
            ClearTestingIniFile();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            string[] filesToAdd = { "test1.db", "test2.db", "test3.db", "test1.db", "test4.db", "test3.db" };
            string[] correctOrder = { "test2.db", "test1.db", "test4.db", "test3.db" };
            const int NUMBER_OF_UNIQUE_FILES = 4;

            for (int i = 0; i < filesToAdd.Length; i++)
            {
                string currentFile = filesToAdd[i];
                recentItemsPresenter.AddToIniFile(currentFile);
                HomeBudget budget = new HomeBudget(currentFile, true);
            }

            // redoing this so that GetRecentFiles is called again
            recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);
            Assert.True(view.recents.Length == NUMBER_OF_UNIQUE_FILES);

            for (int i = 0; i < view.recents.Length; i++)
            {
                string currentFile = correctOrder[i];
                Assert.True(view.recents[i] == currentFile);
            }
        }

        [Fact]
        public void TestGetRecents_TwoItems()
        {
            TestView view = new TestView();
            ClearTestingIniFile();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            string[] filesToAdd = { "test1.db", "test2.db" };

            for (int i = 0; i < filesToAdd.Length; i++)
            {
                string currentFile = filesToAdd[i];
                recentItemsPresenter.AddToIniFile(currentFile);
                HomeBudget budget = new HomeBudget(currentFile, true);
            }

            // redoing this so that GetRecentFiles is called again
            recentItemsPresenter = new RecentItemsPresenter(view, TESTING_INI_FILE);

            Assert.True(view.recents.Length == filesToAdd.Length);

            for (int i = 0; i < view.recents.Length; i++)
            {
                string currentFile = filesToAdd[i];
                Assert.True(view.recents[i] == currentFile);
            }
        }
    }
}
