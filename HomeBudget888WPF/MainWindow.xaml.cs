using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Budget;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls.DataVisualization.Charting;
using System.Media;

namespace HomeBudget888WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml. Displays expenses information for the currently opened
    /// home budget file and allows for manipulation of its data through a user interface.
    /// </summary>

    public partial class MainWindow : Window, ViewInterfaceHomeBudgetPage, ShowRecentInterface
    {

        private readonly HomeBudgetWindowPresenter presenter;
        private readonly RecentItemsPresenter recentItemsPresenter;
        private const string PLACEHOLDER_TEXT = "Search...";
        private List<int> indexList;
        private int currentIndex;
        private int selectedIndex = -1;

        /// <summary>
        /// Instantiates a new Main Window and calls the <see cref="HomeBudgetWindowPresenter">HomeBudgetWindowPresenter</see>
        /// constructor to instantiate an instance of home budget, passing in the current main window, 
        /// the budget file name and whether or not the database is new
        /// </summary>
        /// <param name="budgetFilename">The current home budget's filepath.</param>
        /// <param name="newOrExisting">Whether or not the specified budget file is of a new or an existing database.</param>
        public MainWindow(string budgetFilename, bool newOrExisting)
        {
            InitializeComponent();
            this.recentItemsPresenter = new RecentItemsPresenter(this);
            presenter = new HomeBudgetWindowPresenter(this, budgetFilename, newOrExisting, recentItemsPresenter);
        }

        /// <summary>
        /// Displays all of the categories present in the current budget file in a dropdown menu
        /// in the main window.
        /// The categorie's description is displayed as the information in the dropdown.
        /// </summary>
        /// <param name="categories">The list of categories in the budget to display.</param>
        public void LoadCategories(List<Category> categories, Category dummyCategoryForNoCategoryFilterSelection)
        {
            cmbCategories.DisplayMemberPath = "Description";

            foreach (Category category in categories)
                cmbCategories2.DisplayMemberPath = "Description";  //combo box in the filter part of the page


            foreach (Category category in categories)
            {
                cmbCategories.Items.Add(category);

                // combo box in the filter part of page
                cmbCategories2.Items.Add(category);
            }

            // Option 1
            cmbCategories2.Items.Add(dummyCategoryForNoCategoryFilterSelection);
            cmbCategories2.SelectedIndex = cmbCategories2.Items.Count - 1;

            //// Option 2
            //ComboBoxItem item = new ComboBoxItem();//cmbCategories2.Items.GetItemAt(indexOfDefaultFilterNoCategory);
            //item.Content = "No Category Filter";
            //cmbCategories2.Items.Add(item);

            //int indexOfDefaultFilterNoCategory = cmbCategories2.Items.Count - 1;
            //cmbCategories2.SelectedIndex = indexOfDefaultFilterNoCategory;
        }

        /// <summary>
        /// Displays the summaries in the specified array in the filter section summary combobox. The summaries
        /// refer to the different summary/budget item grouping options available to the user.
        /// </summary>
        /// <param name="summaries">The array containing the summaries to display.</param>
        public void LoadSummaries(string[] summaries)
        {
            foreach (string summary in summaries)
            {
                cmbSummary.Items.Add(summary);

                if(cmbSummary.Items.Count == summaries.Length)
                {
                    cmbSummary.SelectedIndex = cmbSummary.Items.Count - 1; 
                }
            }
        }

        private void LoadBudgetItemsInDataGrid(List<BudgetItem> budgetItems)
        {
            try
            {
                budgetItemsDataGrid.ItemsSource = budgetItems;
                budgetItemsDataGrid.Columns.Clear();

                Style rightAligned = new Style();
                rightAligned.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

                var dateColumn = new DataGridTextColumn();
                dateColumn.Header = "Date";
                dateColumn.Binding = new Binding("Date");
                dateColumn.Binding.StringFormat = "yyyy-MM-dd";
                budgetItemsDataGrid.Columns.Add(dateColumn);

                var categoryColumn = new DataGridTextColumn();
                categoryColumn.Header = "Category";
                categoryColumn.Binding = new Binding("Category");
                budgetItemsDataGrid.Columns.Add(categoryColumn);

                var descriptionColumn = new DataGridTextColumn();
                descriptionColumn.Header = "Description";
                descriptionColumn.Binding = new Binding("ShortDescription");
                budgetItemsDataGrid.Columns.Add(descriptionColumn);

                var amountColumn = new DataGridTextColumn();
                amountColumn.Header = "Amount";
                amountColumn.Binding = new Binding("Amount");
                amountColumn.Binding.StringFormat = "$0.00";
                amountColumn.CellStyle = rightAligned;
                budgetItemsDataGrid.Columns.Add(amountColumn);

                var balanceColumn = new DataGridTextColumn();
                balanceColumn.Header = "Balance";
                balanceColumn.Binding = new Binding("Balance");
                balanceColumn.Binding.StringFormat = "$0.00";
                balanceColumn.CellStyle = rightAligned;
                budgetItemsDataGrid.Columns.Add(balanceColumn);
            }
            catch (Exception) { }

        }

        /// <summary>
        /// Displays all of the budget items present in the current budget file.
        /// </summary>
        /// <param name="budgetItems">The budget items containing the expenses to display.</param>
        public void LoadBudgetItems(List<BudgetItem> budgetItems)
        {
            LoadBudgetItemsInDataGrid(budgetItems);
        }

        /// <summary>
        /// Displays the specified list of BudgetItemsByCategory by biding each one of its items to the expenses data grid.
        /// Each record in the BudgetItemsByCategory list is a group of home budget budget items grouped by category.
        /// Each BudgetItemsByCategory list record also contains the total amount balance for its group.
        /// </summary>
        /// <param name="items">The budget items grouped by their month and then their year.</param>
        public void LoadBudgetItemsByCategory(List<BudgetItemsByCategory> items)
        {
            budgetItemsDataGrid.ItemsSource = items;
            budgetItemsDataGrid.Columns.Clear();

            Style rightAligned = new Style();
            rightAligned.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

            var categoryColumn = new DataGridTextColumn();
            categoryColumn.Header = "Category";
            categoryColumn.Binding = new Binding("Category");
            budgetItemsDataGrid.Columns.Add(categoryColumn);

            var totalColumn = new DataGridTextColumn();
            totalColumn.Header = "Total";
            totalColumn.Binding = new Binding("Total");
            totalColumn.Binding.StringFormat = "$0.00";
            totalColumn.CellStyle = rightAligned;
            budgetItemsDataGrid.Columns.Add(totalColumn);
        }

        /// <summary>
        /// Displays the specified list of BudgetItemsByMonth by biding each one of its items to the expenses data grid.
        /// Each record in the BudgetItemsByMonth list is a group of home budget budget items grouped by their month and year.
        /// Each BudgetItemsByMonth list record also contains the total amount balance for its group.
        /// </summary>
        /// <param name="items">The budget items grouped by their month and then their year.</param>

        public void LoadBudgetItemsByMonth(List<BudgetItemsByMonth> items)
        {
            budgetItemsDataGrid.ItemsSource = items;
            budgetItemsDataGrid.Columns.Clear();

            Style rightAligned = new Style();
            rightAligned.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

            var monthColumn = new DataGridTextColumn();
            monthColumn.Header = "Month";
            monthColumn.Binding = new Binding("Month");
            monthColumn.Binding.StringFormat = "yyyy-MM-dd";
            budgetItemsDataGrid.Columns.Add(monthColumn);

            var totalColumn = new DataGridTextColumn();
            totalColumn.Header = "Total";
            totalColumn.Binding = new Binding("Total");
            totalColumn.Binding.StringFormat = "$0.00";
            totalColumn.CellStyle = rightAligned;
            budgetItemsDataGrid.Columns.Add(totalColumn);
        }

        /// <summary>
        /// Sets the selected date of the DatePicker in the Main Window's
        /// new expense form to the current date.
        /// </summary>
        public void ShowTodayDate()
        {
            dpExpenseDate.SelectedDate = DateTime.Today;
        }

        /// <summary>
        /// Clears the name in the expense name textbox, the amount in the expense amount
        /// textbox and unchecks the credit card checkbox in the Main Window's new expense form.
        /// The DatePicker selected date and the selected Category are not effected.
        /// </summary>
        public void ClearExpenseForm()
        {
            tbExpenseName.Text = string.Empty;
            tbExpenseAmount.Text = string.Empty;
            // does a clear revert to no category or does it keep selected category?
            chkCreditExpense.IsChecked = false;
            //clear the summary
            cmbSummary.SelectedItem = "No Summary";
            presenter.ClearExpenseFormEditMode();
        }

        /// <summary>
        /// Opens a new <see cref="AddCategoryWindow">AddCategoryWindow</see> window by 
        /// instantiating a new instance of AddCategoryWindow and passing in the current
        /// Main Window instance, the specified filepath of the current database file, 
        /// and the specified state of the database (whether it is a new database or not). 
        /// The AddCategoryWindow allows to add new categories to the current home budget.
        /// The Main Window can not be interacted with until the AddCategoryWindow is
        /// interacted with and closed.
        /// </summary>
        /// <param name="saveFileLocation">The specified file location of the current home 
        /// budget database file.</param>
        /// <param name="newOrExisting">Whether or not the specified current home budget 
        /// database file is new or not.</param>
        public void ShowAddCategoryWindow(string saveFileLocation, bool newOrExisting)
        {
            AddCategoryWindow newCategoryWindow = new AddCategoryWindow(this, saveFileLocation, newOrExisting);
            newCategoryWindow.ShowDialog();
        }

        /// <summary>
        /// Closes the current Main Window.
        /// </summary>
        public void Quit()
        {
            this.Close();
        }

        /// <summary>
        /// Gets the user's input in the amount textbox in the Main Window's new expense form.
        /// </summary>
        /// <returns>The string representation of the amount input by the user.
        /// </returns>
        private string GetUserInputExpenseAmount()
        {
            string amount = tbExpenseAmount.Text;

            return amount;
        }

        /// <summary>
        /// Gets the boolean representation of the state of the credit expense checkbox in the
        /// Main Window's new expense form. True if the checkbox is checked; False otherwise.
        /// </summary>
        /// <returns>Returns true if the checkbox is checked; False otherwise.</returns>
        private bool CheckIfCreditExpenseIsChecked()
        {
            if ((bool)chkCreditExpense.IsChecked)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the Category ID of the Category selected in the categories 
        /// drop down menu in the Main Window's new expense form. If the
        /// selected Category has been typed in by the user and is not a Category object
        /// yet, the presenter <see cref="HomeBudgetWindowPresenter.AddCategoryUsingComboBox(string)">
        /// AddCategoryUsingComboBox</see> method is called passing in the ComboBox input text
        /// </summary>
        /// <returns></returns>

        private int GetUserInputExpenseCategory()
        {
            int categoryId = 1;

            if (cmbCategories.SelectedItem is Category)
            {
                categoryId = (cmbCategories.SelectedItem as Category).Id;
            }
            else
            {
                presenter.AddCategoryUsingComboBox(cmbCategories.Text);
            }

            return categoryId;
        }

        private string GetUserInputExpenseDescription()
        {
            return tbExpenseName.Text;
        }

        private DateTime GetUserInputExpenseDate()
        {
            return (DateTime)dpExpenseDate.SelectedDate;
        }

        /// <summary>
        /// Displays an invalid input error MessageBox to the user explaining the 
        /// input error in the new expense Main Window form.
        /// </summary>
        /// <param name="message">The message detailling the invalid input to be displayed in the MessageBox.</param>
        public void ShowInputError(string message)
        {
            MessageBox.Show(message, "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Prompts the user with the specified question in a MessageBox. Gives the user 
        /// the answer to respond with either yes or no. 
        /// </summary>
        /// <param name="question">The question to be displayed in the prompt MessageBox.</param>
        /// <returns>True if the user responds to the message with 'Yes'; False otherwise.</returns>
        public bool PromptUserWithYesOrNoQuestion(string question)
        {
            MessageBoxResult result = MessageBox.Show(question, "Action Required", MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Sets the tab header of the list of expenses data grid to the specified current
        /// home budget database file's filename.
        /// </summary>
        /// <param name="filename">The filename to display in the tab header (the current home budget's filename).</param>
        public void ShowCurrentBudgetFileName(string filename)
        {
            tiBudgetFilename.Header = filename;
        }

        /// <summary>
        /// Enables the autofill and ability to edit on the Main Window categories dropdown
        /// in the new expense form. Must be done after the <see cref="LoadCategories(List{Category})">
        /// LoadCategoriesMethodIsDone</see> so that all of the current home budget's categories are 
        /// already loaded in the dropdown.
        /// </summary>
        public void EnableSearchFeatureOnCategoriesDropdown()
        {
            cmbCategories.IsTextSearchEnabled = true;
            cmbCategories.IsEditable = true;

            //combo box in the filer - im not sure if this will work since its not editable
            cmbCategories2.IsTextSearchEnabled = true;
        }

        /// <summary>
        /// Prompts the user asking if they want to overwrite an empty database file.
        /// </summary>
        /// <param name="fileName">The file name of the file that is empty.</param>
        /// <returns>Returns true if the user does want to overwrite the file; False otherwise.</returns>
        public bool DatabaseEmpty(string fileName)
        {
            MessageBoxResult result =
            MessageBox.Show("This is an empty database. Would you like to use this file? ", "Empty database", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnClearForm_Click(object sender, RoutedEventArgs e)
        {
            ClearExpenseForm();
        }

        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog newBudgetFileWindow = new SaveFileDialog();

            newBudgetFileWindow.InitialDirectory = System.IO.Path.GetFullPath(WelcomePage.DEFAULT_STORAGE_FOLDER);

            newBudgetFileWindow.Filter = "DB Files | *.db";

            if (newBudgetFileWindow.ShowDialog() == true)
            {
                presenter.CreateNewBudget(newBudgetFileWindow.FileName);
            }
        }

        private void brnAddExpense_Click(object sender, RoutedEventArgs e)
        {
            string userInputAmount = GetUserInputExpenseAmount();
            string description = GetUserInputExpenseDescription();
            DateTime date = GetUserInputExpenseDate();
            int categoryId = GetUserInputExpenseCategory();
            bool creditExpenseIsChecked = CheckIfCreditExpenseIsChecked();

            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;
            int categoryIdToFilterBy = (cmbCategories2.SelectedItem as Category).Id;

            presenter.AddExpense(userInputAmount, description, date, categoryId, creditExpenseIsChecked, startDate, endDate, categoryIdToFilterBy);            
        }
        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            presenter.Quit();
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddCategory();
        }

        // tool bar related logic
        private void menuHome_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
            MessageBox.Show("You are about to leave this page to go to the home page. Are you sure? ", "Leaving Main Window", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                WelcomePage welcomePage = new WelcomePage();
                presenter.ReleaseConnection();
                this.Close();
                welcomePage.ShowDialog();
            }
        }

        private void menuBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openExistingFileWindow = new OpenFileDialog();
            openExistingFileWindow.Filter = "DB Files | *.db";

            if (openExistingFileWindow.ShowDialog() == true)
            {
                presenter.OpenExistingHomeBudgetFromMainWindow(openExistingFileWindow.FileName);
            }
        }
        private void menuExitApplication_Click(object sender, RoutedEventArgs e)
        {
            presenter.Quit();
        }

        /// <summary>
        /// Changes the add new expense section title's string to the specified string.
        /// </summary>
        /// <param name="title">The string to change the add new expense section title to.s</param>
        public void ChangeExpenseAddTitle(string title)
        {
            expenseAddTitle.Text = title;
        }

        private void Modify_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem item = budgetItemsDataGrid.SelectedItem as BudgetItem;
            selectedIndex = budgetItemsDataGrid.SelectedIndex;

            if (item != null)
            {
                DateTime? startDate = dpStartDate.SelectedDate;
                DateTime? endDate = dpEndDate.SelectedDate;
                int categoryId = (cmbCategories2.SelectedItem as Category).Id;

                tbExpenseName.Text = item.ShortDescription;
                dpExpenseDate.SelectedDate = item.Date;
                tbExpenseAmount.Text = item.Amount.ToString("$0.00");
                cmbCategories.SelectedIndex = item.CategoryID - 1;

                presenter.ModifyItem(item, startDate, endDate, categoryId);
            }
            else
            {
                MessageBox.Show("Cannot edit this is is not a budget item!");
            }
        }

        private void Delete_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem itemToDelete = budgetItemsDataGrid.SelectedItem as BudgetItem;
            int index = budgetItemsDataGrid.SelectedIndex;

            if (itemToDelete != null)
            {
                DateTime? startDate = dpStartDate.SelectedDate;
                DateTime? endDate = dpEndDate.SelectedDate;
                int categoryId = (cmbCategories2.SelectedItem as Category).Id;

                presenter.DeleteItem(itemToDelete, startDate, endDate, categoryId);
                budgetItemsDataGrid.SelectedIndex = budgetItemsDataGrid.Items.Count == 0 ? -1 : budgetItemsDataGrid.Items.Count == index ? index - 1 : index;
            }
            else
            {
                MessageBox.Show("Cannot delete this it is not a budget item!");
            }
        }


        /// <summary>
        /// Loads and displays the total expense amount for each month group for each category in the data grid. The data grid 
        /// values are defined in the specified list of key value pairs that represent the budget items grouped by their
        /// category and their month group (first month, then year). The items are loaded into the data grid by binding the values
        /// in the specified key value pairs with their keys (categories). The grand total of all of the expense amounts by category
        /// across months is also bound into the data grid as well as the grand total for each month group accross all categories.
        /// </summary>
        /// <param name="items">The list of key value pairs representing the category and month budget item summaries.</param>
        /// <param name="categories">The list of category object in the home budget.</param>
        public void LoadBudgetItemsByCategoryAndMonth(List<Dictionary<string, object>> items, List<Category> categories)
        {
            budgetItemsDataGrid.ItemsSource = items;
            budgetItemsDataGrid.Columns.Clear();

            var monthColumn = new DataGridTextColumn();
            monthColumn.Header = "Month";
            monthColumn.Binding = new Binding("[Month]");
            budgetItemsDataGrid.Columns.Add(monthColumn);

            Style rightAligned = new Style();
            rightAligned.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

            // we could also probably use the key to avoid passing in the list of categories
            foreach (Category category in categories)
            {
                var column = new DataGridTextColumn();
                column.Header = category.Description;
                column.Binding = new Binding($"[{category.Description}]");
                column.Binding.StringFormat = "$0.00";
                column.CellStyle = rightAligned;
                budgetItemsDataGrid.Columns.Add(column);
            }

            var totalColumn = new DataGridTextColumn();
            totalColumn.Header = "Total";
            totalColumn.Binding = new Binding("[Total]");
            totalColumn.Binding.StringFormat = "$0.00";
            totalColumn.CellStyle = rightAligned;
            budgetItemsDataGrid.Columns.Add(totalColumn);
        }

        /// <summary>
        /// Toggles the enabled preoperty on the Delete and Modify DataGrid context menu items and the search bar depending
        /// on the specified boolean.
        /// </summary>
        /// <param name="enableOrDisable">True if the context menu items should be enabled; False otherwise.</param>
        public void EnableContextMenuItems(bool enableOrDisable)
        {
            miDelete.IsEnabled = enableOrDisable;
            miModify.IsEnabled = enableOrDisable;
            tbSearch.IsEnabled = enableOrDisable;
        }

        private void ChangeFilters(object sender, SelectionChangedEventArgs e)
        {
            if(presenter != null)
            {
                DateTime? startDate = dpStartDate.SelectedDate;
                DateTime? endDate = dpEndDate.SelectedDate;
                int categoryId = (cmbCategories2.SelectedItem as Category).Id;
                string summary = cmbSummary.SelectedItem.ToString();

                presenter.GateFilterRequest(startDate, endDate, categoryId, summary);
            }
        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            if(sender is MenuItem)
            {
                string filepath = (sender as MenuItem).Header.ToString();
                presenter.OpenExistingHomeBudgetFromMainWindow(filepath);
            }
        }


        /// <summary>
        /// Displays recently accessed files in MainWindow task bar by displaying all of the items in the specified
        /// string array in a menu item under the Recents menu item.
        /// </summary>
        /// <param name="data">An array containing the string representations of the recently accessed files.</param>
        public void ShowRecent(string[] data)
        {
            miRecent.Items.Clear();

            for (int i = 0; i < data.Length; i++)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = data[i];
                menuItem.Click += OpenFile;

                miRecent.Items.Add(menuItem);
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbSearch.Text == string.Empty)
            {
                ResetSearchResultDisplay();
            }
            else if(tbSearch.Text != PLACEHOLDER_TEXT)
            {
                tbSearch.Foreground = Brushes.Black;
                presenter.SearchInList(tbSearch.Text);
            }
        }

        private void ResetSearchResultDisplay()
        {
            indexList = null;
            currentIndex = 0;
            budgetItemsDataGrid.UnselectAllCells();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceSearchResult();
        }

        private void AdvanceSearchResult()
        {
            if(indexList != null)
            {
                currentIndex = (currentIndex + 1) % indexList.Count; // makes sure that the search result wrap index is always between 0 and search results.length - 1
                budgetItemsDataGrid.SelectedIndex = indexList[currentIndex];
                ScrollSearchResultIntoView();
            }
        }

        /// <summary>
        /// Displays the specified list of budget items which represent all of the home budget items that's description
        /// contains the search string. Uses the specified list of budget items and creates a list of all of the budget 
        /// items's indexes in the data grid. It then selects and highlights the first data grid budget item 
        /// that's description contains the search string and scrolls it into view.
        /// </summary>
        /// <param name="items">The home budget budget items who's short description contains the search bar search string.</param>
        public void DisplaySearchResults(List<BudgetItem> items)
        {
            indexList = new List<int>();
            currentIndex = 0;

            for (int i = 0; i < items.Count; i++)
            {
                for(int j = 0; j < budgetItemsDataGrid.Items.Count; j++)
                {
                    BudgetItem datagridItem = budgetItemsDataGrid.Items.GetItemAt(j) as BudgetItem;

                    if (datagridItem != null)
                    {
                        if(datagridItem.ShortDescription == items[i].ShortDescription)
                        {
                            indexList.Add(j);
                        }
                    }
                }
            }

            budgetItemsDataGrid.SelectedIndex = indexList[currentIndex];        
        }

        /// <summary>
        /// Scrolls the selected budget items data grid element into view.
        /// </summary>
        public void ScrollSearchResultIntoView()
        {
            budgetItemsDataGrid.ScrollIntoView(budgetItemsDataGrid.SelectedItem);

        }

        /// <summary>
        /// 
        /// </summary>
        public void HandleNoSearchResultsFound()
        {
            ResetSearchResultDisplay();
            MakeSoundIndicationNothingFoundInSearch();

        }

        private void MakeSoundIndicationNothingFoundInSearch()
        {
            SystemSounds.Asterisk.Play();

        }

        /// <summary>
        /// Highlights the last budget item in the data grid and scrolls it into view. Used to
        /// highlight and scroll into view a budget item datagrid element after it has been added to the data grid.
        /// </summary>
        public void HighlightAddedExpenseIntoView()
        {
            budgetItemsDataGrid.SelectedIndex = budgetItemsDataGrid.Items.Count - 1;
        }

        /// <summary>
        /// Highlights the budget item data grid that has been selected and editted.
        /// </summary>
        public void HighlightEdittedExpenseIntoView()
        {
            budgetItemsDataGrid.SelectedIndex = selectedIndex;
        }

        private void chPieCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // getting the time period the user would like view -- not sure if this is necessary
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;
            string categoryAndMonthSummary = cmbSummary.SelectedItem as string;


            if (categoryAndMonthSummary == "Category & Month")
            {
                budgetItemsDataGrid.Visibility = budgetItemsDataGrid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                chPie.Visibility = chPie.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;


                if (chPie.Visibility == Visibility.Visible)
                {

                    presenter.GetSummariesByMonthAndCategoryForPieChart(startDate, endDate, false, 0);

                }
            }
        }

        // this is empty so that nothing happens when the box get unchecked by the user
        private void chPieCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HideChart();

        }


        /// <summary>
        /// Displays a pie charts of the "By Month & Category" summary. The chart overlaps on the datagrid
        /// allows for a full view with a color coded legend of the the different categories accompanied with
        /// a dropdown meny of the months that are available to view.
        /// </summary>
        /// <param name="items">The list of key value pairs representing the category and month budget item summaries.</param>
        public void showPieChart(List<Dictionary<string, object>> items)
        {

            List<string> monthsAvailable = new List<string>();

            foreach (Category item in cmbCategories.Items)
                monthsAvailable.Add(item.Description);

            chPie.InitializeByCategoryAndMonthDisplay(monthsAvailable);
            chPie.DataSource = budgetItemsDataGrid.Items.Cast<object>().ToList();

        }

        /// <summary>
        /// Displays the title and the chekcbox to indicate to the user that they can choose
        /// to generate a chart.
        /// </summary>
        public void GiveOptionForChart()
        {
            tbPieChart.Visibility = Visibility.Visible;
            chPieCheckBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the option to display the chart and unchecks the checkbox.
        /// </summary>
        public void HideOptionForChart()
        {
            tbPieChart.Visibility = Visibility.Hidden;
            chPieCheckBox.IsChecked = false;
            chPieCheckBox.Visibility = Visibility.Hidden;
            
        }

        /// <summary>
        /// Hides the chart and diplays the data grid with the current budget items.
        /// </summary>
        public void HideChart()
        {
            chPie.Visibility = Visibility.Hidden;
            budgetItemsDataGrid.Visibility = Visibility.Visible;

        }
    }
}
