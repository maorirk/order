using OrderMonitor.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Events;
using GrpcClient;
using OrderMonitor.Models;
using System.Collections.ObjectModel;

namespace OrderMonitor.ViewModels
{
    class OrderMonitorViewModel : DependencyObject, INotifyPropertyChanged, IOrder
    {
        private object _currentViewModel;
        public event PropertyChangedEventHandler PropertyChanged;
        private string data;
        private ObservableCollection<OrderInfo> lstOrderInfo;
        private void PropertyChangedHandler(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public OrderMonitorViewModel(IEventAggregator eventAggregator)
        {
            CurrentViewModel = this;
            lstOrderInfo = new ObservableCollection<OrderInfo>();          
            StreamOrderHelper();             
        }

        private async Task StreamOrderHelper()
        {
            await StreamOrders();
        }     
        private async Task StreamOrders()
        {
            await foreach(var orderInfo in GrpcClient.Class1.GetStreamOrders())
            {
                OrderInfo order = new OrderInfo()
                {
                    OrderId = orderInfo.OrderId,
                    Symbol = orderInfo.Symbol,
                    Side = orderInfo.Side,
                    Client = orderInfo.Client,
                    LivePrice = orderInfo.LivePrice,
                    Qty = orderInfo.Qty,
                    Total = orderInfo.Total
                };
                lstOrderInfo.Add(order);
                LstOrderInfo = lstOrderInfo;
            }
        }             
        public ObservableCollection<OrderInfo> LstOrderInfo
        {
            get
            {
                return lstOrderInfo;
            }
            set
            {
                lstOrderInfo = value;
                PropertyChangedHandler(nameof(LstOrderInfo));
            }
        }
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                PropertyChangedHandler(nameof(Data));
            }
        }
        public object CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                PropertyChangedHandler(nameof(CurrentViewModel));
            }
        }
    
    }
}
