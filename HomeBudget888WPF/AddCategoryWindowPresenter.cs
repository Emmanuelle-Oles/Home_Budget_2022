using System;
using System.Collections.Generic;
using System.Text;
using Budget;
using System.IO;

namespace HomeBudget888WPF
{
    /// <summary>
    /// Gets and manipulates the data in a given home budget database file and communicates with the Add Category Window
    /// as well as the model to manage the displaying and manipulation of data.
    /// </summary>
    public class AddCategoryWindowPresenter
    {
        AddCategoryViewInterface view;
        HomeBudget budget;

        /// <summary>
        /// Instatiates a new presenter and retrieves the home budget at the specified file location.
        /// It also prepares the add category window by loading all of the home budget's category types
        /// into the add category form's category type dropdown menu.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="saveFileLocation"></param>
        /// <param name="newOrExisting"></param>
        public AddCategoryWindowPresenter(AddCategoryViewInterface view, string saveFileLocation, bool newOrExisting)
        {
            this.view = view;
            this.budget = new HomeBudget(saveFileLocation, newOrExisting);
            PrepareAddCategoryWindow();
        }

        /// <summary>
        /// Prepares the Add Category Window by loading all of the category types into the add category form
        /// dropdown menu.
        /// </summary>
        public void PrepareAddCategoryWindow()
        {
            view.LoadCategoryTypes();
        }

        /// <summary>
        /// Adds a new category to the categories in the home budget database based on the user
        /// input in the add category form. If the user input in the form is invalid (it matches
        /// either an existing category in the database or the input is invalid), the category is
        /// not added. Otherwise, the category is added to the current home budget database and the
        /// new category form inputs are reset.
        /// </summary>
        public void AddCategory()
        {
            string description = view.GetUserInputCategoryName();

            if (ValidateUserInputCategoryDescription(description))
            {
                int categoryTypeNumericValue = view.GetUserInputCategoryType();
                budget.categories.Add(description, (Category.CategoryType)categoryTypeNumericValue);
                view.ResetCategoryAddingForm();         
            }      
        }

        private bool ValidateUserInputCategoryDescription(string description)
        {
            if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description)) // will "    " be considered an empty string?
            {
                view.ShowInputError("Category name is invalid. The category name cannot be blank.");
                return false;
            }

            foreach (Category category in budget.categories.List())
            {
                if (category.Description.ToLower() == description.ToLower())
                {
                    view.ShowInputError($"Category name is invalid. There is already a category with the name {description} in the budget.");
                    return false;
                }
            }

            return true;
        }
    }
}
