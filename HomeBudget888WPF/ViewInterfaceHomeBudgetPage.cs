using System;
using System.Collections.Generic;
using System.Text;
using Budget;

namespace HomeBudget888WPF
{
    public interface ViewInterfaceHomeBudgetPage
    {
        void ShowInputError(string message);
        void ShowAddCategoryWindow(string saveFileLocation, bool newOrExisting);
        void LoadCategories(List<Category> categories, Category dummyCategoryForNoCategoryFilterSelection);
        void LoadSummaries(string[] summaries);
        void LoadBudgetItems(List<BudgetItem> budgetItems);
        void LoadBudgetItemsByCategory(List<BudgetItemsByCategory> items);
        void LoadBudgetItemsByMonth(List<BudgetItemsByMonth> items);
        void LoadBudgetItemsByCategoryAndMonth(List<Dictionary<string, object>> items, List<Category> categories);
        void EnableSearchFeatureOnCategoriesDropdown();
        void ShowTodayDate();
        void Quit();
        void ClearExpenseForm();
        bool PromptUserWithYesOrNoQuestion(string question);
        void ShowCurrentBudgetFileName(string fileName);
        bool DatabaseEmpty(string fileName);
        void ChangeExpenseAddTitle(string title);
        void EnableContextMenuItems(bool enableOrDisable);
        void DisplaySearchResults(List<BudgetItem> items);
        void ScrollSearchResultIntoView();
        void HandleNoSearchResultsFound();
        void HighlightAddedExpenseIntoView();
        void HighlightEdittedExpenseIntoView();
        void showPieChart(List<Dictionary<string, object>> items);
        void GiveOptionForChart();
        void HideOptionForChart();
        void HideChart();
    }
}
