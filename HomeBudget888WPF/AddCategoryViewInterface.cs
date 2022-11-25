using System;
using System.Collections.Generic;
using System.Text;
using Budget;

namespace HomeBudget888WPF
{
    public interface AddCategoryViewInterface
    {
        string GetUserInputCategoryName();
        int GetUserInputCategoryType();
        void LoadCategoryTypes();
        void ResetCategoryAddingForm();
        void ShowInputError(string message);
    }
}
