﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceChat.Web.Models.Items
{
    public class AddItemModel
    {
        public string PartitionKey => "Product";

        [Required]
        public string Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Retailer { get; set; }

        public bool Success { get; set; }

        public IList<ValidationFailure> Errors { get; set; } = new List<ValidationFailure>();

        public List<SelectListItem> RetailerList => new List<SelectListItem>
        {
            new SelectListItem {Value = "Amazon", Text = "Amazon"},
            new SelectListItem {Value = "Argos", Text = "Argos"}
        };
    }
}