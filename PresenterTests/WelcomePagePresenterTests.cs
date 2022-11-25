using System;
using Xunit;
using HomeBudget888WPF;
using Budget;
using System.Collections.Generic;
using System.IO;

namespace PresenterTests
{
    public class TestWelcomePageView : ViewInterfaceWelcomePage, ShowRecentInterface
    {
        public bool calledCreateNewBudgetFile;
        public bool calledDatabaseEmpty;
        public bool calledOpenExistingBudgetFile;
        public bool calledOpenMainWindow;
        public bool calledShowRecent = false;
        public bool calledShowError;
        public bool databaseEmpty;

        public void CreateNewBudgetFile()
        {
            calledCreateNewBudgetFile = true;
        }

        public bool DatabaseEmpty(string fileName)
        {
            calledDatabaseEmpty = true;

            return databaseEmpty;
        }

        public void OpenExistingBudgetFile()
        {
            calledOpenExistingBudgetFile = true;
        }

        public void OpenMainWindow(string saveFileLocation, bool newOrExisting)
        {
            try
            {
                HomeBudget homeBudget = new HomeBudget(saveFileLocation, newOrExisting);
                calledOpenMainWindow = true;
                
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void ShowError(string message)
        {
            calledShowError = true;
        }

        public void ShowRecent(string[] data)
        {
            calledShowRecent = true;
        }

        [Collection("Sequential")]
        public class WelcomePagePresenterTests
        {
            const string EMPTY_DATABASE = "empty_database.db";
            readonly string NONEMPTY_DATABASE = WelcomePage.DEFAULT_STORAGE_FOLDER + "/nonempty_database.db";

            [Fact]
            public void TestConstructor()
            {
                TestWelcomePageView view = new TestWelcomePageView();
                RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
                WelcomePageWindowPresenter welcomePageWindowPresenter = new WelcomePageWindowPresenter(view, recentItemsPresenter);

                Assert.IsType<WelcomePageWindowPresenter>(welcomePageWindowPresenter);
                Assert.True(view.calledShowRecent);
            }

            [Fact]
            public void TestCreateNewBudgetFile()
            {
                TestWelcomePageView view = new TestWelcomePageView();
                RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
                WelcomePageWindowPresenter welcomePageWindowPresenter = new WelcomePageWindowPresenter(view, recentItemsPresenter);

                view.calledOpenMainWindow = false;

                welcomePageWindowPresenter.CreateNewBudget(NONEMPTY_DATABASE);

                Assert.True(view.calledOpenMainWindow);

            }

            [Fact]
            public void TestOpenExistingHomeBudget_NotEmptyDatabase()
            {
                TestWelcomePageView view = new TestWelcomePageView();
                RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
                WelcomePageWindowPresenter welcomePageWindowPresenter = new WelcomePageWindowPresenter(view, recentItemsPresenter);

                view.calledOpenMainWindow = false;

                welcomePageWindowPresenter.OpenExistingHomeBudget(NONEMPTY_DATABASE);

                Assert.True(view.calledOpenMainWindow);

            }

            [Fact]
            public void TestOpenExistingHomeBudget_EmptyDatabse_NoToPrompt()
            {
                File.Delete(EMPTY_DATABASE);

                TestWelcomePageView view = new TestWelcomePageView();
                RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
                WelcomePageWindowPresenter welcomePageWindowPresenter = new WelcomePageWindowPresenter(view, recentItemsPresenter);

                view.calledDatabaseEmpty = false;
                view.calledOpenMainWindow = false;

                view.databaseEmpty = false;

                welcomePageWindowPresenter.OpenExistingHomeBudget(EMPTY_DATABASE);

                Assert.False(view.calledOpenMainWindow);
                Assert.True(view.calledDatabaseEmpty);

            }

            [Fact]
            public void TestOpenExistingHomeBudget_EmptyDatabse_YesToPrompt()
            {
                File.Delete(EMPTY_DATABASE);
                TestWelcomePageView view = new TestWelcomePageView();
                RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
                WelcomePageWindowPresenter welcomePageWindowPresenter = new WelcomePageWindowPresenter(view, recentItemsPresenter);

                view.calledDatabaseEmpty = false;
                view.calledOpenMainWindow = false;

                view.databaseEmpty = true;

                welcomePageWindowPresenter.OpenExistingHomeBudget(EMPTY_DATABASE);

                Assert.True(view.calledDatabaseEmpty);
                Assert.True(view.calledOpenMainWindow);

            }


            [Fact]
            public void TestShowRecent()
            {
                TestWelcomePageView view = new TestWelcomePageView();
                RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
                WelcomePageWindowPresenter welcomePageWindowPresenter = new WelcomePageWindowPresenter(view, recentItemsPresenter);

                Assert.True(view.calledShowRecent);
            }
        }
    }
}
