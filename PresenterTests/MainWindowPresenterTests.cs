using System;
using Xunit;
using HomeBudget888WPF;
using Budget;
using System.Collections.Generic;
using System.IO;

namespace PresenterTests
{
    public class TestMainWindowView : ViewInterfaceHomeBudgetPage, ShowRecentInterface
    {
        public bool calledShowInputError;
        public bool calledShowAddCategoryWindow;
        public bool calledLoadCategories;
        public bool calledEnableSearchFeatureOnCategoriesDropdown;
        public bool calledShowTodayDate;
        public bool calledQuit;
        public bool calledClearForm;
        public bool calledPromptUserWithYesOrNoQuestion;
        public bool calledShowCurrentBudgetFileName;
        public bool calledShowAddedExpense;
        public bool calledDatabaseEmpty;
        public bool yesOrNoPromptAnswer;
        public string invalidInputErrorMessage;
        public string currentBudgetFilename;
        public bool databaseEmpty = true;
        public bool calledLoadSummaries;
        public string[] summaries;
        public bool calledLoadBudgetItems;
        public bool calledLoadBudgetItemsByMonth;
        public bool calledLoadBudgetItemsByCategory;
        public bool calledLoadBudgetItemsByCategoryAndMonth;
        public List<BudgetItem> budgetItems;
        public List<BudgetItemsByMonth> budgetItemsByMonth;
        public List<BudgetItemsByCategory> budgetItemsByCategory;
        public List<Dictionary<string, object>> budgetItemsByCategoryAndMonth;
        public bool calledChangeExpenseAddTitle;
        public bool calledEnableContextMenuItems;
        public string expenseAddTitle;
        public bool contextMenuItemsEnabled;
        public bool calledShowRecents;
        public List<BudgetItem> searchResults;
        public bool calledHideChart;

        //sprint 5
        public bool calledHandleNoResultsFound;
        public bool calledScrollToView;
        public bool calledDisplayResults;
        public bool calledHighlightAddedExpenseIntoView;
        public bool callledHighlightEdittedExpenseIntoView;
        public bool calledShowPieChart;
        public bool calledHideOptionForChart;
        public bool calledGiveOptionForChart;

        //public string userInputAmount;
        //public string userInputDescription;
        //public int userInputCategory;
        //public DateTime userInputDate;
        //public bool creditExpenseIsChecked;

        public void ShowAddedExpense(Expense expense)
        {
            calledShowAddedExpense = true;
        }

        public void ShowInputError(string message)
        {
            invalidInputErrorMessage = message;
            calledShowInputError = true;
        }

        public void ShowAddCategoryWindow(string saveFileLocation, bool newOrExisting)
        {
            calledShowAddCategoryWindow = true;
        }
        public void EnableSearchFeatureOnCategoriesDropdown()
        {
            calledEnableSearchFeatureOnCategoriesDropdown = true;
        }

        public void ShowTodayDate()
        {
            calledShowTodayDate = true;
        }

        public void Quit()
        {
            calledQuit = true;
        }

        public void ClearExpenseForm()
        {
            calledClearForm = true;
        }

        public bool PromptUserWithYesOrNoQuestion(string question)
        {
            calledPromptUserWithYesOrNoQuestion = true;

            return yesOrNoPromptAnswer;
        }

        public void ShowCurrentBudgetFileName(string fileName)
        {
            currentBudgetFilename = fileName;
            calledShowCurrentBudgetFileName = true;
        }

        public bool DatabaseEmpty(string fileName)
        {
            calledDatabaseEmpty = true;

            return databaseEmpty;
        }

        public void LoadSummaries(string[] summaries) 
        {
            calledLoadSummaries = true;
            this.summaries = summaries;
        }
        public void LoadBudgetItems(List<BudgetItem> budgetItems)
        {
            calledLoadBudgetItems = true;
            this.budgetItems = budgetItems;
        }


        public void LoadBudgetItemsByMonth(List<BudgetItemsByMonth> items)
        {
            calledLoadBudgetItemsByMonth = true;
            budgetItemsByMonth = items;
        }

        public void LoadCategories(List<Category> categories, Category dummyCategoryForNoCategoryFilterSelection)
        {
            calledLoadCategories = true;
        }

        public void LoadBudgetItemsByCategory(List<BudgetItemsByCategory> items)
        {
            calledLoadBudgetItemsByCategory = true;
            budgetItemsByCategory = items;
        }

        public void LoadBudgetItemsByCategoryAndMonth(List<Dictionary<string, object>> items, List<Category> categories)
        {
            calledLoadBudgetItemsByCategoryAndMonth = true;
            budgetItemsByCategoryAndMonth = items;
        }

        public void ChangeExpenseAddTitle(string title)
        {
            calledChangeExpenseAddTitle = true;
            expenseAddTitle = title;
        }

        public void EnableContextMenuItems(bool enableOrDisable)
        {
            calledEnableContextMenuItems = true;
            contextMenuItemsEnabled = enableOrDisable;
        }

        public void ShowRecent(string[] data)
        {
            calledShowRecents = true;
        }


        public void DisplaySearchResults(List<BudgetItem> items)
        {
            calledDisplayResults = true;
            this.searchResults = items;
        }

        public void ScrollSearchResultIntoView()
        {
            calledScrollToView = true;
        }


        public void HandleNoSearchResultsFound()
        {
            calledHandleNoResultsFound = true;
        }

        public void HighlightAddedExpenseIntoView()
        {
            calledHighlightAddedExpenseIntoView = true;
        }

        public void HighlightEdittedExpenseIntoView()
        {
            callledHighlightEdittedExpenseIntoView = true;
        }

        public void showPieChart(List<Dictionary<string, object>> items)
        {
            calledShowPieChart = true;
        }

        public void HideOptionForChart()
        {
            calledHideOptionForChart = true;
        }
        public void GiveOptionForChart()
        {
            calledGiveOptionForChart = true;
        }
        public void HideChart()
        {
            calledHideChart = true;
        }
    }
    public class MainWindowPresenterTests
    {
        const string TEST_DB = "testWPFDb.db";
        const string EMPTY_DATABASE = "empty_database.db";

        [Fact]
        public void TestConstructor()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);
            Assert.IsType<HomeBudgetWindowPresenter>(presenter);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestPrepareBudgetView()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.budgetItems.Clear();
            view.calledLoadSummaries = false;
            view.summaries = null;
            view.calledChangeExpenseAddTitle = false;
            view.expenseAddTitle = string.Empty;
            view.currentBudgetFilename = string.Empty;
            view.calledLoadCategories = false;
            view.calledEnableSearchFeatureOnCategoriesDropdown = false;
            view.calledShowTodayDate = false;
            view.calledShowCurrentBudgetFileName = false;

            presenter.PrepareBudgetView();

            Assert.True(TEST_DB == (view.currentBudgetFilename + ".db"));
            Assert.True(view.calledLoadCategories);
            Assert.True(view.calledEnableSearchFeatureOnCategoriesDropdown);
            Assert.True(view.calledShowTodayDate);
            Assert.True(view.calledShowCurrentBudgetFileName);
            Assert.True(view.calledLoadBudgetItems);
            Assert.True(view.calledLoadSummaries);
            Assert.True(view.summaries.Length == 4);
            Assert.True(view.summaries[0] == "Category");
            Assert.True(view.summaries[1] == "Month");
            Assert.True(view.summaries[2] == "Category & Month");
            Assert.True(view.summaries[3] == "No Summary");
            Assert.True(view.calledChangeExpenseAddTitle);
            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddExpense_InvalidAmountValue()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            view.invalidInputErrorMessage = string.Empty;
            view.calledShowInputError = false;
            view.calledPromptUserWithYesOrNoQuestion = false;
            view.calledShowAddedExpense = false;
            view.calledClearForm = false;

            string expenseAmount = "abc";
            string description = "Apple";
            int categoryId = 8;
            DateTime date = DateTime.Now;
            bool isCreditExpense = false;

            presenter.AddExpense(expenseAmount, description, date, categoryId, isCreditExpense);

            Assert.True(view.calledShowInputError);
            Assert.False(view.calledPromptUserWithYesOrNoQuestion);
            Assert.False(view.calledShowAddedExpense);
            Assert.False(view.calledClearForm);

            bool hasRightErrorMessage = view.invalidInputErrorMessage.Contains(" must be a number.");
            Assert.True(hasRightErrorMessage);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddExpense_InvalidDescription()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            view.invalidInputErrorMessage = string.Empty;
            view.calledShowInputError = false;
            view.calledPromptUserWithYesOrNoQuestion = false;
            view.calledShowAddedExpense = false;
            view.calledClearForm = false;

            string expenseAmount = "14.99";
            string description = "";
            int categoryId = 8;
            DateTime date = DateTime.Now;
            bool isCreditExpense = false;

            presenter.AddExpense(expenseAmount, description, date, categoryId, isCreditExpense);

            Assert.True(view.calledShowInputError);
            Assert.False(view.calledPromptUserWithYesOrNoQuestion);
            Assert.False(view.calledShowAddedExpense);
            Assert.False(view.calledClearForm);

            bool hasRightErrorMessage = view.invalidInputErrorMessage.Contains(" cannot be blank.");
            Assert.True(hasRightErrorMessage);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddExpense_DescriptionThatAlreadyExists_YesToPrompt()
        {
            Database.newDatabase(TEST_DB);
            string description = "Apple";
            HomeBudget budget = new HomeBudget(TEST_DB, true);
            budget.expenses.Add(DateTime.Now, 8, 0.99, description);


            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledShowInputError = false;
            view.calledPromptUserWithYesOrNoQuestion = false;
            //view.calledShowAddedExpense = false;
            view.calledClearForm = false;
            view.calledLoadBudgetItems = false;

            string expenseAmount = "14.99";          
            int categoryId = 8;
            DateTime date = DateTime.Now;
            bool isCreditExpense = false;

            view.yesOrNoPromptAnswer = true;
            presenter.AddExpense(expenseAmount, description, date, categoryId, isCreditExpense);

            Assert.False(view.calledShowInputError);
            Assert.True(view.calledPromptUserWithYesOrNoQuestion);
            Assert.True(view.calledLoadBudgetItems);
            Assert.True(view.calledClearForm);

            Database.CloseDatabaseAndReleaseFile();
        }


        [Fact]
        public void TestAddExpense_DescriptionThatAlreadyExists_NoToPrompt()
        {
            Database.newDatabase(TEST_DB);
            string description = "Apple";
            HomeBudget budget = new HomeBudget(TEST_DB, true);
            budget.expenses.Add(DateTime.Now, 8, 0.99, description);


            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledShowInputError = false;
            view.calledPromptUserWithYesOrNoQuestion = false;
            view.calledShowAddedExpense = false;
            view.calledClearForm = false;

            string expenseAmount = "14.99";
            int categoryId = 8;
            DateTime date = DateTime.Now;
            bool isCreditExpense = false;

            view.yesOrNoPromptAnswer = false;
            presenter.AddExpense(expenseAmount, description, date, categoryId, isCreditExpense);

            Assert.False(view.calledShowInputError);
            Assert.True(view.calledPromptUserWithYesOrNoQuestion);
            Assert.False(view.calledShowAddedExpense);
            Assert.True(view.calledClearForm);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddCategoryWithWindow()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            view.calledShowAddCategoryWindow = false;
            view.calledLoadCategories = false;

            presenter.AddCategory();

            Assert.True(view.calledShowAddCategoryWindow);
            Assert.True(view.calledLoadCategories);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddCategoryUsingCombobox_CategoryDoesNotAlreadyExist()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            view.calledLoadCategories = false;

            presenter.AddCategoryUsingComboBox("Paycheck");

            Assert.True(view.calledLoadCategories);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddCategoryUsingCombobox_CategoryAlreadyExists()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            view.calledShowAddCategoryWindow = false;
            view.calledLoadCategories = false;

            presenter.AddCategoryUsingComboBox("Vacation");

            Assert.False(view.calledLoadCategories);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestQuitMainWindow()
        {
            Database.newDatabase(TEST_DB);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            view.calledQuit = false;

            presenter.Quit();

            Assert.True(view.calledQuit);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestOpenExistingHomeBudget_NotEmptyDatabase()
        {
            string NONEMPTY_DATABASE = WelcomePage.DEFAULT_STORAGE_FOLDER + "/nonempty_database.db";

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);


            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, NONEMPTY_DATABASE, false, recentItemsPresenter);

            view.calledDatabaseEmpty = false; 

            presenter.OpenExistingHomeBudgetFromMainWindow(NONEMPTY_DATABASE);

            Assert.False(view.calledDatabaseEmpty);
        }

        [Fact]
        public void TestOpenExistingHomeBudget_EmptyDatabse_NoToPrompt()
        {
            File.Delete(EMPTY_DATABASE);

            string NONEMPTY_DATABASE = WelcomePage.DEFAULT_STORAGE_FOLDER + "/nonempty_database.db";

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, NONEMPTY_DATABASE, false, recentItemsPresenter);

            view.calledDatabaseEmpty = false;

            view.databaseEmpty = false;

            presenter.OpenExistingHomeBudgetFromMainWindow(EMPTY_DATABASE);

            Assert.True(view.calledDatabaseEmpty);
        }

        [Fact]
        public void TestOpenExistingHomeBudget_EmptyDatabse_YesToPrompt()
        {
            File.Delete(EMPTY_DATABASE);
            string NONEMPTY_DATABASE = WelcomePage.DEFAULT_STORAGE_FOLDER + "/nonempty_database.db";

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, NONEMPTY_DATABASE, false, recentItemsPresenter);

            view.calledDatabaseEmpty = false;

            view.databaseEmpty = true;

            presenter.OpenExistingHomeBudgetFromMainWindow(EMPTY_DATABASE);

            Assert.True(view.calledDatabaseEmpty);
        }

        [Fact]
        public void TestCreateNewBudgetFile()
        {
            const string TEST_NEW_BUDGET_FILE = "createNewBudgetFile.db";
            File.Delete(TEST_NEW_BUDGET_FILE);

            Database.newDatabase(TEST_DB);

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, true, recentItemsPresenter);

            presenter.CreateNewBudget(TEST_NEW_BUDGET_FILE);

            Assert.True(File.Exists(TEST_NEW_BUDGET_FILE));
        }

        [Fact]
        public void TestGateFilterRequest_LoadBudgetItems_NoFilters()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date = new DateTime(2022, 01, 01);

            budget.expenses.Add(date, 8, 0.99, "Apple");
            budget.expenses.Add(date, 12, 34, "Dinner");

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;
            view.calledEnableContextMenuItems = false;
            view.contextMenuItemsEnabled = false;

            presenter.GateFilterRequest(null, null, -1, "No Summary");

            Assert.True(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);
            Assert.True(view.calledEnableContextMenuItems);
            Assert.True(view.contextMenuItemsEnabled);

            Assert.Equal("Apple", view.budgetItems[0].ShortDescription);
            Assert.Equal("Dinner", view.budgetItems[1].ShortDescription);
            Assert.Equal(date, view.budgetItems[0].Date);
            Assert.Equal(date, view.budgetItems[1].Date);
            Assert.Equal(0.99, view.budgetItems[0].Amount);
            Assert.Equal(34, view.budgetItems[1].Amount);
            Assert.Equal(8, view.budgetItems[0].CategoryID);
            Assert.Equal(12, view.budgetItems[1].CategoryID);
        }

        [Fact]
        public void TestGateFilterRequest_LoadBudgetItems_Filter_By_Category_12()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date = new DateTime(2022, 01, 01);


            budget.expenses.Add(date, 8, 0.99, "Apple");
            budget.expenses.Add(date, 1, 0.99, "big bird");
            budget.expenses.Add(date, 2, 0.99, "Apple");
            budget.expenses.Add(date, 12, 11, "Dinner");
            budget.expenses.Add(date, 12, 888, "Supper");
            budget.expenses.Add(date, 12, 77, "Breakfast");

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(null, null, 12, "No Summary");

            Assert.True(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Equal(3, view.budgetItems.Count);
            Assert.Equal("Dinner", view.budgetItems[0].ShortDescription);
            Assert.Equal("Supper", view.budgetItems[1].ShortDescription);
            Assert.Equal("Breakfast", view.budgetItems[2].ShortDescription);
            Assert.Equal(date, view.budgetItems[0].Date);
            Assert.Equal(date, view.budgetItems[1].Date);
            Assert.Equal(date, view.budgetItems[2].Date);
            Assert.Equal(11, view.budgetItems[0].Amount);
            Assert.Equal(888, view.budgetItems[1].Amount);
            Assert.Equal(77, view.budgetItems[2].Amount);
            Assert.Equal(12, view.budgetItems[0].CategoryID);
            Assert.Equal(12, view.budgetItems[1].CategoryID);
            Assert.Equal(12, view.budgetItems[2].CategoryID);
        }


        [Fact]
        public void TestGateFilterRequest_LoadBudgetItems_Filter_From_Jan_March()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);
            DateTime date4 = new DateTime(2022, 04, 01);
            DateTime date5 = new DateTime(2022, 05, 01);


            budget.expenses.Add(date1, 1, 0.99, "Apple");
            budget.expenses.Add(date2, 1, 444, "big bird");
            budget.expenses.Add(date3, 1, 999, "Love");
            budget.expenses.Add(date4, 1, 11, "Dinner");
            budget.expenses.Add(date5, 1, 888, "Supper");


            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(date1, date3, -1, "No Summary");

            Assert.True(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Equal(3, view.budgetItems.Count);
            Assert.Equal("Apple", view.budgetItems[0].ShortDescription);
            Assert.Equal("big bird", view.budgetItems[1].ShortDescription);
            Assert.Equal("Love", view.budgetItems[2].ShortDescription);
            Assert.Equal(date1, view.budgetItems[0].Date);
            Assert.Equal(date2, view.budgetItems[1].Date);
            Assert.Equal(date3, view.budgetItems[2].Date);
            Assert.Equal(0.99, view.budgetItems[0].Amount);
            Assert.Equal(444, view.budgetItems[1].Amount);
            Assert.Equal(999, view.budgetItems[2].Amount);
            Assert.Equal(1, view.budgetItems[0].CategoryID);
            Assert.Equal(1, view.budgetItems[1].CategoryID);
            Assert.Equal(1, view.budgetItems[2].CategoryID);
        }

        [Fact]
        public void TestGateFilterRequest_LoadBudgetItems_Filter_From_Jan_March_And_Category_1()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);
            DateTime date4 = new DateTime(2022, 04, 01);
            DateTime date5 = new DateTime(2022, 05, 01);


            budget.expenses.Add(date1, 1, 0.99, "Apple");
            budget.expenses.Add(date2, 1, 444, "big bird");
            budget.expenses.Add(date3, 2, 999, "Love");
            budget.expenses.Add(date4, 2, 11, "Dinner");
            budget.expenses.Add(date5, 2, 888, "Supper");

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = GetBudgetItemsByMonthListNoFilters(budget);

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;

            view.budgetItems = null;

            presenter.GateFilterRequest(date1, date3, 1, "No Summary");

            Assert.True(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Equal(2, view.budgetItems.Count);
            Assert.Equal("Apple", view.budgetItems[0].ShortDescription);
            Assert.Equal("big bird", view.budgetItems[1].ShortDescription);
            Assert.Equal(date1, view.budgetItems[0].Date);
            Assert.Equal(date2, view.budgetItems[1].Date);
            Assert.Equal(0.99, view.budgetItems[0].Amount);
            Assert.Equal(444, view.budgetItems[1].Amount);
            Assert.Equal(1, view.budgetItems[0].CategoryID);
            Assert.Equal(1, view.budgetItems[1].CategoryID);
        }

        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategory_No_Date_No_Category()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);
            DateTime date4 = new DateTime(2022, 04, 01);
            DateTime date5 = new DateTime(2022, 05, 01);


            budget.expenses.Add(date1, 1, 1, "Apple");
            budget.expenses.Add(date2, 2, 1, "big bird");
            budget.expenses.Add(DateTime.Now, 2, 1, "big bird1"); // to change total 
            budget.expenses.Add(date3, 3, 1, "Love");
            budget.expenses.Add(date4, 4, 1, "Dinner");
            budget.expenses.Add(DateTime.Now, 4, 1, "Dinner1"); // to change total
            budget.expenses.Add(date5, 5, 1, "Supper");

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(null, null, -1, "Category");

            Assert.False(view.calledLoadBudgetItems);
            Assert.True(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Equal(5, view.budgetItemsByCategory.Count);
            Assert.Equal(1, view.budgetItemsByCategory[0].Total);
            Assert.Equal(2, view.budgetItemsByCategory[1].Total);
            Assert.Equal(1, view.budgetItemsByCategory[2].Total);
            Assert.Equal(2, view.budgetItemsByCategory[3].Total);
            Assert.Equal(1, view.budgetItemsByCategory[4].Total);
            Assert.Equal("Education", view.budgetItemsByCategory[0].Category);
            Assert.Equal("Entertainment", view.budgetItemsByCategory[1].Category);
            Assert.Equal("Food", view.budgetItemsByCategory[2].Category);
            Assert.Equal("Rent", view.budgetItemsByCategory[3].Category);
            Assert.Equal("Utilities", view.budgetItemsByCategory[4].Category);

        }

        [Fact]
        public void TestGateFilterRequest_LoadBudgetItemsByMonth_FilterByCat12()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = GetBudgetItemsByMonthListFilterByCat12(budget);

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItemsByMonth = null;
            view.calledEnableContextMenuItems = false;
            view.contextMenuItemsEnabled = false;

            presenter.GateFilterRequest(null, null, 12, "Month");

            Assert.False(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.True(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);
            Assert.True(view.calledEnableContextMenuItems);
            Assert.False(view.contextMenuItemsEnabled);

            Assert.Equal(expectedBudgetItemsByMonthList.Count, view.budgetItemsByMonth.Count);

            for (int i = 0; i < expectedBudgetItemsByMonthList.Count; i++)
            {
                Assert.Equal(expectedBudgetItemsByMonthList[i].Month, view.budgetItemsByMonth[i].Month);
                Assert.Equal(expectedBudgetItemsByMonthList[i].Total, view.budgetItemsByMonth[i].Total);
                Assert.Equal(expectedBudgetItemsByMonthList[i].Details.Count, view.budgetItemsByMonth[i].Details.Count);

                for (int j = 0; j < expectedBudgetItemsByMonthList[i].Details.Count; j++)
                {
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Amount, view.budgetItemsByMonth[i].Details[j].Amount);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Balance, view.budgetItemsByMonth[i].Details[j].Balance);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].CategoryID, view.budgetItemsByMonth[i].Details[j].CategoryID);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Date, view.budgetItemsByMonth[i].Details[j].Date);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].ShortDescription, view.budgetItemsByMonth[i].Details[j].ShortDescription);
                }
            }
        }

        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategory_No_Date_Category_2()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);
            DateTime date4 = new DateTime(2022, 04, 01);
            DateTime date5 = new DateTime(2022, 05, 01);


            budget.expenses.Add(date1, 1, 1, "Apple");
            budget.expenses.Add(date2, 2, 1, "big bird");
            budget.expenses.Add(DateTime.Now, 2, 1, "big bird1"); // to change total 
            budget.expenses.Add(date3, 3, 1, "Love");
            budget.expenses.Add(date4, 4, 1, "Dinner");
            budget.expenses.Add(DateTime.Now, 4, 1, "Dinner1"); // to change total
            budget.expenses.Add(date5, 5, 1, "Supper");

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);
       
            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = GetBudgetItemsByMonthListFilterByCat12(budget);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(null, null, 2, "Category");

            Assert.False(view.calledLoadBudgetItems);
            Assert.True(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Single(view.budgetItemsByCategory);
            Assert.Equal(2, view.budgetItemsByCategory[0].Total);
            Assert.Equal("Rent", view.budgetItemsByCategory[0].Category);

        }

        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategory_Month_Jan_March_No_Category()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);
            DateTime date4 = new DateTime(2022, 04, 01);
            DateTime date5 = new DateTime(2022, 05, 01);


            budget.expenses.Add(date1, 1, 1, "Apple");
            budget.expenses.Add(date2, 2, 1, "big bird");
            budget.expenses.Add(date2, 3, 1, "Love");
            budget.expenses.Add(date4, 4, 1, "Dinner");
            budget.expenses.Add(date5, 5, 1, "Supper");

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(date1, date3, -1, "Category");

            Assert.False(view.calledLoadBudgetItems);
            Assert.True(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Equal(3, view.budgetItemsByCategory.Count);
            Assert.Equal(1, view.budgetItemsByCategory[0].Total);
            Assert.Equal(1, view.budgetItemsByCategory[1].Total);
            Assert.Equal(1, view.budgetItemsByCategory[2].Total);

            //alphabetic
            Assert.Equal("Food", view.budgetItemsByCategory[0].Category);
            Assert.Equal("Rent", view.budgetItemsByCategory[1].Category);
            Assert.Equal("Utilities", view.budgetItemsByCategory[2].Category);

        }

        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategory_Month_Jan_March_Category_1()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);
            DateTime date4 = new DateTime(2022, 04, 01);
            DateTime date5 = new DateTime(2022, 05, 01);

            budget.expenses.Add(date1, 1, 1, "Apple");
            budget.expenses.Add(date2, 2, 1, "big bird");
            budget.expenses.Add(date2, 3, 1, "Love");
            budget.expenses.Add(date4, 4, 1, "Dinner");
            budget.expenses.Add(date5, 5, 1, "Supper");


            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);
            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(date1, date3, 1, "Category");

            Assert.False(view.calledLoadBudgetItems);
            Assert.True(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);
            Assert.True(view.calledEnableContextMenuItems);
            Assert.False(view.contextMenuItemsEnabled);

            Assert.Single(view.budgetItemsByCategory);
            Assert.Equal("Utilities", view.budgetItemsByCategory[0].Category);

        }


        [Fact]
        public void TestGateFilterRequest_LoadBudgetItemsByMonth_FilterByTimeframe()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);
            DateTime date4 = new DateTime(2022, 04, 01);
            DateTime date5 = new DateTime(2022, 05, 01);


            budget.expenses.Add(date1, 1, 1, "Apple");
            budget.expenses.Add(date2, 2, 1, "big bird");
            budget.expenses.Add(date2, 3, 1, "Love");
            budget.expenses.Add(date4, 4, 1, "Dinner");
            budget.expenses.Add(date5, 5, 1, "Supper");



            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);


            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = GetBudgetItemsByMonthListFilterByTimeFrame(budget);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(date1, date3, 1, "Category");

            Assert.False(view.calledLoadBudgetItems);
            Assert.True(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Single(view.budgetItemsByCategory);
            Assert.Equal(1, view.budgetItemsByCategory[0].Total);

            //alphabetic
            Assert.Equal("Utilities", view.budgetItemsByCategory[0].Category);

        }

        //[Fact]
        //public void TestGateFilterRequest_calledLoadBudgetItemsByCategoryAndMonth_No_Date_No_Category() 
        //{ 
        //    view.budgetItemsByMonth = null;
        //    view.calledEnableContextMenuItems = false;
        //    view.contextMenuItemsEnabled = false;

        //    presenter.GateFilterRequest(new DateTime(2022, 05, 01), new DateTime(2022, 06, 30), -1, "Month");

        //    Assert.False(view.calledLoadBudgetItems);
        //    Assert.False(view.calledLoadBudgetItemsByCategory);
        //    Assert.True(view.calledLoadBudgetItemsByMonth);
        //    Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);
        //    Assert.True(view.calledEnableContextMenuItems);
        //    Assert.False(view.contextMenuItemsEnabled);

        //    Assert.Equal(expectedBudgetItemsByMonthList.Count, view.budgetItemsByMonth.Count);

        //    for (int i = 0; i < expectedBudgetItemsByMonthList.Count; i++)
        //    {
        //        Assert.Equal(expectedBudgetItemsByMonthList[i].Month, view.budgetItemsByMonth[i].Month);
        //        Assert.Equal(expectedBudgetItemsByMonthList[i].Total, view.budgetItemsByMonth[i].Total);
        //        Assert.Equal(expectedBudgetItemsByMonthList[i].Details.Count, view.budgetItemsByMonth[i].Details.Count);

        //        for (int j = 0; j < expectedBudgetItemsByMonthList[i].Details.Count; j++)
        //        {
        //            Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Amount, view.budgetItemsByMonth[i].Details[j].Amount);
        //            Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Balance, view.budgetItemsByMonth[i].Details[j].Balance);
        //            Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].CategoryID, view.budgetItemsByMonth[i].Details[j].CategoryID);
        //            Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Date, view.budgetItemsByMonth[i].Details[j].Date);
        //            Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].ShortDescription, view.budgetItemsByMonth[i].Details[j].ShortDescription);
        //        }
        //    }
        //}

        [Fact]
        public void TestGateFilterRequest_LoadBudgetItemsByMonth_FilterByCat12AndByTimeframe()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = GetBudgetItemsByMonthListFilterByCat12AndByTimeFrame(budget);

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItemsByMonth = null;
            view.calledEnableContextMenuItems = false;
            view.contextMenuItemsEnabled = false;

            presenter.GateFilterRequest(new DateTime(2022, 05, 01), new DateTime(2022, 06, 30), 12, "Month");

            Assert.False(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.True(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);
            Assert.True(view.calledEnableContextMenuItems);
            Assert.False(view.contextMenuItemsEnabled);

            Assert.Equal(expectedBudgetItemsByMonthList.Count, view.budgetItemsByMonth.Count);

            for (int i = 0; i < expectedBudgetItemsByMonthList.Count; i++)
            {
                Assert.Equal(expectedBudgetItemsByMonthList[i].Month, view.budgetItemsByMonth[i].Month);
                Assert.Equal(expectedBudgetItemsByMonthList[i].Total, view.budgetItemsByMonth[i].Total);
                Assert.Equal(expectedBudgetItemsByMonthList[i].Details.Count, view.budgetItemsByMonth[i].Details.Count);

                for (int j = 0; j < expectedBudgetItemsByMonthList[i].Details.Count; j++)
                {
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Amount, view.budgetItemsByMonth[i].Details[j].Amount);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Balance, view.budgetItemsByMonth[i].Details[j].Balance);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].CategoryID, view.budgetItemsByMonth[i].Details[j].CategoryID);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Date, view.budgetItemsByMonth[i].Details[j].Date);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].ShortDescription, view.budgetItemsByMonth[i].Details[j].ShortDescription);
                }
            }

        }

        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategoryAndMonth_No_Date_Category_1()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);


            budget.expenses.Add(date1, 1, 0, "Apple");
            budget.expenses.Add(date1, 1, 0, "Apple1");
            budget.expenses.Add(date1, 2, 1, "big bird");
            budget.expenses.Add(date2, 2, 1, "big bird1");
            budget.expenses.Add(date2, 2, 1, "big bird2");
            budget.expenses.Add(date3, 3, 1, "Love");


            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(null, null, 1, "Category & Month");

            Assert.False(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.True(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Equal(2, view.budgetItemsByCategoryAndMonth.Count);

        }


        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategoryAndMonth_Date_Jan_March_Category_1()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);


            budget.expenses.Add(date1, 1, 0, "Apple");
            budget.expenses.Add(date1, 1, 0, "Apple1");
            budget.expenses.Add(date1, 1, 1, "big bird");
            budget.expenses.Add(date2, 1, 1, "big bird1");
            budget.expenses.Add(date2, 1, 1, "big bird2");
            budget.expenses.Add(date3, 1, 1, "Love");
            budget.expenses.Add(date3, 1, 1, "Love1");


            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.GateFilterRequest(null, null, -1, "Category & Month");

            Assert.False(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.True(view.calledLoadBudgetItemsByCategoryAndMonth);

            Assert.Equal(4, view.budgetItemsByCategoryAndMonth.Count);
        }

        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategoryAndMonth_Date_Jan_No_Category()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);


            List<Dictionary<string, object>> expectedBudgetItemByCategoryAndMonth = GetBudgetItemsByCategoryAndMonth_Jan_No_Category(budget);

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;
            view.budgetItemsByMonth = null;
            view.calledEnableContextMenuItems = false;
            view.contextMenuItemsEnabled = false;

            presenter.GateFilterRequest(date1, date2, 10, "Category & Month");

            //Assert.False(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.True(view.calledLoadBudgetItemsByCategoryAndMonth);
            Assert.True(view.calledEnableContextMenuItems);
            Assert.False(view.contextMenuItemsEnabled);


            Assert.True(AssertDictionaryForExpenseByCategoryAndMonthIsOK(view.budgetItemsByCategoryAndMonth[0], expectedBudgetItemByCategoryAndMonth[0]));

        }

        [Fact]
        public void TestGateFilterRequest_calledLoadBudgetItemsByCategoryAndMonth_Date_Jan_Category_2()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);


            List<Dictionary<string, object>> expectedBudgetItemByCategoryAndMonth = GetBudgetItemsByCategoryAndMonth_Jan_Category_2(budget);

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;
            view.budgetItemsByMonth = null;
            view.calledEnableContextMenuItems = false;
            view.contextMenuItemsEnabled = false;

            presenter.GateFilterRequest(date1, date2, 2, "Category & Month");

            //Assert.False(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.True(view.calledLoadBudgetItemsByCategoryAndMonth);
            Assert.True(view.calledEnableContextMenuItems);
            Assert.False(view.contextMenuItemsEnabled);


            Assert.True(AssertDictionaryForExpenseByCategoryAndMonthIsOK(view.budgetItemsByCategoryAndMonth[0], expectedBudgetItemByCategoryAndMonth[0]));

        }

        [Fact]
        public void Test_Context_Menu_Delete()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);


            budget.expenses.Add(date1, 1, 1, "emma");
            budget.expenses.Add(date2, 2, 1, "big bird");
            budget.expenses.Add(date3, 3, 1, "Love");

            BudgetItem budgetItem = new BudgetItem();

            budgetItem.Amount = -1;
            budgetItem.Balance = -1;
            budgetItem.Category = "Rent";
            budgetItem.CategoryID = 2;
            budgetItem.Date = date1;
            budgetItem.ExpenseID = 1;
            budgetItem.ShortDescription = "emma";


            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;

            presenter.DeleteItem(budgetItem, null, null, -1);

            Assert.True(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

        }


        [Fact]
        public void Test_Context_Menu_Edit()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            DateTime date1 = new DateTime(2022, 01, 01);
            DateTime date2 = new DateTime(2022, 02, 01);
            DateTime date3 = new DateTime(2022, 03, 01);


            budget.expenses.Add(date1, 1, 1, "emma");
            budget.expenses.Add(date2, 2, 1, "big bird");
            budget.expenses.Add(date3, 3, 1, "Love");

            BudgetItem budgetItem = new BudgetItem();

            budgetItem.Amount = -1;
            budgetItem.Balance = -1;
            budgetItem.Category = "Rent";
            budgetItem.CategoryID = 2;
            budgetItem.Date = date1;
            budgetItem.ExpenseID = 1;
            budgetItem.ShortDescription = "emma";

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItems = null;
            view.calledChangeExpenseAddTitle = false;

            presenter.ModifyItem(budgetItem);

            Assert.False(view.calledLoadBudgetItems);
            Assert.True(view.calledChangeExpenseAddTitle);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.False(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);

        }

        [Fact]
        public void TestGateFilterRequest_LoadBudgetItemsByMonth_NoFilters()
        {
            Database.newDatabase(TEST_DB);
            HomeBudget budget = new HomeBudget(TEST_DB, true);

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = GetBudgetItemsByMonthListNoFilters(budget);

            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, TEST_DB, false, recentItemsPresenter);

            view.calledLoadBudgetItems = false;
            view.calledLoadBudgetItemsByCategory = false;
            view.calledLoadBudgetItemsByMonth = false;
            view.calledLoadBudgetItemsByCategoryAndMonth = false;
            view.budgetItemsByMonth = null;
            view.calledEnableContextMenuItems = false;
            view.contextMenuItemsEnabled = false;

            presenter.GateFilterRequest(null, null, -1, "Month");

            Assert.False(view.calledLoadBudgetItems);
            Assert.False(view.calledLoadBudgetItemsByCategory);
            Assert.True(view.calledLoadBudgetItemsByMonth);
            Assert.False(view.calledLoadBudgetItemsByCategoryAndMonth);
            Assert.True(view.calledEnableContextMenuItems);
            Assert.False(view.contextMenuItemsEnabled);

            Assert.Equal(expectedBudgetItemsByMonthList.Count, view.budgetItemsByMonth.Count);

            for (int i = 0; i < expectedBudgetItemsByMonthList.Count; i++)
            {
                Assert.Equal(expectedBudgetItemsByMonthList[i].Month, view.budgetItemsByMonth[i].Month);
                Assert.Equal(expectedBudgetItemsByMonthList[i].Total, view.budgetItemsByMonth[i].Total);
                Assert.Equal(expectedBudgetItemsByMonthList[i].Details.Count, view.budgetItemsByMonth[i].Details.Count);

                for (int j = 0; j < expectedBudgetItemsByMonthList[i].Details.Count; j++)
                {
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Amount, view.budgetItemsByMonth[i].Details[j].Amount);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Balance, view.budgetItemsByMonth[i].Details[j].Balance);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].CategoryID, view.budgetItemsByMonth[i].Details[j].CategoryID);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].Date, view.budgetItemsByMonth[i].Details[j].Date);
                    Assert.Equal(expectedBudgetItemsByMonthList[i].Details[j].ShortDescription, view.budgetItemsByMonth[i].Details[j].ShortDescription);
                }
            }
        }


        private void AddGetBudgetItemsByMonthToHomeBudget(HomeBudget budget)
        {
            List<Expense> testExpenseList = new List<Expense>();

            testExpenseList.Add(new Expense(1, new DateTime(2022, 4, 20), 1, 0.99, "Apple"));
            testExpenseList.Add(new Expense(2, new DateTime(2022, 4, 20), 12, 34, "Dinner"));
            testExpenseList.Add(new Expense(3, new DateTime(2022, 5, 24), 12, 500, "Banff"));
            testExpenseList.Add(new Expense(4, new DateTime(2022, 6, 24), 6, 250, "Big Bird"));
            testExpenseList.Add(new Expense(5, new DateTime(2022, 7, 24), 7, 100, "pogo"));

            for (int i = 0; i < testExpenseList.Count; i++)
            {
                budget.expenses.Add(testExpenseList[i].Date, testExpenseList[i].Category,
                    testExpenseList[i].Amount, testExpenseList[i].Description);
            }
        }
        private List<BudgetItemsByMonth> GetBudgetItemsByMonthListNoFilters(HomeBudget budget)
        {
            AddGetBudgetItemsByMonthToHomeBudget(budget);

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = new List<BudgetItemsByMonth>();
            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-04",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 1, Amount = 0.99, Balance = 0.99, ShortDescription = "Apple", Date = new DateTime(2022, 4, 20)
                    },
                    new BudgetItem {
                        CategoryID = 12, Amount = 34, Balance = 34.99, ShortDescription = "Dinner", Date = new DateTime(2022, 4, 20)
                    }
                },
                Total = 34.99
            });

            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-05",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 12, Amount = 500, Balance = 500 + 34.99, ShortDescription = "Banff", Date = new DateTime(2022, 5, 24)
                    }
                },
                Total = 500
            });

            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-06",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 6, Amount = 250, Balance = 250 + 500 + 34.99, ShortDescription = "Big Bird", Date = new DateTime(2022, 6, 24)
                    }
                },
                Total = 250
            });

            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-07",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 7, Amount = 100, Balance = 100 + 250 + 500 + 34.99, ShortDescription = "pogo", Date = new DateTime(2022, 7, 24)
                    }
                },
                Total = 100
            });

            return expectedBudgetItemsByMonthList;
        }
        private List<BudgetItemsByMonth> GetBudgetItemsByMonthListFilterByCat12(HomeBudget budget)
        {
            AddGetBudgetItemsByMonthToHomeBudget(budget);

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = new List<BudgetItemsByMonth>();
            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-04",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 12, Amount = 34, Balance = 34, ShortDescription = "Dinner", Date = new DateTime(2022, 4, 20)
                    }
                },

                Total = 34
            });

            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-05",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 12, Amount = 500, Balance = 500 + 34, ShortDescription = "Banff", Date = new DateTime(2022, 5, 24)
                    }
                },
                Total = 500
            });

            return expectedBudgetItemsByMonthList;
        }
        private List<BudgetItemsByMonth> GetBudgetItemsByMonthListFilterByTimeFrame(HomeBudget budget)
        {
            AddGetBudgetItemsByMonthToHomeBudget(budget);

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = new List<BudgetItemsByMonth>();


            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-05",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 12, Amount = 500, Balance = 500, ShortDescription = "Banff", Date = new DateTime(2022, 5, 24)
                    }
                },
                Total = 500
            });

            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-06",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 6, Amount = 250, Balance = 250 + 500, ShortDescription = "Big Bird", Date = new DateTime(2022, 6, 24)
                    }
                },
                Total = 250
            });

            return expectedBudgetItemsByMonthList;
        }
        private List<BudgetItemsByMonth> GetBudgetItemsByMonthListFilterByCat12AndByTimeFrame(HomeBudget budget)
        {
            AddGetBudgetItemsByMonthToHomeBudget(budget);

            List<BudgetItemsByMonth> expectedBudgetItemsByMonthList = new List<BudgetItemsByMonth>();


            expectedBudgetItemsByMonthList.Add(new BudgetItemsByMonth
            {
                Month = "2022-05",
                Details = new List<BudgetItem>() {
                    new BudgetItem {
                        CategoryID = 12, Amount = 500, Balance = 500, ShortDescription = "Banff", Date = new DateTime(2022, 5, 24)
                    }
                },
                Total = 500
            });

            return expectedBudgetItemsByMonthList;
        }

        public List<Dictionary<string, object>> GetBudgetItemsByCategoryAndMonth_Jan_No_Category(HomeBudget budget)
        {

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            List<BudgetItem> budgetItems = new List<BudgetItem>();


            Expense expense1 = new Expense(1, new DateTime(2022, 1, 10), 10, 12, "hat (on credit)");
            BudgetItem budgetItem1 = new BudgetItem
            {
                CategoryID = expense1.Category,
                ExpenseID = expense1.Id,
                Amount = expense1.Amount
            };

            budgetItems.Add(budgetItem1);
            budget.expenses.Add(expense1.Date, expense1.Category, expense1.Amount, expense1.Description);

            list.Add(new Dictionary<string, object> {
                {"Month","2022-01" },
                {"Total", 12.00},
                {"details:Clothes", budgetItems },
                {"Clothes", 12.00}
                });


            return list;
        }


        public List<Dictionary<string, object>> GetBudgetItemsByCategoryAndMonth_Jan_Category_2(HomeBudget budget)
        {

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            List<BudgetItem> budgetItems = new List<BudgetItem>();


            Expense expense1 = new Expense(1, new DateTime(2022, 1, 10), 2, 12, "hat (on credit)");
            BudgetItem budgetItem1 = new BudgetItem
            {
                CategoryID = expense1.Category,
                ExpenseID = expense1.Id,
                Amount = expense1.Amount
            };

            budgetItems.Add(budgetItem1);
            budget.expenses.Add(expense1.Date, expense1.Category, expense1.Amount, expense1.Description);

            list.Add(new Dictionary<string, object> {
                {"Month","2022-01" },
                {"Total", 12.00},
                {"details:Rent", budgetItems },
                {"Rent", 12.00}
                });


            return list;
        }

        // sandys code
        private Boolean AssertDictionaryForExpenseByCategoryAndMonthIsOK(Dictionary<string, object> recordExpeted, Dictionary<string, object> recordGot)
        {
            try
            {
                foreach (var kvp in recordExpeted)
                {
                    String key = kvp.Key as String;
                    Object recordExpectedValue = kvp.Value;
                    Object recordGotValue = recordGot[key];


                    // ... validate the budget items
                    if (recordExpectedValue != null && recordExpectedValue.GetType() == typeof(List<BudgetItem>))
                    {
                        List<BudgetItem> expectedItems = recordExpectedValue as List<BudgetItem>;
                        List<BudgetItem> gotItems = recordGotValue as List<BudgetItem>;
                        for (int budgetItemNumber = 0; budgetItemNumber < expectedItems.Count; budgetItemNumber++)
                        {
                            Assert.Equal(expectedItems[budgetItemNumber].Amount, gotItems[budgetItemNumber].Amount);
                            Assert.Equal(expectedItems[budgetItemNumber].CategoryID, gotItems[budgetItemNumber].CategoryID);
                            Assert.Equal(expectedItems[budgetItemNumber].ExpenseID, gotItems[budgetItemNumber].ExpenseID);
                        }
                    }
                    // else ... validate the value for the specified key
                    else
                    {
                        Assert.Equal(recordExpectedValue, recordGotValue);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        #region SEARCH AND SCROLL TO BUDGET ITEM TEST

        [Fact]
        public void ItemFoundInSearch()
        {
            Database.newDatabase(EMPTY_DATABASE);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, EMPTY_DATABASE, true, recentItemsPresenter);

            presenter.AddExpense("12", "hat", DateTime.Now, 2, false, null, null, -1);
            presenter.AddExpense("2", "sock", DateTime.Now, 2, false, null, null, -1);
            presenter.AddExpense("20", "shirt", DateTime.Now, 2, false, null, null, -1);
            presenter.AddExpense("23", "pants", DateTime.Now, 2, false, null, null, -1);

            view.calledDisplayResults = false;
            view.calledHandleNoResultsFound = false;
            view.calledScrollToView = false;
            view.searchResults = null;

            presenter.SearchInList("s");

            Assert.True(view.calledDisplayResults);
            Assert.False(view.calledHandleNoResultsFound);
            Assert.True(view.calledScrollToView);
            Assert.True(view.searchResults.Count == 3);

            Assert.True(view.searchResults[0].ShortDescription == "sock");
            Assert.True(view.searchResults[1].ShortDescription == "shirt");
            Assert.True(view.searchResults[2].ShortDescription == "pants");            
        }


        [Fact]
        public void ItemNotFoundInSearch()
        {
            Database.newDatabase(EMPTY_DATABASE);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, EMPTY_DATABASE, true, recentItemsPresenter);

            presenter.AddExpense("12", "hat", DateTime.Now, 2, false, null, null, -1);
            presenter.AddExpense("2", "sock", DateTime.Now, 2, false, null, null, -1);
            presenter.AddExpense("20", "shirt", DateTime.Now, 2, false, null, null, -1);
            presenter.AddExpense("23", "pants", DateTime.Now, 2, false, null, null, -1);

            view.calledDisplayResults = false;
            view.calledHandleNoResultsFound = false;
            view.calledScrollToView = false;
            view.searchResults = null;

            presenter.SearchInList("x");

            Assert.False(view.calledDisplayResults);
            Assert.True(view.calledHandleNoResultsFound);        
        }

        [Fact]
        public void AddedItemScrollsIntoViewAndIsHighlighted()
        {
            Database.newDatabase(EMPTY_DATABASE);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, EMPTY_DATABASE, true, recentItemsPresenter);

            view.calledHighlightAddedExpenseIntoView = false;
            view.calledScrollToView = false;
            view.callledHighlightEdittedExpenseIntoView = false;

            presenter.AddExpense("12", "hat", DateTime.Now, 2, false, null, null, -1);

            Assert.True(view.calledHighlightAddedExpenseIntoView);
            Assert.True(view.calledScrollToView);
            Assert.False(view.callledHighlightEdittedExpenseIntoView);
            
        }

        [Fact]
        public void EdittedItemIsHighlighted()
        {
            Database.newDatabase(EMPTY_DATABASE);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, EMPTY_DATABASE, true, recentItemsPresenter);

            presenter.AddExpense("12", "hat", DateTime.Now, 2, false, null, null, -1);

            view.calledHighlightAddedExpenseIntoView = false;
            view.calledScrollToView = false;
            view.callledHighlightEdittedExpenseIntoView = false;

            BudgetItem budgetItem1 = new BudgetItem
            {
                CategoryID = 2,
                Amount = 12,
                ShortDescription = "hat",
                Date = DateTime.Now,
            };


            presenter.ModifyItem(budgetItem1, null, null, -1);
            presenter.AddExpense("12", "fedora", DateTime.Today, 2, false, null, null, -1);

            Assert.False(view.calledHighlightAddedExpenseIntoView);
            Assert.False(view.calledScrollToView);
            Assert.True(view.callledHighlightEdittedExpenseIntoView);
        }
        
        [Fact]
        public void PieChartDisplayed()
        {
            Database.newDatabase(EMPTY_DATABASE);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, EMPTY_DATABASE, true, recentItemsPresenter);

            view.calledShowPieChart = false;

            presenter.GetSummariesByMonthAndCategoryForPieChart();

            Assert.True(view.calledShowPieChart);
        }

        [Fact]
        public void ChartIsHiddenWhenExpenseIsAdded()
        {
            Database.newDatabase(EMPTY_DATABASE);
            TestMainWindowView view = new TestMainWindowView();
            RecentItemsPresenter recentItemsPresenter = new RecentItemsPresenter(view);
            HomeBudgetWindowPresenter presenter = new HomeBudgetWindowPresenter(view, EMPTY_DATABASE, true, recentItemsPresenter);

            view.calledHideChart = false;
            view.calledHideOptionForChart = false;

            presenter.AddExpense("12", "fedora", DateTime.Today, 2, false, null, null, -1);

            Assert.True(view.calledHideChart);
            Assert.True(view.calledHideOptionForChart);
        }

        #endregion



    }

}

