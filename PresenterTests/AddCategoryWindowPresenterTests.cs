using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Budget;
using HomeBudget888WPF;

namespace PresenterTests
{
    public class TestCategoryView : AddCategoryViewInterface
    {
        public bool calledGetUserInputCategoryName;
        public bool calledGetUserInputCategoryType;
        public bool calledResetCategoryAddingForm;
        public bool calledLoadCategoryTypes;
        public bool calledShowInputError;
        public int inputCategoryType;
        public string inputCategoryName;
        public string errorMessage;
        public string GetUserInputCategoryName()
        {
            calledGetUserInputCategoryName = true;

            return inputCategoryName;
        }

        public int GetUserInputCategoryType()
        {
            calledGetUserInputCategoryType = true;

            return inputCategoryType;
        }

        public void LoadCategoryTypes()
        {
            calledLoadCategoryTypes = true;
        }

        public void ResetCategoryAddingForm()
        {
            calledResetCategoryAddingForm = true;
        }

        public void ShowInputError(string message)
        {
            errorMessage = message;
            calledShowInputError = true;
        }
    }

    [Collection("Sequential")]
    public class AddCategoryWindowPresenterTests
    {
        const string TEST_DB = "testWPFDb.db";

        [Fact]
        public void TestConstructor()
        {
            Database.newDatabase(TEST_DB);
            TestCategoryView view = new TestCategoryView();
            AddCategoryWindowPresenter presenter = new AddCategoryWindowPresenter(view, TEST_DB, true);
            Assert.IsType<AddCategoryWindowPresenter>(presenter);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestPrepareAddCategoryWindow()
        {
            Database.newDatabase(TEST_DB);
            TestCategoryView view = new TestCategoryView();
            AddCategoryWindowPresenter presenter = new AddCategoryWindowPresenter(view, TEST_DB, true);

            view.calledLoadCategoryTypes = false;

            presenter.PrepareAddCategoryWindow();

            Assert.True(view.calledLoadCategoryTypes);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddCategory_ValidCategoryDescription()
        {
            Database.newDatabase(TEST_DB);
            TestCategoryView view = new TestCategoryView();
            AddCategoryWindowPresenter presenter = new AddCategoryWindowPresenter(view, TEST_DB, true);

            view.calledGetUserInputCategoryName = false;
            view.calledResetCategoryAddingForm = false;
            view.calledGetUserInputCategoryType = false;
            view.calledShowInputError = false;

            view.inputCategoryName = "Paycheck";
            view.inputCategoryType = 3;

            presenter.AddCategory();

            Assert.True(view.calledGetUserInputCategoryName);
            Assert.True(view.calledGetUserInputCategoryType);
            Assert.True(view.calledResetCategoryAddingForm);
            Assert.False(view.calledShowInputError);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddCategory_InvalidCategoryDescription_CategoryDescriptionAlreadyExists()
        {
            Database.newDatabase(TEST_DB);
            TestCategoryView view = new TestCategoryView();
            AddCategoryWindowPresenter presenter = new AddCategoryWindowPresenter(view, TEST_DB, true);

            view.calledGetUserInputCategoryName = false;
            view.calledResetCategoryAddingForm = false;
            view.calledGetUserInputCategoryType = false;
            view.calledShowInputError = false;
            view.errorMessage = string.Empty;

            view.inputCategoryName = "Vacation";
            view.inputCategoryType = 3;

            presenter.AddCategory();

            Assert.True(view.calledGetUserInputCategoryName);
            Assert.False(view.calledGetUserInputCategoryType);
            Assert.False(view.calledResetCategoryAddingForm);
            Assert.True(view.calledShowInputError);

            bool rightErrorMessage = view.errorMessage.Contains(" There is already ");
            Assert.True(rightErrorMessage);

            Database.CloseDatabaseAndReleaseFile();
        }

        [Fact]
        public void TestAddCategory_InvalidCategoryDescription_CategoryDescriptionIsBlank()
        {
            Database.newDatabase(TEST_DB);
            TestCategoryView view = new TestCategoryView();
            AddCategoryWindowPresenter presenter = new AddCategoryWindowPresenter(view, TEST_DB, true);

            view.calledGetUserInputCategoryName = false;
            view.calledResetCategoryAddingForm = false;
            view.calledGetUserInputCategoryType = false;
            view.calledShowInputError = false;
            view.errorMessage = string.Empty;

            view.inputCategoryName = " ";
            view.inputCategoryType = 3;

            presenter.AddCategory();

            Assert.True(view.calledGetUserInputCategoryName);
            Assert.False(view.calledGetUserInputCategoryType);
            Assert.False(view.calledResetCategoryAddingForm);
            Assert.True(view.calledShowInputError);

            bool rightErrorMessage = view.errorMessage.Contains(" cannot be blank.");
            Assert.True(rightErrorMessage);

            Database.CloseDatabaseAndReleaseFile();
        }
    }
}
