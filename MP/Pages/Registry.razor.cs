using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MP.Data;
using MP.Models;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace MP.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Registry : ComponentBase
    {
        private Table<Order> Table { get; set; } = default!;

        [Inject]
        [NotNull]
        private IStringLocalizer<Foo>? Localizer { get; set; }

        private readonly ConcurrentDictionary<Foo, IEnumerable<SelectedItem>> _cache = new();

        private IEnumerable<SelectedItem> GetHobbys(Foo item) => _cache.GetOrAdd(item, f => Foo.GenerateHobbys(Localizer));

        /// <summary>
        /// 
        /// </summary>
        //private static IEnumerable<int> PageItemsSource => new int[] { 100 };

        //private Task<QueryData<Order>> OnSearchModelQueryAsync(QueryPageOptions options)
        //{
        //    var items = MPContext.Orders.Where(options.ToFilterFunc<Order>());


        //    var total = items.Count();
        //    var result = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            
        //    return Task.FromResult(new QueryData<Order>()
        //    {
        //        Items = result,
        //        TotalCount = total,
        //        IsSorted = true,
        //        IsFiltered = true,
        //        IsSearch = true
        //    });
        //}

        private Task<QueryData<Order>> OnQueryAsync(QueryPageOptions options)
        {
            var items = MPContext.Orders.AsQueryable();

            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<Order>()).AsQueryable();
            }

            if (options.Searches.Any())
            {
                items = items.Where(options.Searches.GetFilterFunc<Order>(FilterLogic.Or)).AsQueryable();
            }

            var total = items.Count();
            //var result = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
            
            return Task.FromResult(new QueryData<Order>()
            {
                Items = items, //result,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            });
        }

        private Task OnEditAsync(Order order)
        {
            var record = MPContext.Orders.FirstOrDefault(_order => _order.Id == order.Id);

            if (record == null)
                return Task.CompletedTask;

            record.Year = order.Year;
            record.Quarter = order.Quarter;
            record.OrderNumber = order.OrderNumber;
            record.ReleaseMonth = order.ReleaseMonth;
            record.ProductName = order.ProductName;
            record.Quantity = order.Quantity;
            record.Price = order.Price;

            return Task.CompletedTask;// MPContext.SaveChangesAsync();
        }
    }
}
