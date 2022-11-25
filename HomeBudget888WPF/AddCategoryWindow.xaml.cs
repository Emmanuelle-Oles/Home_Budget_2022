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

namespace HomeBudget888WPF
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml. Allows for manipulation (adding categories) of the 
    /// home budget's category data through a user interface.
    /// </summary>
    public partial class AddCategoryWindow : Window, AddCategoryViewInterface
    {
        private readonly AddCategoryWindowPresenter presenter;

        /// <summary>
        /// Instantiates a new AddCategoryWindow and calls the <see cref="AddCategoryWindowPresenter">AddCategoryWindowPresenter</see>
        /// constructor to instantiate an instance of home budget of the current database file in order to add categories to it , 
        /// passing in the current add category window, the budget file name and whether or not the database is new.
        /// </summary>
        /// <param name="budgetFilename">The current home budget's filepath.</param>
        /// <param name="newOrExisting">Whether or not the specified budget file is of a new or an existing database.</param>
        public AddCategoryWindow(ViewInterfaceHomeBudgetPage view, string saveFileLocation, bool newOrExisting)
        {
            InitializeComponent();
            presenter = new AddCategoryWindowPresenter(this, saveFileLocation, newOrExisting);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddCategory();
        }

        /// <summary>
        /// Gets the user's input for the new category name in the add category form.
        /// </summary>
        /// <returns>The user's input in the new category name textbox.</returns>
        public string GetUserInputCategoryName()
        {
            return txtCategoryName.Text;
        }

        /// <summary>
        /// Gets the category type that the user selected in the category type dropdown menu.
        /// </summary>
        /// <returns>The numeric value of the category type that the user selected.</returns>
        public int GetUserInputCategoryType()
        {
            return (int)cmbCategoryTypes.SelectedItem;
        }

        /// <summary>
        /// Loads all of the HomeBudget class' category types in the combobox dropdown menu in the add
        /// category form.
        /// </summary>
        public void LoadCategoryTypes()
        {
            bool firstElement = true;

            foreach(Category.CategoryType categoryType in Enum.GetValues(typeof(Category.CategoryType)))
            {
                cmbCategoryTypes.Items.Add(categoryType);

                if (firstElement)
                {
                    cmbCategoryTypes.SelectedItem = categoryType;
                    firstElement = false;
                }
            }
        }

        /// <summary>
        /// Sets the new category name textbox description to an empty string.
        /// </summary>
        public void ResetCategoryAddingForm()
        {
            txtCategoryName.Text = string.Empty;
        }

        /// <summary>
        /// Displays an invalid input error MessageBox to the user explaining the 
        /// input error in the new category form.
        /// </summary>
        /// <param name="message">The message detailling the invalid input to be displayed in the MessageBox.</param>
        public void ShowInputError(string message)
        {
            MessageBox.Show(message, "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
