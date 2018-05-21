using System;

namespace Plutus.ETL.CS.Model
{
    public partial class BankEdit : IEquatable<BankEdit>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(BankEdit other)
        {
            if (other == null)
            {
                return false;
            }
            else if (this.EffectiveDate != other.EffectiveDate)
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
            int hashEffectiveDate = EffectiveDate == null ? 0 : EffectiveDate.GetHashCode();

            // Get the hash code for the FITID field. 
            int hashFITID = FITID.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashEffectiveDate ^ hashFITID;
        }
    }
}
