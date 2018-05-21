using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plutus.ETL.CS.Model
{
    public partial class ExpenseEdit : IEquatable<ExpenseEdit>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(ExpenseEdit other)
        {
            if (other == null)
            {
                return false;
            }
            else if (this.Date != other.Date)
            {
                return false;
            }
            else if (this.Description != other.Description)
            {
                return false;
            }
            else if (this.Detail != other.Detail)
            {
                return false;
            }
            else if (this.Total != other.Total)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Hash code for the transaction for equality checks
        /// </summary>
        public override int GetHashCode()
        {

            // Get the hash code for the Date field if it is not null. 
            int hashDate = Date == null ? 0 : Date.GetHashCode();

            // Get the hash code for the Description field. 
            int hashDescription = Description.GetHashCode();

            // Get the hash code for the UserComments field. 
            int hashDetail = Detail.GetHashCode();

            // Get the hash code for the Amount field. 
            int hashTotal = Total.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashDate ^ hashDescription ^ hashDetail ^ hashTotal;
        }
    }

}
