﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 异常信息类扩展方法
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rex"></param>
        /// <returns></returns>
        public static string GetFullErrorText(this ReflectionTypeLoadException rex)
        {
            var msg = string.Empty;
            foreach (var ex in rex.LoaderExceptions)
            {
                msg += string.Format("Exception: {0}", ex.Message) + Environment.NewLine;
            }
            return msg;
        }
    }
}
