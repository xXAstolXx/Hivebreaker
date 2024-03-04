using System;
using UnityEngine;

namespace CSVImporter
{
    public class BaseImportObject : ScriptableObject
    {
        /// <summary>
        /// This method is used to set the data. 
        /// </summary>
        /// <param name="tokens"></param>
        public virtual void SetupFromTokens(string[] tokens)
        {

        }
        
        /// <summary>
        /// Throws an exeption if the tokens length not equal to the intendet row length
        /// </summary>
        /// <param name="tokensLength">Insert your token array length here</param>
        /// <param name="rowLength">Insert the row length it should have</param>
        /// <returns></returns>

        protected void AssertRowLength(int tokensLength, int rowLength)
        {
            if (tokensLength == rowLength) return;
            throw new Exception("Token Length is Unequal to row Length");
        }
    }

}