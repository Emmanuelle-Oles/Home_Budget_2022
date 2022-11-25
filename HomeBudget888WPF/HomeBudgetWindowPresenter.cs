using System;
using System.Collections.Generic;
using System.Text;
using Budget;
using System.Text.RegularExpressions;
using System.IO;

namespace HomeBudget888WPF
{
    /// <summary>
    /// Interacts with the Main Window and the model to send and manipulate the data between the two.
    /// </summary>
    public class HomeBudgetWindowPresenter
    {
        private HomeBudget budget;
        private ViewInterfaceHomeBudgetPage view;
        RecentItemsPresenter recentItemsPresenter;
        private string budgetFilename;
        private int ID_OF_CREDIT_CARD_CATEGORY = 9;
        private string[] summaries = { "Category", "Month", "Category & Month", "No Summary" };
        private BudgetItem itemBeingModified;
        private const string ADD_EXPENSE_TITLE = "New Expense";
        private Category defaultFilterNoCategory = new Category(-1, "No Category Filter", Category.CategoryType.Savings);
        private List<BudgetItem> displayedBudgetItems;

        /// <summary>
        /// Instatiates a new instance of the presenter and of home budget with the
        /// current home budget database filepath and whether or not the current home budget
        /// is a new or existing database, and stores these values as well as the specified view.
        /// Also calls the <see cref="PrepareBudgetView">PrepareBudgetView</see>
        /// method to set up all the default values to be displayed in the Main Window.
        /// </summary>
        /// <param name="view">The view that the presenter is communicating with to display 
        /// the home budget data.</param>
        /// <param name="budgetFilename">The current home budget database filepath.</param>
        /// <param name="newOrExisting">Whether or not the current home budget is a 
        /// new database or not.</param>
        public HomeBudgetWindowPresenter(ViewInterfaceHomeBudgetPage view, string budgetFilename, bool newOrExisting, RecentItemsPresenter recentItemsPresenter)
        {
            this.view = view;
            this.budget = new HomeBudget(budgetFilename, newOrExisting);
            this.budgetFilename = budgetFilename;
            this.recentItemsPresenter = recentItemsPresenter;
            PrepareBudgetView();

        }

        /// <summary>
        /// Sets up the Main Window with the default values to be displayed in the window, such as
        /// the current database file's file name and the current data as the selection in the new expense
        /// form datepicker. Also provides all of the current home budget's categories to the view so that 
        /// it can load all of them into the new expense form's category dropdown as well as enables the 
        /// autofill and combobox edit features on the categories dropdown.
        /// </summary>
        public void PrepareBudgetView()
        {
            view.LoadBudgetItems(budget.GetBudgetItems(null, null, false, 0));
            view.LoadCategories(budget.categories.List(), defaultFilterNoCategory);
            view.LoadSummaries(summaries);
            view.EnableSearchFeatureOnCategoriesDropdown();
            view.ShowTodayDate();
            view.ShowCurrentBudgetFileName(Path.GetFileNameWithoutExtension(budgetFilename));
            view.ChangeExpenseAddTitle(ADD_EXPENSE_TITLE);

        }

        /// <summary>
        /// Validates and parses all the specified new expense data and adds it to the current home budget's
        /// expenses if all of the data is valid. Displays a message to the user in any of the data is invalid 
        /// and if an expense cannot be created. If the new expense has the same description as an expense
        /// already in home budget's expenses, the user is prompted before adding it to the database.
        /// If the expense is allowed to be added to the database, it is input into the database
        /// and the sign of its expense amount is determined by its Category Type. Expenses and Savings
        /// amounts are written to the database with a negative sign. If the user indicated that
        /// the added expense was a credit card expense, a second version of the same expense is
        /// added to the database except with the Credit Card category and a positive amount for the expense.
        /// Once the specified expense is added to the database successfully, the new expense form is reset
        /// and the newly added expense is displayed in the expenses data grid.
        /// </summary>
        /// <param name="userInputAmount">The monetary worth of the expense.</param>
        /// <param name="description">The new expense's description.</param>
        /// <param name="date">The new expense's date.</param>
        /// <param name="categoryId">The category Id of the new expense's category.</param>
        /// <param name="creditExpenseChecked">Whether or not the new expense was a credit card expense.</param>
        public void AddExpense(string userInputAmount, string description, DateTime date, int categoryId,
            bool creditExpenseChecked, DateTime? startDate = null, DateTime? endDate = null, int categoryIdToFilterBy = 0)
        {
            userInputAmount = RemoveDollarSign(userInputAmount);
            bool filterByCategory = categoryIdToFilterBy == defaultFilterNoCategory.Id ? false : true;

            if (!(double.TryParse(userInputAmount, out double amount)))
            {
                view.ShowInputError("Input expense amount is invalid. Expense amount must be a number.");
            }
            else if (!ValidateUserInputExpenseDescription(description))
            {
                view.ShowInputError("Expense name is invalid. The expense name cannot be blank.");
            }
            else
            {

                bool expenseCanBeAdded = true;


                if (CheckIfExpenseNameAlreadyExists(description) && itemBeingModified == null)
                {
                    expenseCanBeAdded = view.PromptUserWithYesOrNoQuestion($"Expense with name {description} already exists in the list of expenses." +
                        $" Would you like to add this new expense anyway?");

                    //take off chart
                    view.HideChart();
                    view.HideOptionForChart();

                    view.LoadBudgetItems(budget.GetBudgetItems(startDate, endDate, filterByCategory, categoryIdToFilterBy));
                    view.ClearExpenseForm();
                }

                if (expenseCanBeAdded)
                {
                    //take off chart
                    view.HideChart();
                    view.HideOptionForChart();

                    if (itemBeingModified != null)
                    {
                        budget.expenses.Update(itemBeingModified.ExpenseID, date, description, amount, categoryId);
                        view.ChangeExpenseAddTitle(ADD_EXPENSE_TITLE);

                        view.LoadBudgetItems(budget.GetBudgetItems(startDate, endDate, filterByCategory, categoryIdToFilterBy));
                        view.ClearExpenseForm();

                        itemBeingModified = null;
                        view.HighlightEdittedExpenseIntoView();
                    }
                    else
                    {
                        budget.expenses.Add(date, categoryId, GetAmountBasedOnCategoryType(categoryId, amount), description);

                        if (creditExpenseChecked)
                        {
                            description += "(On Credit Card)";
                            budget.expenses.Add(date, ID_OF_CREDIT_CARD_CATEGORY, +amount, description);
                        }

                        view.LoadBudgetItems(budget.GetBudgetItems(startDate, endDate, filterByCategory, categoryIdToFilterBy));
                        view.ClearExpenseForm();
                        view.HighlightAddedExpenseIntoView();
                        view.ScrollSearchResultIntoView();
                    }
                }
            }

        }

        /// <summary>
        /// Clears the add expense form when it is in edit mode and toggles it back to its
        /// add expense regular form.
        /// </summary>
        public void ClearExpenseFormEditMode()
        {
            if(itemBeingModified != null)
            {
                view.ChangeExpenseAddTitle(ADD_EXPENSE_TITLE);
                itemBeingModified = null;
            }
        }

        /// <summary>
        /// Gets the criteria by which the user would like to filter the data and outputs the desired expense
        /// in the main window. By default, all expenses in the database are outputted in the main window.
        /// As the user selects each criteria the expense update dynamincally adding a level of filter each 
        /// selected criteria. For the filtering aspect, the user can select a start date, an end date and
        /// a category. If the user selects a time period, the expenses will be outputted with their 
        /// details. If the user selects a time period and a category, the expenses details and the 
        /// category information will be displayed. For the summary option, by default, displays
        /// a cumulative total of all of the budget items within each category by each month group 
        /// (items grouped by month and then by year). It also displays a grand total for each
        /// category accross all of the month groups and also a grand total for each month group accross
        /// all categories.
        /// </summary>
        /// <param name="startDate"> The start date of the period the user would like to view.</param>
        /// <param name="endDate"> The end of the date of the period the user would like to view.</param>
        /// <param name="category">The category of budget items. </param>
        /// <param name="summary"> The type of summary they would like to view: category, monthly and a specified category and month. </param>
        public void GateFilterRequest(DateTime? startDate, DateTime? endDate, int category, string summary)
        {
            bool filterByCategory = category == defaultFilterNoCategory.Id ? false : true;
            bool enableContextMenuButtons = false;

            if (summary == summaries[0]) // Category           
            {
                view.LoadBudgetItemsByCategory(budget.GeBudgetItemsByCategory(startDate, endDate, filterByCategory, category));
                view.HideOptionForChart();
                view.HideChart();

            }    
            else if (summary == summaries[1]) // Month
            {
                view.LoadBudgetItemsByMonth(budget.GetBudgetItemsByMonth(startDate, endDate, filterByCategory, category));
                view.HideOptionForChart();
                view.HideChart();

            }
            else if(summary == summaries[2]) // Category & Month
            {
                view.LoadBudgetItemsByCategoryAndMonth(budget.GetBudgetDictionaryByCategoryAndMonth(startDate, endDate, filterByCategory, category), budget.categories.List());
                view.GiveOptionForChart();
            }
            else // Get Budget Items
            {
                displayedBudgetItems = budget.GetBudgetItems(startDate, endDate, filterByCategory, category);
                view.LoadBudgetItems(displayedBudgetItems);
                enableContextMenuButtons = true;
                view.HideOptionForChart();
                view.HideChart();
            }

            view.EnableContextMenuItems(enableContextMenuButtons);  
            
        }

        private double GetAmountBasedOnCategoryType(int categoryID, double amount)
        {
            Category.CategoryType currentCategoryType = GetCategoryTypeOfCategory(categoryID);

            if (currentCategoryType == Category.CategoryType.Expense || currentCategoryType == Category.CategoryType.Savings)
            {
                return -amount;
            }
            else
            {
                return +amount;
            }

            //return amount;
        }

        /// <summary>
        /// Deletes the expense that was selected by the user and refreshes the expenses in the main window
        /// while maintaing the filtering criteria selected by the user if any. 
        /// </summary>
        /// <param name="budgetItemToDelete"> The budget item selected from the main window. </param>
        /// <param name="startDate"> The start date of the period that the user is viewing. </param>
        /// <param name="endDate"> The end of the period that the user is viewing. </param>
        /// <param name="category"> The category specified by the user. </param>
        public void DeleteItem(BudgetItem budgetItemToDelete, DateTime? startDate, DateTime? endDate, int category)
        {

            bool filterByCategory = category == defaultFilterNoCategory.Id ? false : true;

            int budgetItemId = budgetItemToDelete.ExpenseID;

            budget.expenses.Delete(budgetItemId);
            view.LoadBudgetItems(budget.GetBudgetItems(startDate, endDate, filterByCategory, category));

        }

        /// <summary>
        /// Updates the expense that was selected by the user
        /// </summary>
        /// <param name="itemToModify"></param>
        public void ModifyItem(BudgetItem itemToModify, DateTime? startDate = null, DateTime? endDate = null, int category = 0)
        {
            bool filterByCategory = category == defaultFilterNoCategory.Id ? false : true;

            itemBeingModified = itemToModify;
            view.ChangeExpenseAddTitle($"Modifying Budget Item: {itemBeingModified.ShortDescription}");

            //view.LoadBudgetItems(budget.GetBudgetItems(startDate, endDate, filterByCategory, category));
        }
        private Category.CategoryType GetCategoryTypeOfCategory(int categoryID)
        {
            Category category = budget.categories.GetCategoryFromId(categoryID);

            return category.Type;
        }
        private string RemoveDollarSign(string amount)
        {
            string[] stringAmountArray = amount.Split("$");
            StringBuilder stringAmount = new StringBuilder();

            foreach(string substring in stringAmountArray)
            {
                stringAmount.Append(substring);
            }

            return stringAmount.ToString();
        }
        private string RemoveDollarMinus(string amount)
        {
            return amount.Remove(0,1);
        }

        private bool ValidateUserInputExpenseDescription(string description)
        {
            if(description == string.Empty) // will "    " be considered an empty string?
            {
                return false;
            }          

            return true;
        }

        private bool CheckIfExpenseNameAlreadyExists(string description)
        {
            foreach (Expense expense in budget.expenses.List())
            {
                if (expense.Description.ToLower() == description.ToLower())
                {                  
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a new category to the current home budget based on the specified
        /// category description. If the specified category description matches
        /// the description of an already existing category in the home budget, 
        /// the specified category is not added to the database. Otherwise, it
        /// is added and the categories the categories in the Main Window new expense 
        /// dropdown are reloaded. Since only the description is specified, the
        /// created category's category type is expense by default.
        /// </summary>
        /// <param name="description">The description for the new category to be created.</param>
        public void AddCategoryUsingComboBox(string description)
        {
            bool expenseExists = false;

            foreach(Category category in budget.categories.List())
            {
                if(category.Description == description)
                {
                    expenseExists = true;
                    break;
                }
            }

            if (!expenseExists)
            {
                budget.categories.Add(description, Category.CategoryType.Expense);
                view.LoadCategories(budget.categories.List(), defaultFilterNoCategory);
            }
        }


        /// <summary>
        /// Opens a pop-up window to add new categories to the current home budget
        /// by calling the Main Window's <see cref="MainWindow.ShowAddCategoryWindow(string, bool)"/>
        /// ShowAddCategoryWindow method and passing in the current home budget database's filepath
        /// and false because the database file exists. Once categories are done being 
        /// added, it reinstantiates the current home budget to reopen the connection to the database file
        /// and receive any changes made in the Add Categories window. Afterwards, it reloads all of the
        /// categories in the new expense form's dropdown menu so that all categories displayed are present
        /// and the dropdown is up to date.
        /// </summary>
        public void AddCategory()
        {
            Database.CloseDatabaseAndReleaseFile();
            view.ShowAddCategoryWindow(budgetFilename, false);
            this.budget = new HomeBudget(budgetFilename, false);
            view.LoadCategories(budget.categories.List(), defaultFilterNoCategory);
        }

        /// tool bar
        /// <summary>
        /// Opens the main window with the specified file name. If the database file is empty,
        /// the user is prompted a message box that with warning and asked if they would like to
        /// proceed with the empty. If they want to proceed with the file, the main window pops up and 
        /// the welcome page closes. If they don't want to proceed with the file, the user stays on the
        /// welcome page.
        /// </summary>
        /// <param name="fileName">The name of the existing file selected by the user.</param>
        public void OpenExistingHomeBudgetFromMainWindow(string fileName)
        {
            Database.CloseDatabaseAndReleaseFile();
            try
            {
                budget = new HomeBudget(fileName, false);
                recentItemsPresenter.AddToIniFile(fileName);
                budgetFilename = fileName;
                
            }
            catch(Exception e)
            {
                if(view.DatabaseEmpty(fileName))
                {
                    budget = new HomeBudget(fileName, true);
                    recentItemsPresenter.AddToIniFile(fileName);
                }
            }

            PrepareBudgetView();
        }

        /// <summary>
        /// Closes the database connection to the current home budget database file and releases
        /// all the resources used by it. Also closes the Main Window, closing the application.
        /// </summary>
        public void Quit()
        {
            ReleaseConnection();
            view.Quit(); 
        }

        /// <summary>
        /// Closes the database connection to the current home budget database file and releases
        /// all the resources used by it.
        /// </summary>
        public void ReleaseConnection()
        {
            Database.CloseDatabaseAndReleaseFile();
        }

        /// <summary>
        /// Opens the new home budget file name in the main window.
        /// Also adds the file name to the ini file that tracks the recent files opened.
        /// </summary>
        /// <param name="fileName">The name of the new file created by the user.</param>
        public void CreateNewBudget(string fileName)
        {
            budgetFilename = fileName;
            recentItemsPresenter.AddToIniFile(fileName);

            budget = new HomeBudget(fileName, true);
            PrepareBudgetView();
        }

        /// <summary>
        /// Creates a list of home budget budget items that's description contains the specified search string. 
        /// If the created list contains items, it calls the <see cref="ViewInterfaceHomeBudgetPage.DisplaySearchResults(List{BudgetItem})">DisplaySearchResults</see>
        /// view method with the created list.
        /// </summary>
        /// <param name="searchString">The string input into the search bar.</param>
        public void SearchInList(string searchString)
        {
            List<BudgetItem> searchResults = new List<BudgetItem>();

            foreach (BudgetItem item in budget.GetBudgetItems(null, null, false, 1))
            {
                if (item.ShortDescription.ToLower().Contains($"{searchString.ToLower()}"))
                {
                    searchResults.Add(item);
                }
            }

            if(searchResults.Count > 0)
            {
                view.DisplaySearchResults(searchResults);
                view.ScrollSearchResultIntoView();
            }
            else
            {
                view.HandleNoSearchResultsFound();

            }
        }

        /// <summary>
        /// Gets the budget items by category and month for the specified time period if any and
        /// relates the information to the view for an interactive pie chart display where the 
        /// user can view each cumulative totals of each category or the totla of each category for
        /// each month individually.
        /// </summary>
        /// <param name="startDate"> The start date of the period that the user is viewing. </param>
        /// <param name="endDate"> The end of the period that the user is viewing. </param>
        /// <param name="filterflag"> Filter flag that is set to false as we want all categories. </param>
        /// <param name="categoryid"> Category id set to 0 as we do not need the category id.</param>
        public void GetSummariesByMonthAndCategoryForPieChart(DateTime? startDate = null, DateTime? endDate = null, bool filterflag=false, int categoryid = 0)
        {
            view.showPieChart(budget.GetBudgetDictionaryByCategoryAndMonth(startDate, endDate, filterflag, categoryid));

        }


    }
}
