using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plutus.ETL.CS.Model
{
    public partial class BankIntouch : IEquatable<BankIntouch>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(BankIntouch other)
        {
            if (other == null)
            {
                return false;
            }
            else if (this.Date != other.Date)
            {
                return false;
            }
            else if (this.BankDescription != other.BankDescription)
            {
                return false;
            }
            else if (this.Description != other.Description)
            {
                return false;
            }
            else if (this.UserComments != other.UserComments)
            {
                return false;
            }
            else if (this.Amount != other.Amount)
            {
                return false;
            }
            else if (this.Net != other.Net)
            {
                return false;
            }
            else if (this.VAT != other.VAT)
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

            // Get the hash code for the BankDescription field. 
            int hashBankDescription = BankDescription.GetHashCode();

            // Get the hash code for the Description field. 
            int hashDescription = Description.GetHashCode();

            // Get the hash code for the UserComments field. 
            int hashUserComments = UserComments.GetHashCode();

            // Get the hash code for the Amount field. 
            int hashAmount = Amount.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashDate ^ hashBankDescription ^ hashDescription ^ hashUserComments ^ hashAmount;
        }
    }
}
