using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Utility
{
   public class SD
    {
        public const string Proc_CoverType_Create = "SP_CoverType_Create";
        public const string Proc_CoverType_Update = "SP_CoverType_Update";
        public const string Proc_CoverType_Delete = "SP_CoverType_Delete";
        public const string proc_CoverType_GetCoverTypes = "SP_CoverType_GetCoverTypes";
        public const string proc_CoverType_GetCoverType = "SP_CoverType_Get CoverType";

        //Roles

        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee User";
        public const string Role_Company = "Company User";
        public const string Role_Individual = "Individual User";
        //Session
        public const String Ss_Session = "My Session";
        
        //GetPriceBasedOnQuantity
        public static double GetPriceBasedOnQuantity(double Quantity, double Price, double Price50, double Price100)
        {
            if (Quantity < 50)
                return Price;
            else if (Quantity < 100)
                return Price50;
            else
                return Price100;
        }
        //ConvertToRawHTML
        public static string ConvertToRawHtml(string Source)
        {
            char[] Array = new char[Source.Length];
            int ArrayIndex = 0;
            bool Inside = false;
            for(int i=0; i < Source.Length; i++)
            {
                char let = Source[i];
                if (let == '<')
                {
                    Inside = true;
                    continue;
                }
                if (let == '>')
                {
                    Inside = false;
                    continue;
                }
                if (!Inside)
                {
                    Array[ArrayIndex] = let;
                    ArrayIndex++;
                }
            }
            return new string(Array, 0, ArrayIndex);
        }
        //OrderStatus
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProgress = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        //PaymentStatus
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "DelayPayment";
        public const string PaymentStatusRejected = "Rejected";
    }
}
