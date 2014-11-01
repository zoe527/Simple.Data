﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Simple.Data.Operations;

namespace Simple.Data.Commands
{
    class FindCommand : ICommand, IQueryCompatibleCommand
    {
        /// <summary>
        /// Determines whether the instance is able to handle the specified method.
        /// </summary>
        /// <param name="method">The method name.</param>
        /// <returns>
        /// 	<c>true</c> if the instance is able to handle the specified method; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCommandFor(string method)
        {
            return method.Equals("find", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="dataStrategy">The database (or transaction)</param>
        /// <param name="table"></param>
        /// <param name="binder">The binder from the <see cref="DynamicTable"/> method invocation.</param>
        /// <param name="args">The arguments from the <see cref="DynamicTable"/> method invocation.</param>
        /// <returns></returns>
        public object Execute(DataStrategy dataStrategy, DynamicTable table, InvokeMemberBinder binder, object[] args)
        {
            if (args.Length == 1 && args[0] is SimpleExpression)
            {
                return new SimpleQuery(dataStrategy, table.GetName()).Where((SimpleExpression) args[0]).Singleton();
            }

            throw new BadExpressionException("Find only accepts a criteria expression.");
        }

        public object Execute(DataStrategy dataStrategy, SimpleQuery query, InvokeMemberBinder binder, object[] args)
        {
            return query.Where((SimpleExpression) args[0]).Take(1).FirstOrDefault();
        }
    }
}
