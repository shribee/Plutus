using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plutus.ETL.CS.Model
{
    /// <summary>
    /// Partial class to add overriding equality methods
    /// </summary>
    public partial class BankCaterAllen : IEquatable<BankCaterAllen>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(BankCaterAllen other)
        {
            if (other == null)
            {
                return false;
            }
            else if (this.FITID != other.FITID)
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
            int hashFITID = FITID == Convert.ToInt64(null) ? 0 : FITID.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashFITID;
        }
    }
}