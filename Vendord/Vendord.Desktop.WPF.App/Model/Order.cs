﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Vendord.Desktop.WPF.App.Properties;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace Vendord.Desktop.WPF.App.Model
{
    /// <summary>
    /// Represents an order.  It is wrapped
    /// by the OrderViewModel class, which enables it to
    /// be easily displayed and edited by a WPF user interface.
    /// </summary>
    public class Order
    {
        #region Creation

        public static Order CreateNewOrder()
        {
            return new Order();
        }

        public static Order CreateOrder(Guid id, string name)
        {
            Order order = new Order();
            order.Id = id;
            order.Name = name;
            return order;
        }

        protected Order()
        {
        }

        #endregion // Creation

        #region State Properties

        public Guid Id { get; set; }

        /// <summary>
        /// Gets/sets the order's name.
        /// </summary>
        public string Name { get; set; }     

        #endregion // State Properties        
    }
}
