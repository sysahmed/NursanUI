﻿namespace Nursan.ORM.Result
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

    }
}
