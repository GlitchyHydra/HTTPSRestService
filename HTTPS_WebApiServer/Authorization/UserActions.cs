using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Models.DatabaseModels
{
    
    [Flags]
    public enum UserActions
    {
        InsertOrder          = 1,    //Customer
        InsertApplication    = 2,    //Freelancer
        GetOrders            = 4,    //Customer
        GetOpenedOrders      = 8,    //Freelancer   
        GetApplications      = 16,   //Freelancer
        GerOrderApplications = 32,   //Customer
        AcceptApplication    = 64,   //Customer
        AcceptWork           = 128,  //Customer
        DoneWork             = 256,  //Freelancer
        UpdateOrderStatus    = 512,  //Customer
        GetWorks             = 1024, //Freelancer
        UpdateWorkStatus     = 2048, //Customer, Freelancer

        //All
        CustomerActions   = InsertOrder | GetOrders | GetOpenedOrders | AcceptApplication | AcceptWork | UpdateOrderStatus | UpdateWorkStatus,
        FreelancerActions = InsertApplication | GetOpenedOrders | GetApplications | DoneWork | GetWorks | UpdateWorkStatus
    }
}
