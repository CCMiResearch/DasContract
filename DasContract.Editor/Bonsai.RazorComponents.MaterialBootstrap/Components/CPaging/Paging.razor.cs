using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CPaging
{
    public partial class Paging: LoadableComponent
    {
        protected override async Task OnParametersSetAsync()
        {
            if (From >= TotalItemsCount && TotalItemsCount > 0)
                await ChangePageAsync(0);
        }

        /// <summary>
        /// Parameter telling how much items should be paged
        /// </summary>
        [Parameter]
        public int TotalItemsCount { get; set; }

        /// <summary>
        /// From what item should the paging be paging
        /// </summary>
        [Parameter]
        public int From { get; set; } = 0;

        /// <summary>
        /// Current total page count
        /// </summary>
        public int PagesCount
        {
            get
            {
                return (int)Math.Ceiling((double)TotalItemsCount / (double)ItemsPerPage);
            }
        }

        /// <summary>
        /// Last item index that should be displayed
        /// </summary>
        public int To
        {
            get
            {
                if (From + ItemsPerPage > TotalItemsCount)
                    return TotalItemsCount;
                return From + ItemsPerPage;
            }
        }

        /// <summary>
        /// How many items per page is displayed initially
        /// </summary>
        public static int InitialItemsPerPage { get; } = 10;

        /// <summary>
        /// How many items is displayed on one page
        /// </summary>
        [Parameter]
        public int ItemsPerPage { get; set; } = InitialItemsPerPage;

        /// <summary>
        /// Currently set page
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return From / ItemsPerPage;
            }
        }

        /// <summary>
        /// Changes page
        /// </summary>
        /// <param name="newPage">New target page</param>
        public async Task ChangePageAsync(int newPage)
        {
            From = ItemsPerPage * newPage;

            //Callback
            await OnChange.InvokeAsync(PagingData);
        }

        /// <summary>
        /// Moves to the previous page
        /// </summary>
        public async Task PreviousAsync()
        {
            await ChangePageAsync(CurrentPage - 1);
        }

        /// <summary>
        /// Moves to the next page
        /// </summary>
        public async Task NextAsync()
        {
            await ChangePageAsync(CurrentPage + 1);
        }

        /// <summary>
        /// Paging change callback
        /// </summary>
        [Parameter]
        public EventCallback<PagingData> OnChange { get; set; }

        /// <summary>
        /// Returns new paging data containing current paging state
        /// </summary>
        public PagingData PagingData
        {
            get
            {
                return new PagingData()
                {
                    CurrentPage = CurrentPage,
                    ItemsPerPage = ItemsPerPage,
                    From = From,
                    To = To,
                    PagesCount = PagesCount,
                    TotalItemsCount = TotalItemsCount
                };
            }
        }

        /// <summary>
        /// Gets previous buttons class
        /// </summary>
        protected string PreviousClass
        {
            get
            {
                if (CurrentPage == 0)
                    return "disabled";
                return "";
            }
        }

        /// <summary>
        /// Gets next buttons class
        /// </summary>
        protected string NextClass
        {
            get
            {
                if (CurrentPage == PagesCount - 1 || PagesCount == 0)
                    return "disabled";
                return "";
            }
        }
    }
}
