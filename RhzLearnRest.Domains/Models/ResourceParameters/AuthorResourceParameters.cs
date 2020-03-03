﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RhzLearnRest.Domains.Models.ResourceParameters
{
    public class AuthorResourceParameters
    {
        const int maxPageSize = 20;
        private int _pageSize = 10;
        public string MainCategory { get; set; }
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { 
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
