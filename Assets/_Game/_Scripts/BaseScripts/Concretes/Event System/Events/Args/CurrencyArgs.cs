using System;

namespace PAG.Events
{
    /// <summary>
    /// A custom event arguments class used to represent changes to a currency value.
    /// </summary>
    public class CurrencyArgs : EventArgs
    {
        /// <summary>
        /// The unique ID of the currency being changed.
        /// </summary>
        public int currencyId;

        /// <summary>
        /// The amount of change to the currency value.
        /// </summary>
        public int changeAmount;

        /// <summary>
        /// Indicates whether the change should be added incrementally or set directly.
        /// </summary>
        public bool addIncrementally;

        /// <summary>
        /// Constructs a new instance of the CurrencyArgs class.
        /// </summary>
        /// <param name="currencyId">The unique ID of the currency being changed.</param>
        /// <param name="changeAmount">The amount of change to the currency value.</param>
        /// <param name="addIncrementally">Indicates whether the change should be added incrementally or set directly.</param>
        public CurrencyArgs(int currencyId, int changeAmount, bool addIncrementally)
        {
            this.currencyId = currencyId;
            this.changeAmount = changeAmount;
            this.addIncrementally = addIncrementally;
        }
    }
}
