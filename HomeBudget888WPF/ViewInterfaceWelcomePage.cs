using System;
using System.Collections.Generic;
using System.Text;
using Budget;

namespace HomeBudget888WPF
{
    public interface ViewInterfaceWelcomePage
    {
        void CreateNewBudgetFile();
        void OpenExistingBudgetFile();
        void ShowError(string message);
        bool DatabaseEmpty(string fileName);
        void OpenMainWindow(string saveFileLocation, bool newOrExisting);

    }
}
