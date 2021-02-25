﻿using DataLibrary.Data;
using DataLibrary.Db;
using DataLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPages.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        // a dropdownlist of items
        public List<SelectListItem> FoodItems { get; set; }

        // you can write to this only on POST
        [BindProperty]
        public OrderModel Order { get; set; }

        public CreateModel(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;

            FoodItems = new List<SelectListItem>();
        }


        public async Task OnGet()
        {
            var food = await _foodData.GetFood();            

            food.ForEach(x =>
            {
                FoodItems.Add(new SelectListItem { Value = x.Id.ToString(), Text = x.Title });
            });
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid == false)
            {
                return Page();
            }

            var food = await _foodData.GetFood();

            Order.Total = Order.Quantity * food.Where(x => x.Id == Order.FoodId).First().Price;

            int id = await _orderData.CreareOrder(Order);

            return RedirectToPage("./Create");
        }
    }
}