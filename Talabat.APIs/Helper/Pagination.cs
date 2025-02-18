﻿using Talabat.APIs.DTOs;

namespace Talabat.APIs.Helper
{
    public class Pagination <T>
    {
       

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyCollection<T> Data { get; set; }


        public Pagination(int pageSize, int pageIndex,int count, IReadOnlyCollection<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }
    }
}
