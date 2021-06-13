using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Core.SharedVariables
{
    public interface IConfigData
    {
        public int VatPercent { get; set; }
        public string DealDocumentPath { get; set; }
    }
}
