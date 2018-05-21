using System;

namespace Plutus.ETL.CS.Model
{
    public partial class Bank : IEquatable<Bank>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(Bank other)
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