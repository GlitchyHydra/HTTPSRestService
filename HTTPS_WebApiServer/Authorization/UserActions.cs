using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FreelancerWeb.Authorization
{
    [Flags]
    public enum UserActions
    {
        [Description("InsertOrder")]
        InsertOrder          = 1,    //Customer
        [Description("InsertApplication")]
        InsertApplication    = 2,    //Freelancer
        [Description("GetOrders")]
        GetOrders            = 4,    //Customer
        [Description("GetOpenedOrders")]
        GetOpenedOrders      = 8,    //Freelancer
        [Description("GetApplication")]
        GetApplications      = 16,   //Freelancer 
        [Description("GetOrderApplications")]
        GerOrderApplications = 32,   //Customer 
        [Description("AcceptApplication")]
        AcceptApplication    = 64,   //Customer 
        [Description("AcceptWork")]
        AcceptWork           = 128,  //Customer 
        [Description("DoneWork")]
        DoneWork             = 256,  //Freelancer 
        [Description("UpdateOrderStatus")]
        UpdateOrderStatus    = 512,  //Customer 
        [Description("GetWorks")]
        GetWorks             = 1024, //Freelancer 
        [Description("UpdateWorkStatus")]
        UpdateWorkStatus     = 2048, //Customer, Freelancer

        //All
        CustomerActions   = InsertOrder | GetOrders | GetOpenedOrders | AcceptApplication | AcceptWork | UpdateOrderStatus | UpdateWorkStatus,
        FreelancerActions = InsertApplication | GetOpenedOrders | GetApplications | DoneWork | GetWorks | UpdateWorkStatus
    }

}
